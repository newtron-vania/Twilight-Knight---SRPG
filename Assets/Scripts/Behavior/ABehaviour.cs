using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ACommandBehaviour
{
    public abstract int BehaviourId { get; }

    private Define.CommandType _commandType = Define.CommandType.None;
    public Define.CommandType CommandType
    {
        get { return _commandType; }
    }

    protected int _minValue = -99;
    protected int _range = -99;

    public abstract int Range { get;}

    protected Data.AnimationDataSO _animationClipData;
    public abstract Data.AnimationDataSO AnimationClipData { get; }
    public abstract void Execute();

    public abstract void OnUpdate();

    public abstract bool Undo();
    
    protected Character _caster;

    public bool Init(Character caster, Define.CommandType state)
    {
        if (caster == null)
        {
            return false;
        }
        _caster = caster;
        _commandType = state;
        _caster.ChangeAnimationClip(CommandType.ToString(), AnimationClipData);
        return true;
    }


    protected void SetStateByCommandType(Define.CommandType type)
    {
        if (_caster == null || type == Define.CommandType.None)
            return;
        
        switch (type)
        {
            case Define.CommandType.Idle:
                _caster.ChangeState(Define.BehaviourState.Idle);
                break;
            case Define.CommandType.Attack:
                _caster.ChangeState(Define.BehaviourState.Attack);
                break;
            case Define.CommandType.Move:
                _caster.ChangeState(Define.BehaviourState.Move);
                break;
            case Define.CommandType.Skill:
                _caster.ChangeState(Define.BehaviourState.Skill);
                break;
        }
        return;
    }
}
