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

    public void Init(ResourceType resourceType) {
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
    }


    private void Update()
    {

    }
}
