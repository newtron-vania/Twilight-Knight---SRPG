using UnityEngine;

public class ClickToUpdateHighlight : MonoBehaviour
{
    public CameraFollowHighlight cameraFollowHighlight; // 하이라이트 포인트를 Transform으로 가져오기
    private Camera mainCamera;
    
    
    private Vector3 initialMousePosition;
    private float clickThresholdDistance = 0.1f; // 클릭 거리 임계값

    private bool isDragging = false;

    void Start()
    {
        cameraFollowHighlight = GetComponent<CameraFollowHighlight>();
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not assigned and no camera with 'MainCamera' tag found.");
        }
    }

    void Update()
    {
        if (mainCamera == null || cameraFollowHighlight == null) return;

        
        if (Input.GetMouseButtonDown(0))
        {
            initialMousePosition = Input.mousePosition;
            isDragging = false;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePosition = Input.mousePosition;
            float distanceDragged = Vector3.Distance(initialMousePosition, currentMousePosition);

            if (distanceDragged > clickThresholdDistance)
            {
                isDragging = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 finalMousePosition = Input.mousePosition;
            float distanceDragged = Vector3.Distance(initialMousePosition, finalMousePosition);

            if (!isDragging && distanceDragged <= clickThresholdDistance)
            {
                UpdateHighlightPoint();
            }

            isDragging = false;
        }
    }

    void UpdateHighlightPoint()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);
        Plane xyPlane = new Plane(Vector3.forward, Vector3.zero);

        if (xyPlane.Raycast(ray, out float distance))
        {
            Vector3 worldPoint = ray.GetPoint(distance);
            cameraFollowHighlight.highlightPoint= new Vector3(worldPoint.x, worldPoint.y, cameraFollowHighlight.highlightPoint.z);
        }
    }
}