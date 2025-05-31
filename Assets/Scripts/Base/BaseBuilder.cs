using UnityEngine;

public class BaseBuilder : MonoBehaviour
{
    [Header("Floor Objects")]
    [SerializeField] private GameObject floor1;
    [SerializeField] private GameObject floor2;
    [SerializeField] private GameObject floor3;
    [SerializeField] private GameObject floor4;

    [Header("Settings")]
    public float spawnHeight = 10f;

    private static int currentFloorIndex = 0;

    private void Start()
    {
            BuildNextFloor(true, false, false);
    }

    public void BuildNextFloor(bool isGay, bool isLuxury, bool isFunctional)
    {
        GameObject floorToBuild = GetNextFloor();
        if (floorToBuild != null)
        {
            // Set visibility of child objects
            Transform basicShape = floorToBuild.transform.Find("BasicShape");
            Transform functional = floorToBuild.transform.Find("Functional");
            Transform gay = floorToBuild.transform.Find("Gay");
            Transform gold = floorToBuild.transform.Find("Gold");

            if (basicShape != null && functional != null)
            {
                basicShape.gameObject.SetActive(!isFunctional);
                functional.gameObject.SetActive(isFunctional);
            }

            if (gold != null)
            {
                gold.gameObject.SetActive(isLuxury);
            }

            if (gay != null)
            {
                gay.gameObject.SetActive(isGay);
            }

            var floorAnimation = floorToBuild.GetComponent<FloorAnimation>();
            if (floorAnimation == null)
            {
                floorAnimation = floorToBuild.AddComponent<FloorAnimation>();
            }            
            
            Vector3 spawnPosition = floorToBuild.transform.position + Vector3.up * spawnHeight;
            floorAnimation.StartFalling(spawnPosition);

            currentFloorIndex++;
        }
    }

    private GameObject GetNextFloor()
    {
        switch (currentFloorIndex)
        {
            case 0: return floor1;
            case 1: return floor2;
            case 2: return floor3;
            case 3: return floor4;
            default: return null;
        }
    }
}
