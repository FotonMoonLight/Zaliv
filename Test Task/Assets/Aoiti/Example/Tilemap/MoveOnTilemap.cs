using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Aoiti.Pathfinding;
using UnityEngine.UI;


public class MoveOnTilemap : MonoBehaviour
{
    Vector3Int[] directions=new Vector3Int[4] {Vector3Int.left,Vector3Int.right,Vector3Int.up,Vector3Int.down }; //���� ����������� ��� ������� �������
    /*����� ����������� ��������� ��� ��������� ����� ��� ����������� ������ �� ������ Y-���������
     * ��� ��������� ������ ������� ��� ������ ����
     */

    public Tilemap tilemap;// ������� �� �������� ������ ������ �����
    public TileAndMovementCost[] tiles;//���� ������ ������� ������������ ��� ������������
    public Button button;//������ ����������� ����������� ������� ���� ��� ��� ������ �������
    Pathfinder<Vector3Int> pathfinder;// ���� �������� �� ������� ������� �������� ��� ���������� ����
    private bool _IsTouch = false;// ���������� ����������� ������

    [System.Serializable]
    public struct TileAndMovementCost // ��������� ������ � ������� ���������� ������� ������ ������
    {
        public Tile tile;
        public bool movable;
        public float movementCost;
    }

    public List<Vector3Int> path;
    [Range(0.001f,1f)]
    public float stepTime;// ����� ������������� �� ���� ���


    public float DistanceFunc(Vector3Int a, Vector3Int b) // ������ ��������� ��� ��������
    {
        return (a-b).sqrMagnitude;
    }


    public Dictionary<Vector3Int,float> connectionsAndCosts(Vector3Int a) // ������� ������� ��� ������ ������� ���� (�� �������� ��� �������������)
    {
        Dictionary<Vector3Int, float> result= new Dictionary<Vector3Int, float>();
        foreach (Vector3Int dir in directions)
        {
            foreach (TileAndMovementCost tmc in tiles)
            {
                if (tilemap.GetTile(a+dir)==tmc.tile)
                {
                    if (tmc.movable) result.Add(a + dir, tmc.movementCost);

                }
            }
                
        }
        return result;
    }

    void Start()
    {
        button.onClick.AddListener(MayMove);
        pathfinder = new Pathfinder<Vector3Int>(DistanceFunc, connectionsAndCosts);
    }

    void Update()
    {

        MouseMove();//�������� �� ��������� ������ ����
        TouchMove();//�������� �� ��������� ����� � ��������
    }
    private void TouchMove()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && _IsTouch==true)
            {
                var currentCellPos = tilemap.WorldToCell(transform.position);
                var target = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(touch.position));
                target.z = 0;//����� ����� �� ������� ���������� ����������� ��������� ������ ���� �� ����� ������ Z � ���; � ������� ������ �������� ��� ��������
                pathfinder.GenerateAstarPath(currentCellPos, target, out path);
                StopAllCoroutines();
                StartCoroutine(Move());
                _IsTouch = false;
            }
        }
    }
    private void MouseMove()
    {
        if (Input.GetMouseButtonDown(1) && _IsTouch == true)
        {
            var currentCellPos = tilemap.WorldToCell(transform.position);
            var target = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            target.z = 0;//����� ����� �� ������� ���������� ����������� ��������� ������ ���� �� ����� ������ Z � ���; � ������� ������ �������� ��� ��������
            pathfinder.GenerateAstarPath(currentCellPos, target, out path);
            StopAllCoroutines();
            StartCoroutine(Move());
            _IsTouch = false;
        }
    }
    private void MayMove()
    {
        _IsTouch = true;
    }
    IEnumerator Move()// ���� �����
    {
        while (path.Count > 0)
        {
            transform.position = tilemap.CellToWorld(path[0]);
            path.RemoveAt(0);
            yield return new WaitForSeconds(stepTime);
            
        }
        

    }



    
}

