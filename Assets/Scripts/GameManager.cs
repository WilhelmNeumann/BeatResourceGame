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

    private float cardSpacing = 3; // space between cards

    private GameState _gameState;

    private List<ResourceObject> resources;

    private ResourceObject _currentResourcePlaying;

    private IEnumerator Start()
    {
        resources = InstantiateResources(5);

        _gameState = GameState.Listening;
        yield return AppearWithAnimation(resources);

        _gameState = GameState.Playing;
        yield return Play(resources);

        _gameState = GameState.Building;
        // yield return DisappearWithAnimation(resources);
    }

    private List<ResourceObject> InstantiateResources(int amount) {
        var sequence = ResourceGenerator.GenerateResources(amount);
        resources = new();
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
    
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeStrength = 0.3f;
    [SerializeField] private int vibrato = 10;

    public IEnumerator Play(List<ResourceObject> resources)
    {
        int count = resources.Count;

        for (int i = 0; i < count; i++)
        {
            Transform t = resources[i].transform;

            // Shake position slightly (local or world, depending on layout)
            t.DOShakePosition(shakeDuration, strength: shakeStrength, vibrato: vibrato, randomness: 90, snapping: false, fadeOut: true);

            // Shake scale
            t.DOShakeScale(shakeDuration, strength: 0.2f, vibrato: vibrato, randomness: 90, fadeOut: true);

            yield return new WaitForSeconds(0.1f); // Delay between each resource
        }

        yield return new WaitForSeconds(shakeDuration);
    }


    private void Update()
    {

    }
}
