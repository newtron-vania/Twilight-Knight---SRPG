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
    private TileMap tileMap;
    private List<Tile> _path;
    private int _currentTileIndex = 0;
    
    
    private readonly float _moveSpeed = 2.0f; // 이동 속도
    private float _rotationSpeed = 10f;
    private Tile _previousTile;

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

    public override void Execute()
    {
        tileMap = GameManager.Instance.getTileMap();
        
        // 접근 가능한 타일을 가져옵니다.
        _accessibleTiles = GetAccessibleTiles(_caster, Range, tileMap);
        // 목표 타일까지의 최단 경로를 찾습니다.
        List<Tile> path = FindPathToTarget(_caster, _targetTile, tileMap);

        if (path == null || path.Count == 0)
        {
            Debug.LogWarning($"No path found or path is empty.");
            return;
        }
        
        _path = path;
        
        Debug.Log("Move Animation Start!");
        SetStateByCommandType(Define.CommandType.Move);
    }

    public override void OnUpdate()
    {
        Move();
    }

    public override bool Undo()
    {
        if (_previousTile != null && _previousTile != _targetTile)
        {
            _caster._characterData.CurrentTile = _previousTile;
            
            return true;
        }

        return false;
    }
    public void Move()
    {
        if (_path == null || _path.Count == 0)
        {
            Debug.LogWarning("No path found or path is empty.");
            _caster.ChangeState(Define.BehaviourState.Idle);
            return;
        }
        
        if (_currentTileIndex >= _path.Count)
        {
            _currentTileIndex = 0;
            _accessibleTiles = null;
            _targetTile = null;
            _path = null;
            Debug.Log($"Move to Idle Animation");
            _caster.ChangeState(Define.BehaviourState.Idle);
            return;
        }
        
        
        Vector3 targetPosition = tileMap.GetWorldPositionByTile(_path[_currentTileIndex]);
        Vector3 direction = (targetPosition - _caster.transform.position).normalized;

        if (Vector3.Distance(_caster.transform.position, targetPosition) > 0.01f)
        {
            _caster.transform.position = Vector3.MoveTowards(_caster.transform.position, targetPosition, _moveSpeed * Time.deltaTime);
            
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
                Quaternion targetRotation = lookRotation * GameManager.Instance.defaultRotationValue;
                _caster.transform.rotation = Quaternion.Slerp(_caster.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            _caster._characterData.CurrentTile = _path[_currentTileIndex];
            _currentTileIndex++;
        }
    }

    public List<Tile> GetAccessibleTiles(Character caster, int inputRange, TileMap tileMap)
    {
        if (_pathFinding == null)
        {
            // 기본 PathFinding 인스턴스를 생성하거나 가져옵니다.
            _pathFinding = new SimpleAStar(); // 여기에 맞는 PathFinding 구현체를 할당
        }
        
        // PathFinding을 통해 접근 가능한 타일 목록을 가져옵니다.
        return _accessibleTiles = _pathFinding.GetReachableTiles(caster._characterData.CurrentTile, inputRange, tileMap);
    }

    public List<Tile> FindPathToTarget(Character caster, Tile targetTile, TileMap tileMap)
    {
        if (targetTile == null)
        {
            Debug.LogError("Target tile is not set.");
            return new List<Tile>();
        }

        // PathFinding을 통해 최단 경로를 찾습니다.
        return _pathFinding.FindPath(caster._characterData.CurrentTile, targetTile, tileMap, _accessibleTiles);
    }

    // private IEnumerator MoveAlongPath(Character caster, List<Tile> path, TileMap tileMap)
    // {
    //     if (path == null || path.Count == 0)
    //     {
    //         Debug.LogWarning("No path found or path is empty.");
    //         yield break;
    //     }
    //
    //     // 경로를 따라 이동하기
    //     foreach (var tile in path)
    //     {
    //         Vector3 targetPosition = tileMap.GetWorldPositionByTile(tile);
    //         while (Vector3.Distance(caster.transform.position, targetPosition) > 0.01f)
    //         {
    //             caster.transform.position = Vector3.MoveTowards(caster.transform.position, targetPosition, _moveSpeed * Time.deltaTime);
    //             
    //             Vector3 direction = (targetPosition - caster.transform.position).normalized;
    //             
    //             if (direction != Vector3.zero)
    //             {
    //                 Quaternion lookRotation = Quaternion.LookRotation(direction);
    //                 caster.transform.rotation = Quaternion.Slerp(caster.transform.rotation, lookRotation,  _rotationSpeed * Time.deltaTime);
    //             }
    //             yield return null;
    //         }
    //     }
    //
    //     caster.transform.position = tileMap.GetWorldPositionByTile(path[path.Count - 1]);
    //     _accessibleTiles = null;
    //     _targetTile = null;
    // }
    
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