using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RhythmController : MonoBehaviour
{
    [SerializeField] private RhythmInput rhythmInput;
    [SerializeField] private RhythmPressUI ui;
    [SerializeField] private string songName;
    [Space]
    [SerializeField] private bool mockInit;

    public event Action<Dictionary<ResourceType, float>> OnSongOver;
    public event Action<RhythmKey, bool> OnKeyMatch;

    private List<int> pastTimestamps = new List<int>();
    private List<RhythmResource> resources;
    private int totalLength;
    private int startBeat;

    private int Beat => Conductor.SongPositionInBeats - startBeat;

    private void Start()
    {
        if (!mockInit)
        {
            return;
        }
        Init(new List<RhythmResource>()
        {
            new RhythmResource(ResourceType.Gay, new List<RhythmKey>() { RhythmKey.Left, RhythmKey.Up, RhythmKey.Right }),
            new RhythmResource(ResourceType.Luxury, new List<RhythmKey>() { RhythmKey.Right, RhythmKey.Down, RhythmKey.Down }),
            new RhythmResource(ResourceType.Functional, new List<RhythmKey>() { RhythmKey.Up, RhythmKey.Left, RhythmKey.Up }),
        });
    }

    private void CheckFinalBeat(int beat)
    {
        if (Beat >= totalLength)
        {
            rhythmInput.OnKeyPressed -= ValidateKey;
            Conductor.OnBeat -= CheckFinalBeat;
            Dictionary<ResourceType, (float Acc, int Sum)> midResult = new Dictionary<ResourceType, (float Acc, int Sum)>();
            Dictionary<ResourceType, float> result = new Dictionary<ResourceType, float>();
            resources.ForEach(a => midResult.AddOrSet(a.Type, (midResult.SafeGet(a.Type).Acc + a.Accuracy, midResult.SafeGet(a.Type).Sum + 1)));
            midResult.ForEach((key, value) => result.Add(key, value.Acc / value.Sum));
            OnSongOver?.Invoke(result);
            gameObject.SetActive(false);
            ui.gameObject.SetActive(false);
            Debug.Log("[RhythmController]: FINAL RESULT: " + string.Join(", ", result.Keys.ToList().ConvertAll(key => key + " - " + result[key])));
        }
    }

    public void Init(List<RhythmResource> resources)
    {
        rhythmInput.OnKeyPressed += ValidateKey;
        Conductor.OnBeat += CheckFinalBeat;
        this.resources = resources;
        resources.ForEach(a => totalLength += a.Count);
        startBeat = Conductor.SongPositionInBeats;
        //Conductor.PlaySong(songName);
        ui.Init(rhythmInput);
    }

    private void ValidateKey(RhythmKey key)
    {
        if (pastTimestamps.Contains(Beat))
        {
            return;
        }
        if (Beat >= totalLength)
        {
            Debug.LogWarning("[RhythmController]: Pressed key after song is over - might be okay if it's exactly the last frame");
            return;
        }
        pastTimestamps.Add(Beat);
        int index = Beat, i = 0;
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
        Debug.Log("[RhythmController]: " + (success ? "SUCCESS: " : "FAIL          : ") + key + " / " + resources[i].GetKey(index) + " at pos " + index + " of resource " + i + ", a " + resources[i].Type);
    }
}
