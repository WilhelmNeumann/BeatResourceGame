using UnityEngine;
using System.Collections.Generic;
using System;

public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private RhythmOutroUI rhythmOutro;

    [Header("Floor Objects")]
    [SerializeField] private GameObject floor1;
    [SerializeField] private GameObject floor2;
    [SerializeField] private GameObject floor3;
    [SerializeField] private GameObject floor4;

    [Header("Settings")]
    public float spawnHeight = 10f;
    public float accuracyThreshold = 0.3f; // Threshold for considering a resource type successful

    public event Action OnFinishedAnimation;

    private static int currentFloorIndex = 0;

    private void Start()
    {
        // Initial test floor
        //BuildNextFloor(new Dictionary<ResourceType, float>());
        rhythmOutro.OnSongOver += BuildNextFloor;
    }

    public void BuildNextFloor(Dictionary<ResourceType, float> result)
    {
        GameObject floorToBuild = GetNextFloor();
        if (floorToBuild != null)
        {
            // Set visibility of child objects based on accuracy
            Transform basicShape = floorToBuild.transform.Find("BasicShape");
            Transform functional = floorToBuild.transform.Find("Functional");
            Transform gay = floorToBuild.transform.Find("Gay");
            Transform gold = floorToBuild.transform.Find("Gold");

            if (basicShape != null && functional != null)
            {
                bool isFunctional = result.ContainsKey(ResourceType.Functional) && result[ResourceType.Functional] >= accuracyThreshold;
                basicShape.gameObject.SetActive(!isFunctional);
                functional.gameObject.SetActive(isFunctional);
            }

            if (gold != null)
            {
                bool isLuxury = result.ContainsKey(ResourceType.Luxury) && result[ResourceType.Luxury] >= accuracyThreshold;
                gold.gameObject.SetActive(isLuxury);
            }

            if (gay != null)
            {
                bool isGay = result.ContainsKey(ResourceType.Gay) && result[ResourceType.Gay] >= accuracyThreshold;
                gay.gameObject.SetActive(isGay);
            }

            var floorAnimation = floorToBuild.GetComponent<FloorAnimation>();
            if (floorAnimation == null)
            {
                floorAnimation = floorToBuild.AddComponent<FloorAnimation>();
            }            
                floorAnimation.OnFinishedAnimation += () => OnFinishedAnimation?.Invoke();
            
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
