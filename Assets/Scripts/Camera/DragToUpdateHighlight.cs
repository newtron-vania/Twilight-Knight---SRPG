using UnityEngine;

public class DragToUpdateHighlight : MonoBehaviour
{
    public CameraFollowHighlight cameraFollowHighlight; // 하이라이트 포인트를 Transform으로 가져오기
    private Camera mainCamera;
    private Vector3 lastMousePosition;
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
            lastMousePosition = Input.mousePosition;
            isDragging = false;
        }

        if (Input.GetMouseButton(0))
        {
            if (!isDragging && Vector3.Distance(Input.mousePosition, lastMousePosition) > 0.1f)
            {
                isDragging = true;
            }

            if (isDragging)
            {
                Vector3 currentMousePosition = Input.mousePosition;
                Vector3 mouseDelta = currentMousePosition - lastMousePosition;

                Vector3 worldDelta = mainCamera.ScreenToWorldPoint(new Vector3(mouseDelta.x, mouseDelta.y, mainCamera.nearClipPlane)) -
                                     mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));

                cameraFollowHighlight.highlightPoint -= new Vector3(worldDelta.x, worldDelta.y, 0f);
                lastMousePosition = currentMousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}