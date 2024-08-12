using Modules.EventSystem;
using UnityEngine;
using EventType = Modules.EventSystem.EventType;

public class ClickToUpdateHighlight : MonoBehaviour, IEventListener
{
    public CameraFollowHighlight cameraFollowHighlight; // 하이라이트 포인트를 Transform으로 가져오기
    private const float clickThresholdDistance = 0.1f; // 클릭 거리 임계값


    private Vector3 _initialMousePosition;

    private bool _bDragging;
    private bool _bActive;
    private Camera _mainCamera;

    private void Start()
    {
        cameraFollowHighlight = GetComponent<CameraFollowHighlight>();
        _mainCamera = Camera.main;

        if (_mainCamera == null) Debug.LogError("Main camera not assigned and no camera with 'MainCamera' tag found.");
        
        InputManager.Instance.KeyAction -= OnUpdate;
        InputManager.Instance.KeyAction += OnUpdate;
    }
    
    private void OnDestroy()
    {
        // InputManager에서 함수 제거
        InputManager.Instance.KeyAction -= OnUpdate;
    }

    private void OnUpdate()
    {
        if (_mainCamera == null || cameraFollowHighlight == null) return;

        if (!_bActive) return;

        if (Input.GetMouseButtonDown(0))
        {
            _initialMousePosition = Input.mousePosition;
            _bDragging = false;
        }

        if (Input.GetMouseButton(0))
        {
            var currentMousePosition = Input.mousePosition;
            var distanceDragged = Vector3.Distance(_initialMousePosition, currentMousePosition);

            if (distanceDragged > clickThresholdDistance) _bDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            var finalMousePosition = Input.mousePosition;
            var distanceDragged = Vector3.Distance(_initialMousePosition, finalMousePosition);

            if (!_bDragging && distanceDragged <= clickThresholdDistance) UpdateHighlightPoint();

            _bDragging = false;
        }
    }

    private void UpdateHighlightPoint()
    {
        var mouseScreenPosition = Input.mousePosition;
        var ray = _mainCamera.ScreenPointToRay(mouseScreenPosition);
        var xyPlane = new Plane(Vector3.forward, Vector3.zero);

        if (xyPlane.Raycast(ray, out var distance))
        {
            var worldPoint = ray.GetPoint(distance);
            cameraFollowHighlight.highlightPoint =
                new Vector3(worldPoint.x, worldPoint.y, cameraFollowHighlight.highlightPoint.z);
        }
    }

    public void OnEvent(EventType eventType, Component sender, object parameter = null)
    {
        if (parameter == null)
        {
            _bActive = true;
            return;
        }

        _bActive = false;
    }
}