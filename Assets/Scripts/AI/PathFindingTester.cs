using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingTester : MonoBehaviour
{
    void Start()
    {
       
        TileMap tileMap = GameManager.Instance.getTileMap();

        // 캐릭터 배치
        Tile start = tileMap.GetTile(0, 0, 0);
        Tile goal = tileMap.GetTile(5, 5, 0);
        Character character = GameManager.Instance.AddCharacter(true, null);

        // 장애물 설치
        GameObject obstacle = new GameObject("Obstacle");
        tileMap.SetTileObject(3, 3, 0, obstacle);
        tileMap.SetTileObject(3, 4, 0, obstacle);
        tileMap.SetTileObject(3, 5, 0, obstacle);

        // BFS를 통해 이동 가능한 타일을 미리 계산
        IPathFinding pathFinding = new SimpleAStar();
        int moveRange = 10; // 캐릭터의 이동 범위
        List<Tile> reachableTiles = pathFinding.GetReachableTiles(start, moveRange, GameManager.Instance.getTileMap());

        if (reachableTiles.Count > 0)
        {
            Debug.Log("Reachable tiles found:");
            foreach (Tile tile in reachableTiles)
            {
                Debug.Log($"Tile: {tile.x}, {tile.y}, {tile.z}");
            }
        }
        else
        {
            Debug.Log("No reachable tiles found");
        }

        // A* 알고리즘을 통해 경로 찾기
        List<Tile> path = pathFinding.FindPath(start, goal, GameManager.Instance.getTileMap());

        if (path.Count > 0)
        {
            Debug.Log("Path found:");
            foreach (Tile tile in path)
            {
                Debug.Log($"Tile: {tile.x}, {tile.y}, {tile.z}");
            }
        }
        else
        {
            Debug.Log("Path not found");
        }
    }}
