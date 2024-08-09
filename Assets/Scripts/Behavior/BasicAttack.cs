using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicAttack : ACommandBehaviour, ITarget
{
    public override int BehaviourId
    {
        get { return 1 << 16 | 1 << 0 | 1; }
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
        get
        {
            if (_animationClipData == null) { _animationClipData = DataManager.Instance.AnimationData["BasickAttack"]; }
            return _animationClipData;
        }
    }


    private Tile _targetTile; // 공격 목표 타일
    private IPathFinding _pathFinding; // PathFinding 인스턴스를 참조할 변수
    private List<Tile> _accessibleTiles; // 접근 가능한 타일
    private float _commandDamage = 1f;
    
    public int Damage
    {
        get { return Mathf.CeilToInt(_caster._characterData.AttackPower * _commandDamage); }
    }
    

    public override void Execute()
    {

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

    public override void SetAnimationEvent()
    {
        AnimationEvent animationEvent = new AnimationEvent();

        animationEvent.functionName = "Attack";
        animationEvent.time = 0.05f;
        AnimationClipData.animationClip.AddEvent(animationEvent);
    }

    public void Attack()
    {
        if (_targetTile == null || (!_accessibleTiles.Contains(_targetTile)))
        {
            return;
        }

        if (_targetTile.tileObject.TryGetComponent<Character>(out Character targetCharacter))
        {
            targetCharacter.TakeDamage(Damage);
        }

        _targetTile = null;
        _pathFinding = null;
    }
    
    public List<Tile> GetAccessibleTiles(Character caster, int inputRange, TileMap tileMap)
    {
        if (_pathFinding == null)
        {
            _pathFinding = new SimpleAStar();
        }

        return _accessibleTiles = _pathFinding.GetReachableTiles(caster.GetCurrentTile(), inputRange, tileMap);
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
