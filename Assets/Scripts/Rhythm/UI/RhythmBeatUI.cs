using UnityEngine;
using UnityEngine.UI;

public class RhythmBeatUI : MonoBehaviour
{
    public Image BeatIcon;
    public Image PlanetIcon;
    [Header("Animation")]
    public Vector2 BeatSizeRange;
    public float BeatTime = 0.25f;
    private Vector2 baseSizeDelta;
    private Color basePlanetColor;

    private void Start()
    {
        baseSizeDelta = BeatIcon.rectTransform.sizeDelta;
        basePlanetColor = PlanetIcon?.color ?? BeatIcon.color;
    }

    private void Update()
    {
        float percent = BeatSizeRange.x + Mathf.Max(0, BeatTime - Conductor.TimeSinceLastBeat) * (BeatSizeRange.y - BeatSizeRange.x);
        //BeatIcon.color = new Color(basePlanetColor.r, basePlanetColor.g, basePlanetColor.b, Mathf.Max(0, 1 - Conductor.TimeSinceLastBeat * 2));
        BeatIcon.rectTransform.sizeDelta = baseSizeDelta * percent;
        //if (PlanetIcon != null)
        //{
        //    PlanetIcon.color = basePlanetColor * Mathf.Max(1, 1.2f - Conductor.TimeSinceLastBeat);
        //}
        BeatIcon.color = basePlanetColor * percent;
    }
}
