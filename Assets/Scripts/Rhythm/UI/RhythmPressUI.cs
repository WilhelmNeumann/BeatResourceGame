using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RhythmPressUI : MonoBehaviour
{
    private static readonly Dictionary<RhythmKey, string> KEY_TO_TEXT = new Dictionary<RhythmKey, string>()
    {
        { RhythmKey.Left, "←" },
        { RhythmKey.Up, "↑" },
        { RhythmKey.Down, "↓" },
        { RhythmKey.Right, "→" },
    };
    private static readonly Dictionary<RhythmKey, float> KEY_TO_ROT = new Dictionary<RhythmKey, float>()
    {
        { RhythmKey.Left, 0 },
        { RhythmKey.Up, 270 },
        { RhythmKey.Down, 90 },
        { RhythmKey.Right, 180 },
    };

    [SerializeField] private RectTransform holder;
    [SerializeField] private CanvasGroup arrow;
    [SerializeField] private RhythmInput input;
    [Header("Animation")]
    [SerializeField] private float arrowHoldTime = 0.2f;
    [SerializeField] private float arrowFadeTime = 0.4f;
    [SerializeField] private float holderSizeMult = 1.2f;
    [SerializeField] private float holderSizeFadeTime = 0.3f;

    private void Start()
    {
        input.OnKeyPressed += AnimateKeyPress;
    }

    private void AnimateKeyPress(RhythmKey key)
    {
        //text.text = KEY_TO_TEXT[key];
        arrow.transform.localEulerAngles = Vector3.forward * KEY_TO_ROT[key];
        Sequence arrowSequence = DOTween.Sequence();
        //arrowSequence.Append()
        
    }
}
