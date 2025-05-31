using System;
using System.Collections.Generic;
using UnityEngine;

public class RhythmInput : MonoBehaviour
{
    private static readonly Dictionary<KeyCode, int> KEYCODE_TO_INDEX = new Dictionary<KeyCode, int>()
    {
        { KeyCode.LeftArrow, 0 },
        { KeyCode.UpArrow, 1},
        { KeyCode.DownArrow, 2},
        { KeyCode.RightArrow, 3},
    };

    public Action<int> OnKeyPressed;

    private void Update()
    {
        foreach (var pair in KEYCODE_TO_INDEX)
        {
            if (Input.GetKeyDown(pair.Key))
            {
                if (Conductor.BeatAccuracy(false) > 0)
                {
                    OnKeyPressed?.Invoke(pair.Value);
                }
            }
        }
    }
}
