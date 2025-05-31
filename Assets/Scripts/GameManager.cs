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

    [SerializeField] private Vector3 centerPosition = new Vector3(0, 0, 0);
    [SerializeField] private float animationDuration = 0.5f;

    private float cardSpacing = 3.5f; // space between cards

    private List<ResourceObject> resources;

    private ResourceObject _currentResourcePlaying;

    private int currentResourceIndex = 0;

    private bool isPlaying = false;

    private IEnumerator Start()
    {
        resources = InstantiateResources(5);


        yield return AppearWithAnimation(resources);

        Conductor.OnBeat += OnBeat;
    
        isPlaying = true;
        yield return new WaitWhile(() => isPlaying);

        Conductor.OnBeat -= OnBeat;
        yield return DisappearWithAnimation(resources);
    }

    private List<ResourceObject> InstantiateResources(int amount) {
        var sequence = ResourceGenerator.GenerateResources(amount);
        resources = new List<ResourceObject>();
        foreach (var resourceType in sequence)
        {
            GameObject instance = Instantiate(resourePrefab, ResourceObject.OffscreenPosition, Quaternion.identity);
            ResourceObject resource = instance.GetComponent<ResourceObject>();
          

            List<RhythmKey> keys = new () {RhythmKey.Left, RhythmKey.Up, RhythmKey.Down};
            resource.Init(resourceType, keys);
            resources.Add(resource);
        }

        return resources;
    }


    public IEnumerator AppearWithAnimation(List<ResourceObject> resources)
    {
        int count = resources.Count;
        float totalWidth = (count - 1) * cardSpacing;

        for (int i = 0; i < count; i++)
        {
            float xOffset = i * cardSpacing - totalWidth / 2;
            Vector3 targetPosition = centerPosition + new Vector3(xOffset, 0, 0);
            Transform t = resources[i].transform;
            t.DOMove(targetPosition, animationDuration).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(animationDuration);
    }

    public IEnumerator DisappearWithAnimation(List<ResourceObject> resources)
    {
        int count = resources.Count;
        float totalWidth = (count - 1) * cardSpacing;

        for (int i = 0; i < count; i++)
        {
            float xOffset = i * cardSpacing - totalWidth / 2;
            Vector3 targetPosition = centerPosition + new Vector3(xOffset, 0, 0);
            Transform t = resources[i].transform;
            t.DOMove(ResourceObject.OffscreenPosition, animationDuration).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(animationDuration);
    }


    private void OnBeat(int _)
    {
        if (currentResourceIndex >= resources.Count)
        {
            // All resources are done
            isPlaying = false;
            return;
        }

        var currentResource = resources[currentResourceIndex];

        var resourceBeatIsFinished = currentResource.Beat();
        if(resourceBeatIsFinished) {
            currentResourceIndex++;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnBeat(0); // or any index you want to test
        }
    }
}
