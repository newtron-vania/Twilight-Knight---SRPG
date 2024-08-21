using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UI_CharacterCommand : UI_Popup
{
    
    private enum Buttons
    {
        Button_Attack,
        Button_Defense,
        Button_Move,
        Button_Skill,
    }
    
    private enum Images
    {
        Background,
        CommandPosition
    }
    
    public float radius = 1000f; // 원의 반지름
    public float duration = 1f; // 애니메이션 지속 시간

    
    public override Define.PopupUIGroup PopupID { get { return Define.PopupUIGroup.UI_CharacterCommand; } }


    public override void Init()
    {
        base.Init();
        
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        
        Get<Image>((int)Images.Background).gameObject.AddUIEvent(DisableUI, Define.UIEvent.Click);
        foreach (var VARIABLE in Enum.GetValues(typeof(Buttons)))
        {
            Get<Button>((int)VARIABLE).gameObject.AddUIEvent(UseCommand);
        }
        
        // 애니메이션 실시
        CircleRound();
    }

    private void CircleRound()
    {
        RectTransform container = Get<Image>((int)Images.CommandPosition).rectTransform;
        int childCount = container.childCount; // 자식 오브젝트의 개수
        if (childCount == 0) return;

        // 원형 궤도를 계산하기 위해 경로를 생성합니다.
        Vector3[] path = new Vector3[childCount];
        float angleStep = 360f / childCount;

        // 경로를 원형 궤도로 설정
        for (int i = 0; i < childCount; i++)
        {
            float angle = angleStep * i;
            path[i] = GetPositionOnCircle(angle);
        }

        // 각 UI 요소에 대해 애니메이션을 설정
        for (int i = 0; i < childCount; i++)
        {
            RectTransform child = container.GetChild(i) as RectTransform;

            // 애니메이션 루프
            DG.Tweening.Sequence sequence = DOTween.Sequence();
            sequence.Append(child.DOPath(path, duration, PathType.CatmullRom).SetEase(Ease.Linear)); // 경로를 따라 이동

            sequence.Play();
        }
        
    }
    
    // 원 위의 각도에 해당하는 위치를 계산하는 함수
    private Vector3 GetPositionOnCircle(float angle)
    {
        float angleInRadians = Mathf.Deg2Rad * angle;
        // container의 중심을 기준으로 하는 위치를 반환
        return new Vector3(Mathf.Cos(angleInRadians) * radius, Mathf.Sin(angleInRadians) * radius, 0);
    }

    private void DisableUI(PointerEventData pointerEventData)
    {
        UIManager.Instance.ClosePopupUI(this);
    }

    private void UseCommand(PointerEventData pointerEventData)
    {
        // 각 커맨드별 명령어 실시
    }
    
}
