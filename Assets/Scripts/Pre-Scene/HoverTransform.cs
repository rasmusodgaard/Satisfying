using DG.Tweening;
using UnityEngine;

public class HoverTransform : MonoBehaviour
{
    [SerializeField]
    float hoverRadius;

    [SerializeField]
    float hoverDuration;

    [SerializeField]
    Ease easeFunction;

    private void Start()
    {
        transform.DOMoveY(transform.position.y + hoverRadius, hoverDuration).SetLoops(-1, LoopType.Yoyo).SetEase(easeFunction);
    }
}
