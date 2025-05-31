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
    [SerializeField] private RhythmPreviewUI rhythmStart;

    [SerializeField] private Vector3 centerPosition = new Vector3(0, 0, 0);
    [SerializeField] private float animationDuration = 0.5f;

    private float cardSpacing = 3.5f; // space between cards

    private List<ResourceObject> resources;

    private ResourceObject _currentResourcePlaying;

    private int currentResourceIndex = 0;

    private bool isPlaying = false;

    private Dictionary<ResourceType, List<RhythmKey>> arrows;

    private List<RhythmKey> GenerateRandomList()
    {
        List<RhythmKey> result = new List<RhythmKey>();
        Array rhythmKeyValues = Enum.GetValues(typeof(RhythmKey));
        int enumLength = rhythmKeyValues.Length;

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, enumLength); // 0 to 3
            RhythmKey key = (RhythmKey)rhythmKeyValues.GetValue(randomIndex);
            result.Add(key);
        }

        return result;
    }

    private IEnumerator Start() {
        for(var i = 0; i < 7; i++) {
            yield return PlayRound();
        }
    }

    private IEnumerator PlayRound()
    {
        Conductor.PlaySong("Beat");

        arrows = new Dictionary<ResourceType, List<RhythmKey>>
        {
            { ResourceType.Gay, GenerateRandomList() },
            { ResourceType.Luxury, GenerateRandomList() },
            { ResourceType.Functional, GenerateRandomList() }
        };

        resources = InstantiateResources(4);


        yield return AppearWithAnimation(resources);

        Conductor.OnBeat += OnBeat;

        isPlaying = true;
        yield return new WaitWhile(() => isPlaying);

        Conductor.OnBeat -= OnBeat;
        yield return DisappearWithAnimation(resources);

        rhythmStart.Init(resources.ConvertAll(a => new RhythmResource(a.resourceType, a.Sequence)));
    }

    private List<ResourceObject> InstantiateResources(int amount) {
        var sequence = ResourceGenerator.GenerateResources(amount);
        resources = new List<ResourceObject>();
        foreach (var resourceType in sequence)
        {
            GameObject instance = Instantiate(resourePrefab, ResourceObject.OffscreenPosition, Quaternion.identity);
            ResourceObject resource = instance.GetComponent<ResourceObject>();
          

            List<RhythmKey> keys = arrows[resourceType];
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
            yield return new WaitForSeconds(.2f);
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
