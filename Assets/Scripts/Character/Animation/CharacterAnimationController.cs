using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator; // 캐릭터의 Animator
    [SerializeField] private AnimatorOverrideController overrideController; // 오버라이드 컨트롤러

    public void Init()
    {
        animator = GetComponentInChildren<Animator>();
        // AnimatorOverrideController 생성
        overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

        // 오버라이드 컨트롤러를 Animator에 적용
        animator.runtimeAnimatorController = overrideController;
    }

    public void Play(string animationName, int layer, float normalizedTime)
    {
        animator.Play(animationName, layer, normalizedTime);
    }

    public void Play(string animationName)
    {
        animator.Play(animationName);
        Debug.Log($"Animation name : {animationName}");
    }
    
    
    public void ReplaceAnimation(string originalClipName, AnimationClip newClip)
    {
        // 오리지널 클립을 새 클립으로 오버라이드
        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(overrideController.overridesCount);
        overrideController.GetOverrides(overrides);
        
        for (int i = 0; i < overrides.Count; i++)
        {
            if (overrides[i].Key.name == originalClipName)
            {
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, newClip);
                break;
            }
        }

        overrideController.ApplyOverrides(overrides);
    }

    public float GetAnimationNormalizedTime()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    
    public bool IsAnimationPlaying(string animationName)
    {
        var animInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animInfo.IsName(animationName) && animInfo.normalizedTime >= 1f) return false;
        return true;
    }
}
