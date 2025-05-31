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
        { RhythmKey.Left, 0 + 180 },
        { RhythmKey.Up, 270 + 180 },
        { RhythmKey.Down, 90 + 180 },
        { RhythmKey.Right, 180 + 180 },
    };
    [SerializeField] private RectTransform holder;
    [SerializeField] private CanvasGroup arrow;
    [SerializeField] private RhythmPressBoomUI boom;
    [Header("Animation")]
    [SerializeField] private float arrowHoldTime = 0.2f;
    [SerializeField] private float arrowFadeTime = 0.4f;
    [SerializeField] private float holderSizeMult = 1.2f;
    [SerializeField] private float holderSizeFadeTime = 0.3f;

    private Vector2 baseSizeDelta;
    private Sequence arrowSequence;

    public void Init(RhythmInput input)
    {
        baseSizeDelta = holder.sizeDelta;
        input.OnKeyPressed += AnimateKeyPress;
    }

    private void AnimateKeyPress(RhythmKey key)
    {
        //text.text = KEY_TO_TEXT[key];
        arrow.transform.localEulerAngles = Vector3.forward * KEY_TO_ROT[key];
        arrow.alpha = 1;
        if (arrowSequence != null)
        {
            arrowSequence.Kill();
            arrowSequence = null;
        }
        arrowSequence = DOTween.Sequence();
        arrowSequence.AppendInterval(arrowHoldTime);
        arrowSequence.Append(arrow.DOFade(0, arrowFadeTime));
        holder.sizeDelta = holderSizeMult * baseSizeDelta;
        holder.DOSizeDelta(baseSizeDelta, holderSizeFadeTime);
        boom.Activate();
    }
}
