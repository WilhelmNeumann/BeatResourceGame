using UnityEngine;
using UnityEngine.UI;

public class RhythmPressBoomUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float speed;
    [SerializeField] private float maxSize;

    private Vector3 initSize;
    private float count;

    public void Activate()
    {
        GameObject clone = Instantiate(gameObject, transform.parent);
        //clone.GetComponent<RhythmPressBoomUI>().color = accuracy > 0 ? Colors[Mathf.FloorToInt(Mathf.Abs(accuracy - 0.00001f) * (Colors.Count - 1)) + 1] : Colors[0];
        clone.SetActive(true);
    }

    private void Awake()
    {
        initSize = transform.localScale;
    }

    private void Update()
    {
        count += Time.deltaTime * speed;
        if (count < 1)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1 - count);
            transform.localScale = initSize * (1 + count * (maxSize - 1));
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
