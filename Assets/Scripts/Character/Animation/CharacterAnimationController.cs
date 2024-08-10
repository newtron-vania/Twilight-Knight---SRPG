using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator; // 캐릭터의 Animator 컴포넌트
    [SerializeField] private AnimatorOverrideController overrideController; // 오버라이드 컨트롤러
    private readonly Dictionary<string, AnimationClip> originStateClipDict = new(); // 상태 이름과 원래 애니메이션 클립을 저장할 딕셔너리

    public void Init()
    {
        // Animator 컴포넌트를 가져옴
        animator = GetComponentInChildren<Animator>();

        // AnimatorController를 가져옴
        var controller = animator.runtimeAnimatorController as AnimatorController;

        // 만약 AnimatorController가 존재하지 않을 경우 경고 메시지 출력
        if (controller == null) Debug.LogWarning("Animator Controller is Missing!");

        // 첫 번째 레이어의 상태 머신에서 모든 상태를 가져옴
        var states = controller.layers[0].stateMachine.states;

        // 각 상태의 이름과 애니메이션 클립을 딕셔너리에 저장
        foreach (var VARIABLE in states)
            originStateClipDict.Add(VARIABLE.state.name, VARIABLE.state.motion as AnimationClip);

        // AnimatorOverrideController 생성 (현재 사용 중인 Animator Controller를 기반으로)
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

        // 오버라이드 컨트롤러를 Animator에 적용
        animator.runtimeAnimatorController = overrideController;
    }

    // 특정 상태의 애니메이션 클립을 새로운 클립으로 교체하는 메서드
    public void ReplaceClipInState(string stateName, AnimationClip newClip)
    {
        // 오버라이드 컨트롤러의 클립 쌍을 가져옴
        var clipPairs = overrideController.clips;

        // 상태 이름에 해당하는 클립을 찾고, 새로운 클립으로 교체
        foreach (var clipPair in clipPairs)
            // 현재 상태의 원래 클립과 일치하는지 확인
            if (clipPair.originalClip.name == originStateClipDict[stateName].name)
            {
                // 일치하는 클립을 새로운 클립으로 교체
                overrideController[clipPair.originalClip] = newClip;
                Debug.Log($"{stateName} 상태의 애니메이션 클립이 {newClip.name}으로 변경되었습니다.");
                return;
            }
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