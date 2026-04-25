using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private RawImage scrollingImage;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;


    void Update()
    {
        scrollingImage.uvRect = new Rect(scrollingImage.uvRect.position + new Vector2(xOffset, yOffset) * Time.deltaTime, scrollingImage.uvRect.size);
    }
}
