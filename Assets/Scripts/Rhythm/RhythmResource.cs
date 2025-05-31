using System.Collections.Generic;
using UnityEngine;

public class RhythmResource
{
    public ResourceType Type { get; private set; }

    private List<(RhythmKey Key, bool Pressed)> keySequence { get; } = new List<(RhythmKey key, bool pressed)>();

    public int Count => keySequence.Count;
    public float Accuracy => keySequence.FindAll(a => a.Pressed).Count / (float)keySequence.Count;

    public RhythmResource(ResourceType type, List<RhythmKey> sequence)
    {
        Type = type;
        sequence.ForEach(a => keySequence.Add((a, false)));
    }

    public void MatchKey(RhythmKey key, int index)
    {
        if (keySequence[index].Key == key)
        {
            keySequence[index] = (key, true);
        }
    }
}
