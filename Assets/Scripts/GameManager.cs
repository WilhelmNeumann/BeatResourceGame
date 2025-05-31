using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int gameRounds;
    [SerializeField] private int numberOfResourcesPerRound;
    [SerializeField] private GameObject resourePrefab;

    private List<ResourceObject> resources = new();

    private IEnumerator Start()
    {
       
        var sequence = ResourceGenerator.GenerateResources(5);
    
        foreach (var resourceType in sequence)
        {
            GameObject instance = Instantiate(resourePrefab);
            ResourceObject resource = instance.GetComponent<ResourceObject>();
          
            resource.Init(resourceType);
            resources.Add(resource);
        }
           
    
        yield return null;

    }


    private void Update()
    {

    }
}
