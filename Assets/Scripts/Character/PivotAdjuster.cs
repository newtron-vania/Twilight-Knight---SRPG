using UnityEngine;

public class PivotAdjuster : MonoBehaviour
{
    public Transform target;
    public Vector3 pivotOffset;

    private void Start()
    {
        if (target == null) return;

        // 빈 게임 오브젝트를 생성하고 위치를 조정하여 피벗을 설정합니다.
        var pivotObject = new GameObject("Pivot");
        pivotObject.transform.position = target.position + pivotOffset;
        target.parent = pivotObject.transform;
        target.localPosition = Vector3.zero;
    }
}