using UnityEngine;
using System;
using System.Collections;

public class FloorAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    public float magicCircleScale = 0.5f;
    public float magicCircleDuration = 1f;
    public GameObject magicCirclePrefab;

    private Rigidbody2D rb;
    private bool hasReachedTarget = false;
    private Vector3 initialPosition;
    private SpriteRenderer[] floorRenderers;

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
            }
        }
    }

    private void SpawnMagicCircle()
    {
        if (magicCirclePrefab != null)
        {
            var magicCircle = Instantiate(magicCirclePrefab, transform.position, Quaternion.identity);
            magicCircle.gameObject.SetActive(true);
            magicCircle.transform.localScale = Vector3.one * magicCircleScale;
            
            // Add SpriteRenderer if it doesn't exist
            var spriteRenderer = magicCircle.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = magicCircle.AddComponent<SpriteRenderer>();
                // Set default sprite if needed
                // spriteRenderer.sprite = Resources.Load<Sprite>("MagicCircle");
            }
            
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = 50; // Magic circle will be drawn at order 50
            }
            
            Destroy(magicCircle, magicCircleDuration);
        }
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