using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BasicMove : ACommandBehaviour, IMove, ITarget
{
    public override int BehaviourId
    {
        get { return 1 << 16 | 1 << 8 | 1; }
    }

    private Tile _targetTile; // 이동할 목표 타일
    private IPathFinding _pathFinding; // PathFinding 인스턴스를 참조할 변수
    private List<Tile> _accessibleTiles; // 접근 가능한 타일
    private readonly float moveSpeed = 2.0f; // 이동 속도
    private Tile previousTile;

    public override int Range
    {
        get
        {
            if (_range <= _minValue) _range = 0;
            return _range + _caster._characterData.MoveRange;
        }
    }

    public override Data.AnimationDataSO AnimationClipData
    {
        get { return DataManager.Instance.AnimationData["BasicMove"]; }
    }

    public override void Execute(Character caster)
    {
        Move(caster);
    }

    public override bool Undo(Character caster)
    {
        if (previousTile != null && previousTile != _targetTile)
        {
            caster._characterData.CurrentTile = previousTile;
            
            return true;
        }

        return false;
    }

    public void Move(Character caster)
    {
        if (_pathFinding == null)
        {
            // 기본 PathFinding 인스턴스를 생성하거나 가져옵니다.
            _pathFinding = new SimpleAStar(); // 여기에 맞는 PathFinding 구현체를 할당
        }

        TileMap tileMap = GameManager.Instance.getTileMap();

        // 접근 가능한 타일을 가져옵니다.
        _accessibleTiles = GetAccessibleTiles(caster, caster.GetMoveRange(), tileMap);
        
        // 목표 타일까지의 최단 경로를 찾습니다.
        List<Tile> path = FindPathToTarget(caster, _targetTile, tileMap);

        // 경로를 따라 이동합니다.
        caster.StartCoroutine(MoveAlongPath(caster, path, tileMap));
    }

    public List<Tile> GetAccessibleTiles(Character caster, int inputRange, TileMap tileMap)
    {
        if (_pathFinding == null)
        {
            // 기본 PathFinding 인스턴스를 생성하거나 가져옵니다.
            _pathFinding = new SimpleAStar(); // 여기에 맞는 PathFinding 구현체를 할당
        }
        
        // PathFinding을 통해 접근 가능한 타일 목록을 가져옵니다.
        return _accessibleTiles = _pathFinding.GetReachableTiles(caster.GetCurrentTile(), inputRange, tileMap);
    }

    public List<Tile> FindPathToTarget(Character caster, Tile targetTile, TileMap tileMap)
    {
        if (targetTile == null)
        {
            Debug.LogError("Target tile is not set.");
            return new List<Tile>();
        }

        // PathFinding을 통해 최단 경로를 찾습니다.
        return _pathFinding.FindPath(caster.GetCurrentTile(), targetTile, tileMap, _accessibleTiles);
    }

    private IEnumerator MoveAlongPath(Character caster, List<Tile> path, TileMap tileMap)
    {
        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("No path found or path is empty.");
            yield break;
        }

        // Move along the path
        foreach (var tile in path)
        {
            Vector3 targetPosition = tileMap.GetWorldPositionByTile(tile);
            while (Vector3.Distance(caster.transform.position, targetPosition) > 0.01f)
            {
                caster.transform.position = Vector3.MoveTowards(caster.transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        _accessibleTiles = null;
        _targetTile = null;
    }
    public Tile GetTarget()
    {
        return _targetTile;
    }

    public bool SetTarget(Tile inputTile)
    {
        if (inputTile == null)
        {
            return false;
        }

        _targetTile = inputTile;

        return true;
    }
    
}