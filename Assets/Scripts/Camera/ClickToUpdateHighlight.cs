using UnityEngine;

public class ClickToUpdateHighlight : MonoBehaviour
{
    public CameraFollowHighlight cameraFollowHighlight; // 하이라이트 포인트를 Transform으로 가져오기
    private readonly float clickThresholdDistance = 0.1f; // 클릭 거리 임계값


    private Vector3 initialMousePosition;

    private bool isDragging;
    private Camera mainCamera;

    private void Start()
    {
        cameraFollowHighlight = GetComponent<CameraFollowHighlight>();
        mainCamera = Camera.main;

        if (mainCamera == null) Debug.LogError("Main camera not assigned and no camera with 'MainCamera' tag found.");
    }

    private void Update()
    {
        if (mainCamera == null || cameraFollowHighlight == null) return;


        if (Input.GetMouseButtonDown(0))
        {
            initialMousePosition = Input.mousePosition;
            isDragging = false;
        }

        if (Input.GetMouseButton(0))
        {
            var currentMousePosition = Input.mousePosition;
            var distanceDragged = Vector3.Distance(initialMousePosition, currentMousePosition);

            if (distanceDragged > clickThresholdDistance) isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            var finalMousePosition = Input.mousePosition;
            var distanceDragged = Vector3.Distance(initialMousePosition, finalMousePosition);

            if (!isDragging && distanceDragged <= clickThresholdDistance) UpdateHighlightPoint();

            isDragging = false;
        }
    }

    private void UpdateHighlightPoint()
    {
        var mouseScreenPosition = Input.mousePosition;
        var ray = mainCamera.ScreenPointToRay(mouseScreenPosition);
        var xyPlane = new Plane(Vector3.forward, Vector3.zero);

        if (xyPlane.Raycast(ray, out var distance))
        {
            var worldPoint = ray.GetPoint(distance);
            cameraFollowHighlight.highlightPoint =
                new Vector3(worldPoint.x, worldPoint.y, cameraFollowHighlight.highlightPoint.z);
        }
    }
}