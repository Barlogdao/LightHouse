using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapProvider : SingletonPersistent<MapProvider>
{
    [SerializeField] private LayerMask _unitMask;
    [SerializeField] private Tilemap _ground, _water;
    private List<Vector2> _groundTiles, _waterTiles;
    public List<Vector2> FreeTiles { get; private set; }


    protected override void OnAwake()
    {
        _groundTiles = CalcArea(_ground);
        _waterTiles = CalcArea(_water);
        FreeTiles = _groundTiles.Except(_waterTiles).ToList();
    }


    private List<Vector2> CalcArea(Tilemap tm)
    {
        List<Vector2> list = new();
        foreach (var pos in tm.cellBounds.allPositionsWithin)
        {
            Vector3Int gridPlace = new(pos.x, pos.y, pos.z);
            if (tm.HasTile(gridPlace))
            {
                Vector3 worldPlace = _ground.GetCellCenterWorld(gridPlace);
                list.Add(worldPlace);
            }
        }
        return list;
    }

    public List<Vector2> GetFreePositions()
    {
        return FreeTiles.Where(x => (Physics2D.OverlapPoint(x, _unitMask) == null)).ToList();
    }

}
