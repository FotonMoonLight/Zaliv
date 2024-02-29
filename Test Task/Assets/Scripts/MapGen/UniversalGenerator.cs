using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class UniversalGenerator
{
    //Метод расширения с перегрузкой для tilemap
    public static IEnumerator GenerateUniv(this Tilemap tilemap,int x, int y, float zoom, int layer, Tile[] tiles, int[] SetZone, float _perlin,float _worldSeed, Tilemap[] _tileLauches, Tile[] _tileGround)
    {
        for (int i = 1; i < x; i++)
        {
            for (int j = 1; j < y; j++)
            {
                _perlin = Mathf.PerlinNoise((i + _worldSeed) / zoom, (j + _worldSeed) / zoom);
                for (int k = 0; k < tiles.Length; k++)
                {
                    if (_tileLauches[layer].GetTile(new Vector3Int(i, j)) == (_tileGround[SetZone[k]]) && _perlin > 0.5)
                        tilemap.SetTile(new Vector3Int(i, j), tiles[k]);
                }
            }
        }
        yield return null;
    }
    public static IEnumerator GenerateUniv(this Tilemap tilemap,int x, int y, float zoom, int layer, Tile[] tiles, int _min, int _max, float _Sch, float _perlin, float _worldSeed, Tilemap[] _tileLauches, Tile[] _tileGround)
    {
        int setter = 0;
        for (int i = 1, j = 1; i < x; i++, j++)
        {
            _perlin = Mathf.PerlinNoise((i + _worldSeed) / zoom, (j + _worldSeed) / zoom);
            setter = Random.Range(0, tiles.Length);
            for (int k = _min; k < _max; k++)
            {
                if (_tileLauches[layer].GetTile(new Vector3Int(i, j)) == (_tileGround[k]) && _perlin < _Sch)
                    tilemap.SetTile(new Vector3Int(i, j), tiles[setter]);
            }
        }
        yield return null;
    }
}
