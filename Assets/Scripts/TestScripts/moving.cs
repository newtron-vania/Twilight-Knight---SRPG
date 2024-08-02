using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving : MonoBehaviour
{
    public float speed = 1.0f; // 이동 속도
    [SerializeField]
    private Vector3 targetPosition; // 목표 위치
    private bool isMoving = false; // 이동 중인지 여부 확인

    void Update()
    {
        // 마우스 클릭 시 처리
        if (Input.GetMouseButtonDown(0))
        {
            // 카메라에서 마우스 위치로 레이캐스팅
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 레이캐스팅이 "Tile" 레이어의 콜라이더와 충돌할 경우
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Tile")))
            {
                // 목표 위치 설정 (충돌 지점 + 0.5)
                Debug.Log($"raycasting target name : {hit.collider.name}");
                targetPosition = hit.collider.transform.position + new Vector3(0f, 0.5f, 0f);
                isMoving = true;
            }
        }

        // 이동 로직
        if (isMoving)
        {
            // 현재 위치에서 목표 위치까지 1의 속도로 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // 목표 위치에 도달하면 이동 중지
            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                isMoving = false;
            }
        }
    }
}
