using UnityEngine;
using System;
using System.Collections;

public class FloorAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    public float magicCircleScale = 0.5f;
    public float magicCircleDuration = 1f;
    public GameObject magicCirclePrefab;
    public Vector3 magicCircleOffset = new Vector3(-0.5f, 0f, 0f); // Offset to move animation left

    private Rigidbody2D rb;
    private bool hasReachedTarget = false;
    private Vector3 initialPosition;
    private SpriteRenderer[] floorRenderers;

    public event Action OnFinishedAnimation;

    public void StartFalling(Vector3 spawnPosition)
    {
        rb = GetComponent<Rigidbody2D>();
        hasReachedTarget = false;
        initialPosition = transform.position;
        transform.position = spawnPosition;
        gameObject.SetActive(true);

        // Set sorting order for all floor renderers
        floorRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var renderer in floorRenderers)
        {
            renderer.sortingOrder = 10; // Floors will be drawn at order 10
        }

        if (rb != null)
        {
            Debug.Log("rb.gravityScale = 1f;");
            rb.gravityScale = 1f;
        }
    }

    private void Update()
    {
        if (!hasReachedTarget && rb != null)
        {
            // Check if floor has reached its initial position
            if (Mathf.Abs(transform.position.y - initialPosition.y) < 0.1f)
            {
                Debug.Log("Mathf.Abs(transform.position.y - initialPosition.y) < 0.1f");
                hasReachedTarget = true;
                rb.gravityScale = 0f;
                rb.linearVelocity = Vector2.zero;
                transform.position = initialPosition;
                SpawnMagicCircle();
                OnFinishedAnimation?.Invoke();
            }
        }
    }

    private void SpawnMagicCircle()
    {
        if (magicCirclePrefab == null)
        {
            Debug.LogError("Magic circle prefab is not assigned!");
            return;
        }

        Debug.Log("Spawning magic circle");
        // Add offset to position
        Vector3 spawnPosition = transform.position + magicCircleOffset;
        var magicCircle = Instantiate(magicCirclePrefab, spawnPosition, Quaternion.identity);
        magicCircle.gameObject.SetActive(true);
        magicCircle.transform.localScale = Vector3.one * magicCircleScale;

        // Set renderer settings for all particle systems
        var particleSystems = magicCircle.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in particleSystems)
        {
            var renderer = ps.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
            {
                renderer.sortingOrder = 50; // Set high sorting order for all particle systems
            }
        }
        
        Destroy(magicCircle, magicCircleDuration);
    }

    private IEnumerator TestAnimationCoroutine()
    {
        Vector3 startPos = transform.position + Vector3.up * 10f;
        
        for (int i = 0; i < 5; i++)
        {
            transform.position = startPos;
            hasReachedTarget = false;
            
            StartFalling(transform.position - Vector3.up * 10f);
            
            yield return new WaitForSeconds(2f);
        }
    }
} 