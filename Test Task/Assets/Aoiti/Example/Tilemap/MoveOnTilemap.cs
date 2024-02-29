using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Aoiti.Pathfinding;
using UnityEngine.UI;


public class MoveOnTilemap : MonoBehaviour
{
    Vector3Int[] directions=new Vector3Int[4] {Vector3Int.left,Vector3Int.right,Vector3Int.up,Vector3Int.down }; //Углы направления для команды игроков
    /*Можно попробовать расширить пул возможных шагов для исправления ошибки на стыках Y-координат
     * Или поправить Расчет соседей для выбора шага
     */

    public Tilemap tilemap;// Тайлмап по которому должен ходить игрок
    public TileAndMovementCost[] tiles;//Пулл тайлов которые учитываються при передвижении
    public Button button;//Кнопка разрешающая возможность сделать один шаг для партии игроков
    Pathfinder<Vector3Int> pathfinder;// Пулл векторов по которым пройдет персонаж для достижения цели
    private bool _IsTouch = false;// Переменная возможности ходить

    [System.Serializable]
    public struct TileAndMovementCost // Структура ячейки в которую необходимо занести данные тайлов
    {
        public Tile tile;
        public bool movable;
        public float movementCost;
    }

    public List<Vector3Int> path;
    [Range(0.001f,1f)]
    public float stepTime;// Время затрачиваемое на один шаг


    public float DistanceFunc(Vector3Int a, Vector3Int b) // Расчет Дистанции для векторов
    {
        return (a-b).sqrMagnitude;
    }


    public Dictionary<Vector3Int,float> connectionsAndCosts(Vector3Int a) // Словарь соседей для выбора лучшего шага (Не изменять без необходимости)
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

        MouseMove();//Отвечает за обработку кликов мыши
        TouchMove();//Отвечает за обработку тачей с телефона
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
                target.z = 0;//ВАЖНО Карта по которой необходимо передвигать персонажа должна быть на одном уровне Z с ним; В крайнем случае заменить это значение
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
            target.z = 0;//ВАЖНО Карта по которой необходимо передвигать персонажа должна быть на одном уровне Z с ним; В крайнем случае заменить это значение
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
    IEnumerator Move()// Цикл шагов
    {
        while (path.Count > 0)
        {
            transform.position = tilemap.CellToWorld(path[0]);
            path.RemoveAt(0);
            yield return new WaitForSeconds(stepTime);
            
        }
        

    }



    
}

