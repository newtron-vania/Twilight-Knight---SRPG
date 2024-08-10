using UnityEngine;

public class PathFindingTester : MonoBehaviour
{
    private void Start()
    {
        var tileMap = GameManager.Instance.getTileMap();

        // 캐릭터 배치
        var start = tileMap.GetTile(0, 0, 0);
        var goal = tileMap.GetTile(5, 5, 0);
        var character = GameManager.Instance.AddCharacter(true);

        // 장애물 설치
        var obstacle = new GameObject("Obstacle");
        tileMap.SetTileObject(3, 3, 0, obstacle);
        tileMap.SetTileObject(3, 4, 0, obstacle);
        tileMap.SetTileObject(3, 5, 0, obstacle);

        // BFS를 통해 이동 가능한 타일을 미리 계산
        IPathFinding pathFinding = new SimpleAStar();
        var moveRange = 10; // 캐릭터의 이동 범위
        var reachableTiles = pathFinding.GetReachableTiles(start, moveRange, GameManager.Instance.getTileMap());

        if (reachableTiles.Count > 0)
        {
            Debug.Log("Reachable tiles found:");
            foreach (var tile in reachableTiles) Debug.Log($"Tile: {tile.x}, {tile.y}, {tile.z}");
        }
        else
        {
            Debug.Log("No reachable tiles found");
        }

        // A* 알고리즘을 통해 경로 찾기
        var path = pathFinding.FindPath(start, goal, GameManager.Instance.getTileMap());

        if (path.Count > 0)
        {
            Debug.Log("Path found:");
            foreach (var tile in path) Debug.Log($"Tile: {tile.x}, {tile.y}, {tile.z}");
        }
        else
        {
            Debug.Log("Path not found");
        }
    }
}