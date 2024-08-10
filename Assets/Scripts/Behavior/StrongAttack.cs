using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttack : ACommandBehaviour, ITarget
{
    public override int BehaviourId
    {
        get { return 1 << 16 | 1 << 0 | 2; }
    }

    public override int Range
    {
        get
        {
            if (_range <= _minValue) _range = 1;
            return _range; 
        }
    }

    public override Data.AnimationDataSO AnimationClipData
    {
        get { return DataManager.Instance.AnimationData["StrongAttack"]; }
    }


    private Tile _targetTile; // 공격 목표 타일
    private IPathFinding _pathFinding; // PathFinding 인스턴스를 참조할 변수
    private List<Tile> _accessibleTiles; // 접근 가능한 타일
    private float _commandDamage = 2f;
    private float _currentTime = 0f;
    private float _rotationSpeed = 10f;
    private bool eventActive = false;
    
    public int Damage
    {
        get { return Mathf.CeilToInt(_caster._characterData.AttackPower * _commandDamage); }
    }
    

    public override void Execute()
    {
        Debug.Log($"Attack Play");
        
        GetAccessibleTiles(_caster, Range, GameManager.Instance.getTileMap()); // 테스트

        foreach (var varialble in _accessibleTiles)
        {
            Debug.Log($"_accessibleTile : {varialble.x}, {varialble.y}, {varialble.z}");
        }
        
        if (_targetTile == null || (!_accessibleTiles.Contains(_targetTile)))
        {
            return;
        }
        
        SetStateByCommandType(CommandType);
        eventActive = false;
    }

    public override void OnUpdate()
    {
        Attack();
    }

    public override bool Undo()
    {
        if (_targetTile.tileObject.TryGetComponent<Character>(out Character targetCharacter))
        {
            targetCharacter.TakeDamage(-Damage);

            return true;
        }

        return false;
    }
    
    private void Attack()
    {
        if (_currentTime >= 0.08f && !eventActive)
        {
            if (_targetTile == null || (!_accessibleTiles.Contains(_targetTile)))
            {
                return;
            }

            TileMap tileMap = GameManager.Instance.getTileMap();
            Vector3 targetPosition = tileMap.GetWorldPositionByTile(_targetTile);
            Vector3 direction = (targetPosition - _caster.transform.position).normalized;
            
            if (_targetTile.tileObject.TryGetComponent<Character>(out Character targetCharacter))
            {
                Debug.Log("target " + targetCharacter.name + "Attack!");
                
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
                _caster.transform.rotation = targetRotation *  GameManager.Instance.defaultRotationValue;
                targetCharacter.TakeDamage(Damage);
            }

            eventActive = true;
            return;
        }

        _currentTime += Time.deltaTime;
        
        if (eventActive && !_caster.IsAnimationPlaying(CommandType.ToString()))
        {
            _targetTile = null;
            SetStateByCommandType(Define.CommandType.Idle);
        }
    }
    
    public List<Tile> GetAccessibleTiles(Character caster, int inputRange, TileMap tileMap)
    {
        if (_pathFinding == null)
        {
            _pathFinding = new SimpleAStar();
        }

        return _accessibleTiles = _pathFinding.GetAttackableTiles(caster._characterData.CurrentTile, inputRange, tileMap);
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
