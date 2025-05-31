using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RhythmOutroUI : MonoBehaviour
{
    [SerializeField] private RhythmController controller;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private List<GameObject> toDisable;
    [SerializeField] private float fadeTime = 0.1f;
    
    public event Action<Dictionary<ResourceType, float>> OnSongOver;

    public void Start()
    {
        controller.OnSongOver += Init;
    }

    public void Init(Dictionary<ResourceType, float> callback)
    {
        Sequence delay = DOTween.Sequence();
        delay.Append(canvasGroup.DOFade(0, fadeTime));
        delay.AppendCallback(() => toDisable.ForEach(a => a.gameObject.SetActive(false)));
        delay.AppendCallback(() => OnSongOver?.Invoke(callback));
    }
}
