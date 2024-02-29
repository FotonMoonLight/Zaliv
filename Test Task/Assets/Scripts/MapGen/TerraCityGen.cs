using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class TerraCityGen : MonoBehaviour
{
    [Header("Тайлы")]
    [Space(8)]
    [SerializeField] private Tilemap[] _tileLauches; // Cлои карты, на которых происходит генерация(1 слой - земля, 2 слой - объекты на карте
    [SerializeField] private Tile[] _tileGround; // Тайлы земли из которых строиться слой 1
    [SerializeField] private Tile[] _tileCity; // Тайлы городов для растановки( для городов можно использовать UniversalGenerator с растановкой на определенных тайлах
    [SerializeField] private Tile[] _tileForest;// Тайлы леса для разных местностей
    [SerializeField] private Tile[] _tileEvent;// Все случайные места на карте

    [Header("Параметры генерации")]
    [Space(8)]
    [SerializeField] private int[] _SetZonesForForest; // Номера тайлов на которых могут быть деревья
    [SerializeField] private float[] _mapScale; // Карта высот для спавна земли(Можно заполнить по своему усмотрению)
    /*_mapScale количевство элементов должно быть равно количевству тайлов из которых происходит генерация грунта
     * где первый или (нулевой) элемент отвечает за самое низкое значение карты(пример: Самое глубокое море)
     * а последний отвечает за самое высокое значение карты(пример:Самая высокая точка горы)
     * Формат задаваемых данных дробный в диапозоне от 0 до 1 (Значение 0.589234-премлемо для крайне тонокой настройки болшого кол-во тайло, иначе используйте формат 0.1,0.2 или 0.15,0.2,0.25)
     */
    [SerializeField] private Button _startGenerateButton;// Кнопка генерации
    [SerializeField] private Button[] _SetButtons; // Кнопки выбора размеров

    Model model = new Model(); // Технические переменные
    private float _perlin;
    private float _worldSeed;

    private void Start()
    {
        _startGenerateButton.onClick.AddListener(StartGenerateEnumerator);
        AddLisButt();
    }

    private void OnDestroy()
    {
        _startGenerateButton.onClick.RemoveAllListeners();
        RemoveAllListSetting();
    }

    private void AddLisButt()//Добавление прослушивания кнопок настройки карты (Заменить и добавить болше по необходимости)
    {
        _SetButtons[0].onClick.AddListener(model.SetSmallSetting);
        _SetButtons[1].onClick.AddListener(model.SetMediumSetting);
        _SetButtons[2].onClick.AddListener(model.SetBigSetting);
    }

    private void RemoveAllListSetting()
    {
        _SetButtons[0].onClick.RemoveAllListeners();
        _SetButtons[1].onClick.RemoveAllListeners();
        _SetButtons[2].onClick.RemoveAllListeners();
        
    }

    private IEnumerator GenerateCity()//Генерация городов с деревнями (По необходимости постановки разных тайлов городов, заменить на уневерсальный генератор(Перегрузка №2) и настроить зум=2)
    {
        //Данный генератор спавнит только города и деревни из 2х тайлов на один по каждому типу
        for (int i = 1; i < model.Weight - 1; i++)
        {
            for (int j = 0; j < model.Height - 1; j++)
            {
                _perlin = Mathf.PerlinNoise((i + _worldSeed) / 2f, (j + _worldSeed) / 2f);
                for (int k = model.MinSpawnValue; k < model.MaxSpawnValue; k++)
                {
                    if (_tileLauches[0].GetTile(new Vector3Int(i, j)) == (_tileGround[k]))
                        CitySetter(i, j);
                }
            }

        }

        yield return null;
    }

    private IEnumerator GenerateTerrain()
    {
        //Генератор грунта (Можно применять для генерации лесов и тп. Но требует отдельной настройки высот)
        for (int i = 0; i < model.Height; i++)
        {
            for (int j = 0; j < model.Weight; j++)
            {
                _perlin = Mathf.PerlinNoise((i + _worldSeed) / model.Zoom, (j + _worldSeed) / model.Zoom);
                MapControllerUD(i, j);
            }
        }

        yield return null;
    }
    
    private IEnumerator StartMapGenerate()
    {
        SetWorldSeed();
        CleanerMap();
        yield return StartCoroutine(GenerateTerrain()); 
        yield return StartCoroutine(_tileLauches[1].GenerateUniv(model.Weight, model.Height, 6f, MapGeneratorConstant.SetZero, _tileForest, _SetZonesForForest,MapGeneratorConstant.SetZero,_worldSeed,_tileLauches,_tileGround));//Генерация леса
        yield return StartCoroutine(_tileLauches[1].GenerateUniv(model.Weight, model.Height, 2f, MapGeneratorConstant.SetZero, _tileEvent, model.MinSpawnValue, model.MaxSpawnValue, 0.34f, MapGeneratorConstant.SetZero, _worldSeed, _tileLauches, _tileGround));//Генерация событий
        yield return StartCoroutine(GenerateCity());// Можно убрать и заменить на универсальный генератор если необходимо спавнить разные города на разных тайлах мира
        /*
          Применение GenerateUniv:
           tilemap.GenerateUniv(Указать необходимые данные в соответствиии с перегрузкой)
           tilemap - тот тайлмап на котором будет происходить генерация
           .GenerateUniv - Метод расширения для tilemap
         */
        StopAllCoroutines();
    }

    private void CitySetter(int x, int y) // Установка городов и деревень
    {
        if (_perlin > MapGeneratorConstant.PerlinDouble) //Постановка города из пула _tileCity
        {
            _tileLauches[1].SetTile(new Vector3Int(x, y), _tileCity[0]);
        }
        else if (_perlin < MapGeneratorConstant.PerlinDouble && _perlin > MapGeneratorConstant.PerlinDouble - 0.1f)//Постановка деревни
        {
            _tileLauches[1].SetTile(new Vector3Int(x, y), _tileCity[1]);
        }
    }

    private void MapControllerUD(int x, int y) //Установка тайлов земли в соответствии с шумом перлина
    {
        if (_perlin <= _mapScale[0])
            _tileLauches[0].SetTile(new Vector3Int(x, y), _tileGround[0]);//Установка минимального значения
        else if (_perlin >= _mapScale[_mapScale.Length - 1]) //Установка максимального значения 
            _tileLauches[0].SetTile(new Vector3Int(x, y), _tileGround[_tileGround.Length - 1]);
        else //Смежные тайлы
        {
            for (int k = 1; k < _tileGround.Length; k++)
            {
                if (_perlin >= _mapScale[k - 1] && _perlin <= _mapScale[k])
                {
                    _tileLauches[0].SetTile(new Vector3Int(x, y), _tileGround[k - 1]);
                    break;
                }
            }
        }
    }

    public void SetWorldSeed() =>
        _worldSeed = Random.Range(MapGeneratorConstant.MinInclusive, MapGeneratorConstant.MaxExclusive);

    public void CleanerMap()//Отчистка всех слоев карты
    {
        for (int i = 0; i < _tileLauches.Length; i++)
            _tileLauches[i].ClearAllTiles();
    }

    public float GetWorldSeed()
    {
        return _worldSeed;
    }

    private void StartGenerateEnumerator()
    {
        StartCoroutine(StartMapGenerate());
    }
    
}
