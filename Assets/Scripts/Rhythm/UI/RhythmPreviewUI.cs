using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class RhythmPreviewUI : MonoBehaviour
{
    [SerializeField] private RhythmPressUI ui;
    [SerializeField] private RhythmController controller;
    [SerializeField] private RhythmConveyorBeltUI conveyorBelt;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private int countdown = 3;
    [Space]
    [SerializeField] private bool mockInit;
    [Header("Animation objects")]
    [SerializeField] private TMP_Text text;
    [Header("Animation data")]
    [SerializeField] private float textHoldTime = 0.2f;
    [SerializeField] private float textFadeTime = 0.4f;
    [SerializeField] private float textSizeMult = 1.2f;
    [SerializeField] private float textSizeFadeTime = 0.3f;
    [SerializeField] private float fadeTime = 0.1f;

    private List<RhythmResource> resources;
    private int count;

    private Vector2 baseSizeDelta;
    private Sequence arrowSequence;

    private void Start()
    {
        if (!mockInit)
        {
            return;
        }
        Conductor.PlaySong("Beat");
        Sequence delay = DOTween.Sequence();
        delay.AppendInterval(2);
        delay.AppendCallback(() =>
        {
            Init(new List<RhythmResource>()
            {
                new RhythmResource(ResourceType.Gay, new List<RhythmKey>() { RhythmKey.Left, RhythmKey.Up, RhythmKey.Right }),
                new RhythmResource(ResourceType.Luxury, new List<RhythmKey>() { RhythmKey.Right, RhythmKey.Down, RhythmKey.Down }),
                new RhythmResource(ResourceType.Functional, new List<RhythmKey>() { RhythmKey.Up, RhythmKey.Left, RhythmKey.Up }),
            });
        });
    }

    public void Init(List<RhythmResource> resources)
    {
        Debug.Log("[RhythmPreviewUI]: Init! Resources: " + string.Join(", ", resources.ConvertAll(a => a.Type + " - " + string.Join(";", a.keySequence))));

        this.resources = resources;
        baseSizeDelta = text.rectTransform.sizeDelta;
        Conductor.OnBeat += NextNumber;
        count = countdown;

        int sum = 0;
        resources.ForEach(a => sum += a.Count);
        conveyorBelt.Init(Conductor.SongPositionInBeats + countdown + 1, sum);

        gameObject.SetActive(true);
        ui.gameObject.SetActive(true);

        canvasGroup.DOFade(1, 0.2f);
    }

    private void NextNumber(int beat)
    {
        if (count > 0)
        {
            text.text = count.ToString();
            Animate();
        }
        else
        {
            Conductor.OnBeat -= NextNumber;
            text.text = "GO!";
            Animate().onComplete += () => gameObject.SetActive(false);
            controller.Init(resources);
        }
        count--;
    }

    private Sequence Animate()
    {
        if (arrowSequence != null)
        {
            arrowSequence.Kill();
            arrowSequence = null;
        }
        text.alpha = 1;
        arrowSequence = DOTween.Sequence();
        arrowSequence.AppendInterval(textHoldTime);
        arrowSequence.Append(text.DOFade(0, textFadeTime));
        text.rectTransform.sizeDelta = textSizeMult * baseSizeDelta;
        text.rectTransform.DOSizeDelta(baseSizeDelta, textHoldTime + textFadeTime);
        return arrowSequence;
    }
}
