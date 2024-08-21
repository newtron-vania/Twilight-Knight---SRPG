using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileManager
{
    public TileMap Tilemap { get; private set; }
    private List<GameObject> _selectTileList = new List<GameObject>();

    private string[] _selectColorPath =
        { "IsomtricSelectMaterial", "IsomtricAttackMaterial", "IsometricInstancingMaterial" };

    // 해당 맵의 타일맵을 불러와 생성한다.
    // 현재 다음 타일맵이 존재하지 않기에 대기
    public bool CreateTileMap()
    {
        return true;
    }

    public void DestorySelectTile()
    {
        foreach (var VARIABLE in _selectTileList)
        {
            ResourceManager.Instance.Destroy(VARIABLE);
        }

        _selectTileList.Clear();
    }

    public List<GameObject> SetSelectTileList(List<Tile> tiles, Define.SelectTileColor color = Define.SelectTileColor.Blue)
    {
        if (tiles == null || !(tiles.Count > 0))
        {
            return null;
        }
        
        List<GameObject> goes = new List<GameObject>();
        foreach (var VARIABLE in tiles)
        {
            GameObject go = SetSelectTile(VARIABLE, "SelectTile");
            goes.Add(go);
            go.GetComponentInChildren<SpriteRenderer>().material =
                ResourceManager.Instance.Load<Material>($"Materials{_selectColorPath[(int)color]}");
        }

        return goes;
    }
    
    
    private GameObject SetSelectTile(Tile originTile, string path)
    {
        Vector3 position = Tilemap.GetWorldPositionByTile(originTile.x, originTile.y, originTile.z);

        GameObject tile = null;
        RaycastHit raycastHit;
        int layerMask = (1 << LayerMask.NameToLayer("Tile"));
        if (Physics.Raycast(position, Vector3.down, out raycastHit, 1f, layerMask))
        {
            Vector3 hitPoint = raycastHit.point;
            tile = ResourceManager.Instance.Instantiate($"Tiles/{path}", hitPoint);
        }

        return tile;
    }
    
    // 타일 생성
    public GameObject CreateTile(int x, int y, int z, string path)
    {
        GameObject tile = ResourceManager.Instance.Instantiate($"Tiles/{path}");
        tile.transform.position = Tilemap.GetWorldPositionByTile(x, y, z);

        _selectTileList.Add(tile);
        return tile;
    }

    public void SetTileMap(int x, int y, int z)
    {
        Tilemap = new TileMap(x, y, z);
    }
    
}
