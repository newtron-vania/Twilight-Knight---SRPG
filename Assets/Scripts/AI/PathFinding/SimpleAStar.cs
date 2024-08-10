using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAStar : IPathFinding
{ 
    public List<Tile> FindPath(Tile startTile, Tile targetTile, TileMap tileMap, List<Tile> accessibleTiles = null)
    {
        List<Tile> openSet = new List<Tile> { startTile };
        HashSet<Tile> closedSet = new HashSet<Tile>();
        Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
        Dictionary<Tile, float> gCost = new Dictionary<Tile, float> { { startTile, 0 } };
        Dictionary<Tile, float> fCost = new Dictionary<Tile, float> { { startTile, GetHeuristic(startTile, targetTile) } };

        while (openSet.Count > 0)
        {
            Tile current = GetTileWithLowestFCost(openSet, fCost);
            if (current == targetTile)
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var neighbor in GetNeighbors(current, tileMap))
            {
                if (closedSet.Contains(neighbor) || (accessibleTiles != null && !accessibleTiles.Contains(neighbor)))
                {
                    continue;
                }
                
                if (!IsTileWalkable(neighbor, tileMap))
                {
                    continue;
                }

                float tentativeGCost = gCost[current] + GetDistance(current, neighbor);

                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeGCost >= gCost[neighbor])
                {
                    continue;
                }

                cameFrom[neighbor] = current;
                gCost[neighbor] = tentativeGCost;
                fCost[neighbor] = gCost[neighbor] + GetHeuristic(neighbor, targetTile);
            }

            Debug.Log($"Openset Count : {openSet.Count}");
            foreach (var VARIABLE in openSet)
            {
                Debug.Log($"Tile in Openset: ({VARIABLE.x}, {VARIABLE.y}, {VARIABLE.z})");
            }
        }

        return new List<Tile>();
    }

    public List<Tile> GetReachableTiles(Tile startTile, int moveRange, TileMap tileMap)
    {
        List<Tile> reachableTiles = new List<Tile>();
        Queue<Tile> queue = new Queue<Tile>();
        HashSet<Tile> visited = new HashSet<Tile>();

        queue.Enqueue(startTile);
        visited.Add(startTile);

        while (queue.Count > 0)
        {
            Tile current = queue.Dequeue();
            reachableTiles.Add(current);

            foreach (var neighbor in GetNeighbors(current, tileMap))
            {
                if (!visited.Contains(neighbor) && IsTileWalkable(neighbor, tileMap) && GetDistance(startTile, neighbor) <= moveRange)
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
        }

        return reachableTiles;
    }

    public List<Tile> GetAttackableTiles(Tile startTile, int Range, TileMap tileMap)
    {
        List<Tile> reachableTiles = new List<Tile>();
        Queue<Tile> queue = new Queue<Tile>();
        HashSet<Tile> visited = new HashSet<Tile>();

        queue.Enqueue(startTile);
        visited.Add(startTile);

        while (queue.Count > 0)
        {
            Tile current = queue.Dequeue();
            reachableTiles.Add(current);

            foreach (var neighbor in GetNeighbors(current, tileMap))
            {
                if (!visited.Contains(neighbor) && GetDistance(startTile, neighbor) <= Range)
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
        }

        return reachableTiles;
    }
    private Tile GetTileWithLowestFCost(List<Tile> openSet, Dictionary<Tile, float> fCost)
    {
        Tile lowest = openSet[0];
        foreach (var tile in openSet)
        {
            if (fCost[tile] < fCost[lowest])
            {
                lowest = tile;
            }
        }
        return lowest;
    }

    private List<Tile> ReconstructPath(Dictionary<Tile, Tile> cameFrom, Tile current)
    {
        List<Tile> path = new List<Tile> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        
        return path;
    }

    private List<Tile> GetNeighbors(Tile tile, TileMap tileMap)
    {
        List<Tile> neighbors = new List<Tile>();

        int[,] directions = new int[,]
        {
            { 1, 0, 0 }, { -1, 0, 0 },
            { 0, 1, 0 }, { 0, -1, 0 }
        };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int newX = tile.x + directions[i, 0];
            int newY = tile.y + directions[i, 1];
            int newZ = tile.z + directions[i, 2];

            Tile newTile = tileMap.GetTile(newX, newY, newZ);
            if (newTile != null)
            {
                neighbors.Add(newTile);
            }
        }
        return neighbors;
    }

    private bool IsTileWalkable(Tile tile, TileMap tileMap)
    {
        return tile.tileObject == null; // 예: 타일에 오브젝트가 없으면 이동 가능
    }

    private float GetDistance(Tile a, Tile b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z);
    }

    private float GetHeuristic(Tile a, Tile b)
    {
        return GetDistance(a, b);
    }
}
