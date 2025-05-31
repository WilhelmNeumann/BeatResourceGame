using UnityEngine;
using UnityEngine.UI;

public class RhythmBeatShipUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [Header("Data")]
    [SerializeField] private float width = 150;

    private int baseBeat;

    public void Init(int baseBeat)
    {
        this.baseBeat = baseBeat;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        image.rectTransform.anchoredPosition = new Vector2(-width * ((Conductor.SongPositionInBeats - baseBeat) + Conductor.TimeSinceLastBeat), 0);
    }
}
