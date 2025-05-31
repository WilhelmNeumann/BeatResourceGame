using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int gameRounds;
    [SerializeField] private int numberOfResourcesPerRound;
    [SerializeField] private GameObject resourePrefab;
    [SerializeField] private float cardSpacing = 2f; // space between cards
    [SerializeField] private Vector3 centerPosition = new Vector3(0, 0, 0);
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float startY = -10f;

    private List<ResourceObject> resources = new();

    private IEnumerator Start()
    {
        yield return SpawnResources();

    }


    private IEnumerator SpawnResources() {
        var sequence = ResourceGenerator.GenerateResources(5);
    
        foreach (var resourceType in sequence)
        {
            GameObject instance = Instantiate(resourePrefab, ResourceObject.OffscrenPosition, Quaternion.identity);
            ResourceObject resource = instance.GetComponent<ResourceObject>();
          
            resource.Init(resourceType);
            resources.Add(resource);
        }

        yield return AnimateResourcesIntoView(resources);
    }
   

    public IEnumerator AnimateResourcesIntoView(List<ResourceObject> resources)
    {
        int count = resources.Count;
        float totalWidth = (count - 1) * cardSpacing;

        for (int i = 0; i < count; i++)
        {
            float xOffset = i * cardSpacing - totalWidth / 2;
            Vector3 targetPosition = centerPosition + new Vector3(xOffset, 0, 0);
            Transform t = resources[i].transform;

            t.position = new Vector3(targetPosition.x, startY, targetPosition.z);
            t.DOMove(targetPosition, animationDuration).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(animationDuration);
    }
    

    private void Update()
    {

    }
}
