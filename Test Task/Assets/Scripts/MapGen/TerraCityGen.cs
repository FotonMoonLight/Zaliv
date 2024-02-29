using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class TerraCityGen : MonoBehaviour
{
    [Header("�����")]
    [Space(8)]
    [SerializeField] private Tilemap[] _tileLauches; // C��� �����, �� ������� ���������� ���������(1 ���� - �����, 2 ���� - ������� �� �����
    [SerializeField] private Tile[] _tileGround; // ����� ����� �� ������� ��������� ���� 1
    [SerializeField] private Tile[] _tileCity; // ����� ������� ��� ����������( ��� ������� ����� ������������ UniversalGenerator � ����������� �� ������������ ������
    [SerializeField] private Tile[] _tileForest;// ����� ���� ��� ������ ����������
    [SerializeField] private Tile[] _tileEvent;// ��� ��������� ����� �� �����

    [Header("��������� ���������")]
    [Space(8)]
    [SerializeField] private int[] _SetZonesForForest; // ������ ������ �� ������� ����� ���� �������
    [SerializeField] private float[] _mapScale; // ����� ����� ��� ������ �����(����� ��������� �� ������ ����������)
    /*_mapScale ����������� ��������� ������ ���� ����� ����������� ������ �� ������� ���������� ��������� ������
     * ��� ������ ��� (�������) ������� �������� �� ����� ������ �������� �����(������: ����� �������� ����)
     * � ��������� �������� �� ����� ������� �������� �����(������:����� ������� ����� ����)
     * ������ ���������� ������ ������� � ��������� �� 0 �� 1 (�������� 0.589234-�������� ��� ������ ������� ��������� ������� ���-�� �����, ����� ����������� ������ 0.1,0.2 ��� 0.15,0.2,0.25)
     */
    [SerializeField] private Button _startGenerateButton;// ������ ���������
    [SerializeField] private Button[] _SetButtons; // ������ ������ ��������

    Model model = new Model(); // ����������� ����������
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

    private void AddLisButt()//���������� ������������� ������ ��������� ����� (�������� � �������� ����� �� �������������)
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

    private IEnumerator GenerateCity()//��������� ������� � ��������� (�� ������������� ���������� ������ ������ �������, �������� �� ������������� ���������(���������� �2) � ��������� ���=2)
    {
        //������ ��������� ������� ������ ������ � ������� �� 2� ������ �� ���� �� ������� ����
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
        //��������� ������ (����� ��������� ��� ��������� ����� � ��. �� ������� ��������� ��������� �����)
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
        yield return StartCoroutine(_tileLauches[1].GenerateUniv(model.Weight, model.Height, 6f, MapGeneratorConstant.SetZero, _tileForest, _SetZonesForForest,MapGeneratorConstant.SetZero,_worldSeed,_tileLauches,_tileGround));//��������� ����
        yield return StartCoroutine(_tileLauches[1].GenerateUniv(model.Weight, model.Height, 2f, MapGeneratorConstant.SetZero, _tileEvent, model.MinSpawnValue, model.MaxSpawnValue, 0.34f, MapGeneratorConstant.SetZero, _worldSeed, _tileLauches, _tileGround));//��������� �������
        yield return StartCoroutine(GenerateCity());// ����� ������ � �������� �� ������������� ��������� ���� ���������� �������� ������ ������ �� ������ ������ ����
        /*
          ���������� GenerateUniv:
           tilemap.GenerateUniv(������� ����������� ������ � ������������� � �����������)
           tilemap - ��� ������� �� ������� ����� ����������� ���������
           .GenerateUniv - ����� ���������� ��� tilemap
         */
        StopAllCoroutines();
    }

    private void CitySetter(int x, int y) // ��������� ������� � ��������
    {
        if (_perlin > MapGeneratorConstant.PerlinDouble) //���������� ������ �� ���� _tileCity
        {
            _tileLauches[1].SetTile(new Vector3Int(x, y), _tileCity[0]);
        }
        else if (_perlin < MapGeneratorConstant.PerlinDouble && _perlin > MapGeneratorConstant.PerlinDouble - 0.1f)//���������� �������
        {
            _tileLauches[1].SetTile(new Vector3Int(x, y), _tileCity[1]);
        }
    }

    private void MapControllerUD(int x, int y) //��������� ������ ����� � ������������ � ����� �������
    {
        if (_perlin <= _mapScale[0])
            _tileLauches[0].SetTile(new Vector3Int(x, y), _tileGround[0]);//��������� ������������ ��������
        else if (_perlin >= _mapScale[_mapScale.Length - 1]) //��������� ������������� �������� 
            _tileLauches[0].SetTile(new Vector3Int(x, y), _tileGround[_tileGround.Length - 1]);
        else //������� �����
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

    public void CleanerMap()//�������� ���� ����� �����
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
