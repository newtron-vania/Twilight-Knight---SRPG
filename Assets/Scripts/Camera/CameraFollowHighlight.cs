using Modules.EventSystem;
using UnityEngine;
using EventType = Modules.EventSystem.EventType;

public class CameraFollowHighlight : MonoBehaviour
{
    public Vector3 highlightPoint; // 하이라이트 포인트를 Transform으로 가져오기
    public float moveSpeed = 30f; // 카메라가 이동하는 속도

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null) Debug.LogError("Main camera not assigned and no camera with 'MainCamera' tag found.");

        highlightPoint = mainCamera.transform.position;
    }

    private void Update()
    {
        if (mainCamera == null || highlightPoint == null) return;

        // 카메라의 현재 위치
        var cameraPosition = mainCamera.transform.position;

        // 목표 위치 계산 (카메라의 z 위치는 유지하고 x, y 좌표만 이동)
        var targetPosition = new Vector3(highlightPoint.x, highlightPoint.y, cameraPosition.z);

        // 카메라를 하이라이트 포인트 방향으로 일정 속도로 이동
        mainCamera.transform.position = Vector3.Lerp(cameraPosition, targetPosition, moveSpeed * Time.deltaTime);
    }
}