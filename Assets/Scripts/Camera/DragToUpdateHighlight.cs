using Modules.EventSystem;
using UnityEngine;
using EventType = Modules.EventSystem.EventType;

public class DragToUpdateHighlight : MonoBehaviour, IEventListener
{
    public CameraFollowHighlight cameraFollowHighlight; // 하이라이트 포인트를 Transform으로 가져오기
    private bool _bDragging;
    private bool _bActive;
    private Vector3 _lastMousePosition;
    private Camera _mainCamera;

    private void Start()
    {
        cameraFollowHighlight = GetComponent<CameraFollowHighlight>();
        _mainCamera = Camera.main;

        if (_mainCamera == null)
        {
            Debug.LogError("Main camera not assigned and no camera with 'MainCamera' tag found.");
            return;
        }

        
        // InputManager에 함수 등록
        InputManager.Instance.KeyAction -= OnUpdate;
        InputManager.Instance.KeyAction += OnUpdate;
        
        // 이벤트 등록
        EventManager.Instance.AddListener(EventType.EVENT_HIGHLIGHT, this);
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
            _lastMousePosition = Input.mousePosition;
            _bDragging = false;
        }

        if (Input.GetMouseButton(0))
        {
            if (!_bDragging && Vector3.Distance(Input.mousePosition, _lastMousePosition) > 0.1f) _bDragging = true;

            if (_bDragging)
            {
                var currentMousePosition = Input.mousePosition;
                var mouseDelta = currentMousePosition - _lastMousePosition;

                var worldDelta =
                    _mainCamera.ScreenToWorldPoint(new Vector3(mouseDelta.x, mouseDelta.y, _mainCamera.nearClipPlane)) -
                    _mainCamera.ScreenToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane));

                cameraFollowHighlight.highlightPoint -= new Vector3(worldDelta.x, worldDelta.y, 0f);
                _lastMousePosition = currentMousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0)) _bDragging = false;
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