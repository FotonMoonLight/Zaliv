using UnityEngine;

public class SetTerrainPosition : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _terrain;

    public void SetOriginPos()
    {
        _camera.transform.position = new Vector3(0,0,-10);
        _terrain.transform.position = new Vector3(0, 0, 0);//Обратите внимание что Z координата должна быть равна координате шагателя и устанавливаеммому значению в MoveOnTilemap
    }

    public void SetStuffPos()
    {
        _terrain.transform.position = new Vector3(500,500,10);
    }
}