using System.Collections.Generic;
using Data;
using UnityEngine;

public class StrongAttack : ACommandBehaviour, ITarget
{
    private readonly float _commandDamage = 2f;
    private List<Tile> _accessibleTiles; // 접근 가능한 타일
    private float _currentTime;
    private IPathFinding _pathFinding; // PathFinding 인스턴스를 참조할 변수
    private float _rotationSpeed = 10f;


    private Tile _targetTile; // 공격 목표 타일
    private bool eventActive;

    public override int BehaviourId => (1 << 16) | (1 << 0) | 2;

    public override int Range
    {
        get
        {
            if (_range <= _minValue) _range = 1;
            return _range;
        }
    }

    public override AnimationDataSO AnimationClipData => DataManager.Instance.AnimationData["StrongAttack"];

    public int Damage => Mathf.CeilToInt(_caster._characterData.AttackPower * _commandDamage);

    public List<Tile> GetAccessibleTiles(Character caster, int inputRange, TileMap tileMap)
    {
        if (_pathFinding == null) _pathFinding = new SimpleAStar();

        return _accessibleTiles =
            _pathFinding.GetAttackableTiles(caster._characterData.CurrentTile, inputRange, tileMap);
    }

    public Tile GetTarget()
    {
        return _targetTile;
    }

    public bool SetTarget(Tile inputTile)
    {
        if (inputTile == null) return false;

        _targetTile = inputTile;

        return true;
    }


    public override void Execute()
    {
        Debug.Log("Attack Play");

        GetAccessibleTiles(_caster, Range, GameManager.Instance.getTileMap()); // 테스트

        foreach (var varialble in _accessibleTiles)
            Debug.Log($"_accessibleTile : {varialble.x}, {varialble.y}, {varialble.z}");

        if (_targetTile == null || !_accessibleTiles.Contains(_targetTile)) return;

        SetStateByCommandType(CommandType);
        eventActive = false;
    }

    public override void OnUpdate()
    {
        Attack();
    }

    public override bool Undo()
    {
        if (_targetTile.tileObject.TryGetComponent(out Character targetCharacter))
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
            if (_targetTile == null || !_accessibleTiles.Contains(_targetTile)) return;

            var tileMap = GameManager.Instance.getTileMap();
            var targetPosition = tileMap.GetWorldPositionByTile(_targetTile);
            var direction = (targetPosition - _caster.transform.position).normalized;

            var targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            _caster.transform.rotation = targetRotation * GameManager.Instance.defaultRotationValue;

            if (_targetTile.tileObject != null && _targetTile.tileObject.TryGetComponent(out Character targetCharacter))
            {
                Debug.Log("target " + targetCharacter.name + "Attack!");


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
}