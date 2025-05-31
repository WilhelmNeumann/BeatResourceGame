using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RhythmController : MonoBehaviour
{
    [SerializeField] private RhythmInput rhythmInput;
    [SerializeField] private string songName;

    public Action<Dictionary<ResourceType, float>> OnSongOver;
    public Action<RhythmKey, bool> OnKeyMatch;

    private List<int> pastTimestamps = new List<int>();
    private List<RhythmResource> resources;
    private int totalLength;

    private void Start()
    {
        rhythmInput.OnKeyPressed += ValidateKey;
    }

    private void Update()
    {
        if (Conductor.SongPositionInBeats >= totalLength)
        {
            Dictionary<ResourceType, (float Acc, int Sum)> midResult = new Dictionary<ResourceType, (float Acc, int Sum)>();
            Dictionary<ResourceType, float> result = new Dictionary<ResourceType, float>();
            resources.ForEach(a => midResult.AddOrSet(a.Type, (midResult.SafeGet(a.Type).Acc + a.Accuracy, midResult.SafeGet(a.Type).Sum + 1)));
            midResult.ForEach((key, value) => result.Add(key, value.Acc / value.Sum));
            OnSongOver?.Invoke(result);
            gameObject.SetActive(false);
            Debug.Log("[RhythmController]: FINAL RESULT: " + string.Join(", ", result.Keys.ToList().ConvertAll(key => key + " - " + result[key])));
        }
    }

    public void Init(List<RhythmResource> resources)
    {
        this.resources = resources;
        Conductor.PlaySong(songName);
    }

    private void ValidateKey(RhythmKey key)
    {
        if (pastTimestamps.Contains(Conductor.SongPositionInBeats))
        {
            return;
        }
        if (Conductor.SongPositionInBeats >= totalLength)
        {
            Debug.LogWarning("[RhythmController]: Pressed key after song is over - might be okay if it's exactly the last frame");
            return;
        }
        pastTimestamps.Add(Conductor.SongPositionInBeats);
        int index = Conductor.SongPositionInBeats, i = 0;
        while (index >= resources[i].Count && ++i < resources.Count)
        {
            index -= resources[i - 1].Count;
        }
        if (i >= resources.Count)
        {
            Debug.LogError("[RhythmController]: Too many keys!");
            return;
        }
        bool success = resources[i].MatchKey(key, index);
        OnKeyMatch?.Invoke(key, success);
        Debug.Log("[RhythmController]: " + (success ? "SUCCESS: " : "FAIL   : ") + key + " at pos " + index + " of resource " + i + ", a " + resources[i].Type);
    }
}
