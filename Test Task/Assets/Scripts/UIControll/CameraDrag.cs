using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    [SerializeField] private float _dragSensitivity = 0.5f;

    private Vector3 _dragOrigin;
    private bool _isDragging;

    private Camera _camera;

    private void Start() =>
        _camera = Camera.main;   

    private void LateUpdate()
    {
        HandleMouseInput();
        HandleTouchInput();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrag();
        }

        if (Input.GetMouseButton(0) && _isDragging)
        {
            PerformDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = touch.deltaPosition;
                MoveCamera(-touchDeltaPosition.x * _dragSensitivity, -touchDeltaPosition.y * _dragSensitivity);
            }
        }
    }

    private void StartDrag()
    {
        _dragOrigin = _camera.ScreenToWorldPoint(Input.mousePosition);
        _isDragging = true;
    }

    private void PerformDrag()
    {
        Vector3 difference = _dragOrigin - _camera.ScreenToWorldPoint(Input.mousePosition);
        MoveCamera(difference.x, difference.y);
    }

    private void EndDrag()
    {
        _isDragging = false;
    }

    private void MoveCamera(float x, float y)
    {
        Vector3 newPosition = _camera.transform.position + new Vector3(x, y, 0);
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, newPosition, Time.deltaTime * _dragSensitivity);
    }
}