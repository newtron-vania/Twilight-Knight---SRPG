using System.Collections.Generic;

public interface IPathFinding
{
    List<Tile> FindPath(Tile startTile, Tile targetTile, TileMap tileMap, List<Tile> accessibleTiles = null);
    List<Tile> GetReachableTiles(Tile startTile, int moveRange, TileMap tileMap);

    List<Tile> GetAttackableTiles(Tile startTile, int Range, TileMap tileMap);
}