using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AnimancerComponent))]
public class RandomOneShotAnimationController : MonoBehaviour
{
    AnimancerComponent animancer;

    [SerializeField]
    List<AnimationClip> animationClips = new List<AnimationClip>();

    [SerializeField]
    float minWaitTime;

    [SerializeField]
    float maxWaitTime;

    private void Awake()
    {
        animancer = GetComponent<AnimancerComponent>();
    }

    private void Start()
    {
        StartNewWaitAndPlay();
    }

    void StartNewWaitAndPlay()
    {
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        AnimationClip animationClip = animationClips[Random.Range(0, animationClips.Count)];
        StartCoroutine(WaitAndPlayAnimationCoroutine(waitTime, animationClip));
    }

    IEnumerator WaitAndPlayAnimationCoroutine(float waitTime, AnimationClip animationClip)
    {
        yield return new WaitForSeconds(waitTime);
        AnimancerState state = animancer.Play(animationClip);
        yield return new WaitUntil(() => state.NormalizedTime >= 1);
        state.Stop();
        state.NormalizedTime = 0;
        StartNewWaitAndPlay();
    }
}
