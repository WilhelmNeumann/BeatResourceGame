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

    public bool MatchKey(RhythmKey key, int index)
    {
        keySequence[index] = (keySequence[index].Key, keySequence[index].Key == key);
        return keySequence[index].Key == key;
    }
}
