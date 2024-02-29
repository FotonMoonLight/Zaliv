using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float _zoomSensitivity = 1f;
    [SerializeField] private float _zoomSpeed = 10f;
    [SerializeField] private float _minZoom = 2f;
    [SerializeField] private float _maxZoom = 20f;

    private Camera cam;

    private void Awake() =>
        cam = GetComponent<Camera>();

    private void Update()
    {
        HandleMouseZoom();
        HandleTouchZoom();
    }

    private void HandleMouseZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            float newSize = cam.orthographicSize - scroll * _zoomSensitivity;
            cam.orthographicSize = Mathf.Clamp(newSize, _minZoom, _maxZoom);
        }
    }

    private void HandleTouchZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            float newSize = cam.orthographicSize + deltaMagnitudeDiff * _zoomSpeed * Time.deltaTime;
            cam.orthographicSize = Mathf.Clamp(newSize, _minZoom, _maxZoom);
        }
    }
}
