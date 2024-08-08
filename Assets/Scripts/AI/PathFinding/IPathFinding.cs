using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathFinding
{
    List<Tile> FindPath(Tile startTile, Tile targetTile, TileMap tileMap, List<Tile> accessibleTiles = null);
    List<Tile> GetReachableTiles(Tile startTile, int moveRange, TileMap tileMap);
}
