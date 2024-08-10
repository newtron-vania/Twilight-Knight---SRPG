using System.Collections.Generic;
using UnityEngine;

public class TileMap
{
    private Tile[,,] tiles; // 3차원 타일 배열

    public TileMap(int width, int height, int depth)
    {
        Width = width;
        Height = height;
        Depth = depth;
        WorldPositionOfZero = Vector3.zero;
        Padding = 1f;
        InitializeDefaultTileMap();
    }

    public int Width { get; }
    public int Height { get; }
    public int Depth { get; }

    public Vector3 WorldPositionOfZero { get; set; }
    public float Padding { get; set; }

    private void InitializeDefaultTileMap()
    {
        tiles = new Tile[Width, Height, Depth];
        // 타일 초기화
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        for (var z = 0; z < Depth; z++)
            tiles[x, y, z] = new Tile(x, y, z);
    }

    public void SetTileMapData(List<Tile> tileList)
    {
        tiles = null;
        foreach (var tile in tileList) tiles[tile.x, tile.y, tile.z] = tile;
    }

    // 타일에 오브젝트 설정
    public void SetTileObject(int x, int y, int z, GameObject tileObject)
    {
        if (IsPositionValid(x, y, z)) tiles[x, y, z].tileObject = tileObject;
    }

    public void SetTileObject(Tile tile, GameObject tileObject)
    {
        if (IsPositionValid(tile.x, tile.y, tile.z)) tiles[tile.x, tile.y, tile.z].tileObject = tileObject;
    }

    // 타일 데이터 반환
    public Tile GetTile(int x, int y, int z)
    {
        return IsPositionValid(x, y, z) ? tiles[x, y, z] : null;
    }

    public Vector3 GetWorldPositionByTile(int x, int y, int z)
    {
        return WorldPositionOfZero + new Vector3(y * Padding, z * Padding, x * Padding);
    }

    public Vector3 GetWorldPositionByTile(Tile tile)
    {
        return WorldPositionOfZero + new Vector3(tile.y * Padding, tile.z * Padding, tile.x * Padding);
    }

    public Tile GetTileByWorldPosition(Vector3 worldPosition)
    {
        var x = Mathf.CeilToInt((worldPosition.z - WorldPositionOfZero.z) / Padding);
        var y = Mathf.CeilToInt((worldPosition.x - WorldPositionOfZero.x) / Padding);
        var z = Mathf.CeilToInt((worldPosition.y - WorldPositionOfZero.y) / Padding);

        Debug.LogWarning($" x y z : {x} {y} {z}");
        return GetTile(x, y, z);
    }

    private bool IsPositionValid(int x, int y, int z)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height && z >= 0 && z < Depth;
    }
}