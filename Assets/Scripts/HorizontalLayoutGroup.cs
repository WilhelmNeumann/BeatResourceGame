using UnityEngine;

public class HorizontalLayoutGroup : MonoBehaviour
{
    [SerializeField] private float spacing = 2f;
    [SerializeField] private bool center = true;

    public void LayoutChildren()
    {
        int count = transform.childCount;
        float totalWidth = (count - 1) * spacing;
        float startX = center ? -totalWidth / 2f : 0f;

        for (int i = 0; i < count; i++)
        {
            Transform child = transform.GetChild(i);
            Vector3 newPos = new Vector3(startX + i * spacing, 0, 0);
            child.localPosition = newPos;
        }
    }
}

