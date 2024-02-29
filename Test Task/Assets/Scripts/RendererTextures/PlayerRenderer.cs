using UnityEngine;
using UnityEngine.UI;

public class PlayerRenderer : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage;

    private Camera _renderCamera;

    private void Start()
    {
        _renderCamera = GetComponent<Camera>();
        RenderTexture playerTexture = new RenderTexture(1920,1080,1);
        _renderCamera.targetTexture = playerTexture;
        _rawImage.texture = _renderCamera.targetTexture;
    }
}
