using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ResourceObject : MonoBehaviour
{
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowsContainer;

    public static Vector3 OffscreenPosition = new Vector3(10, -10, 0);

    public void Init(ResourceType resourceType, List<RhythmKey> sequence) {
        this.resourceType = resourceType;

        switch (resourceType)
        {
            case ResourceType.Gay:
                sprite.sprite = sprites.Length > 0 ? sprites[0] : null;
                break;
            case ResourceType.Luxury:
                sprite.sprite = sprites.Length > 1 ? sprites[1] : null;
                break;
            case ResourceType.Functional:
                sprite.sprite = sprites.Length > 2 ? sprites[2] : null;
                break;
            default:
                Debug.LogWarning("Unknown resource type: " + resourceType);
                break;
        }

        // Layout arrows in a centered horizontal row
        float arrowSpacing = 0.7f; // Distance between arrows
        int arrowCount = sequence.Count;
        float totalWidth = (arrowCount - 1) * arrowSpacing;

        for (int i = 0; i < arrowCount; i++)
        {
            GameObject arrow = Instantiate(arrowPrefab, arrowsContainer);
            Transform arrowTransform = arrow.transform;

            float xOffset = i * arrowSpacing - totalWidth / 2f;
            arrowTransform.localPosition = new Vector3(xOffset, -1f, 0f); // Adjust Y if needed

            var key = sequence[i];
            // Set rotation based on direction
            switch (key)
            {
                case RhythmKey.Up:
                    arrowTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);     // Default pointing up
                    break;
                case RhythmKey.Right:
                    arrowTransform.localRotation = Quaternion.Euler(0f, 0f, -90f);
                    break;
                case RhythmKey.Down:
                    arrowTransform.localRotation = Quaternion.Euler(0f, 0f, 180f);
                    break;
                case RhythmKey.Left:
                    arrowTransform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                    break;
                default:
                    arrowTransform.localRotation = Quaternion.identity;
                    break;
            }
        }
    }

    public void Shake() {

    }


    private void Update()
    {

    }
}
