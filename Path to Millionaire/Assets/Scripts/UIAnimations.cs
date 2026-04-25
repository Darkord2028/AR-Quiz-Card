using NaughtyAttributes;
using UnityEngine;

public class UIAnimations : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] RectTransform topImageRect;
    [SerializeField] RectTransform bottomImageRect;

    [Header("Clerp")]
    [SerializeField] Vector2 topImageDefaultAnchor;
    [SerializeField] Vector2 topImageAfterAnchor;
    [SerializeField] Vector2 bottomImageDefaultAnchor;
    [SerializeField] Vector2 bottomImageAfterAnchor;
    public float tweenTime = 5.0f;
    [SerializeField] LeanTweenType tweenTypeOnCut;
    [SerializeField] LeanTweenType tweenTypeOnAction;

    private void Start()
    {
        
    }

    public void Cut(bool stay)
    {
        CutTransition(topImageDefaultAnchor.y, topImageAfterAnchor.y, bottomImageDefaultAnchor.y, bottomImageAfterAnchor.y, tweenTypeOnCut);
        if (!stay) LeanTween.delayedCall(tweenTime + 0.1f, Action);
    }

    private void Action()
    {
        CutTransition(topImageAfterAnchor.y, topImageDefaultAnchor.y, topImageAfterAnchor.y, bottomImageDefaultAnchor.y, tweenTypeOnAction);
    }

    private void CutTransition(float topTransitionFrom, float topTransitionTo, float bottomTransitionFrom, float bottomTransitionTo, LeanTweenType tweenType)
    {
        LeanTween.value(topImageRect.gameObject, topTransitionFrom, topTransitionTo, tweenTime).setEase(tweenType)
            .setOnUpdate((float value) =>
            {
                Vector2 offset = topImageRect.anchorMax;
                offset.y = value;
                topImageRect.anchorMax = offset;
            });
        LeanTween.value(bottomImageRect.gameObject, bottomTransitionFrom, bottomTransitionTo, tweenTime).setEase(tweenType)
            .setOnUpdate((float value) =>
            {
                Vector2 offset = bottomImageRect.anchorMin;
                offset.y = value;
                bottomImageRect.anchorMin = offset;
            });
    }

}
