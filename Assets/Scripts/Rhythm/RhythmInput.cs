using System;
using System.Collections.Generic;
using UnityEngine;

public class RhythmInput : MonoBehaviour
{
    private static readonly Dictionary<KeyCode, RhythmKey> KEYCODE_TO_INDEX = new Dictionary<KeyCode, RhythmKey>()
    {
        { KeyCode.LeftArrow, RhythmKey.Left },
        { KeyCode.UpArrow, RhythmKey.Up },
        { KeyCode.DownArrow, RhythmKey.Down },
        { KeyCode.RightArrow, RhythmKey.Right },
    };

    public event Action<RhythmKey> OnKeyPressed;

    private void Update()
    {
        foreach (var pair in KEYCODE_TO_INDEX)
        {
            if (Input.GetKeyDown(pair.Key))
            {
                //if (Conductor.BeatAccuracy(false) > 0)
                //{
                    OnKeyPressed?.Invoke(pair.Value);
                    SoundController.PlaySound((int)pair.Value, false);
                //}
            }
        }
    }
}
