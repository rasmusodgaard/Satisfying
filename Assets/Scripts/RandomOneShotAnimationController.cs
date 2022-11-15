using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RandomOneShotAnimationController : MonoBehaviour
{
    Animator animator;

    [SerializeField]
    List<AnimationClip> animationClips = new List<AnimationClip>();

    [SerializeField]
    float minWaitTime;

    [SerializeField]
    float maxWaitTime;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //StartNewWaitAndPlay();
        //float waitTime = Random.Range(minWaitTime, maxWaitTime);
        //AnimationClip animationClip = animationClips[Random.Range(0, animationClips.Count - 1)];
        //animator.clip = animationClip;
    }

    void StartNewWaitAndPlay()
    {
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        AnimationClip animationClip = animationClips[Random.Range(0, animationClips.Count - 1)];
        //animator.clip = animationClip;
        StartCoroutine(WaitAndPlayAnimationCoroutine(waitTime, animationClip));
    }

    IEnumerator WaitAndPlayAnimationCoroutine(float waitTime, AnimationClip animationClip)
    {
        yield return new WaitForSeconds(waitTime);
        //animator.Play();
        //yield return new WaitUntil(() => animator.isPlaying == false);
        StartNewWaitAndPlay();
    }
}
