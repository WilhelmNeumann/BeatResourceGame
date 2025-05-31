using UnityEngine;

public class RhythmConveyorBeltUI : MonoBehaviour
{
    [SerializeField] private RhythmBeatShipUI baseShip;

    public void Init(int start, int count)
    {
        for (int i = 0; i < count; i++)
        {
            RhythmBeatShipUI ship = Instantiate(baseShip, baseShip.transform.parent);
            ship.Init(start + i);
        }
        gameObject.SetActive(true);
    }
}
