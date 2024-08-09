using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ACommandBehaviour
{
    public abstract int BehaviourId { get; }

    protected int _minValue = -99;
    protected int _range = -99;

    public abstract int Range { get;}

    protected Data.AnimationDataSO _animationClipData;
    public abstract Data.AnimationDataSO AnimationClipData { get; }
    public abstract void Execute();

    public abstract bool Undo();
    
    protected Character _caster;

    public bool Init(Character caster)
    {
        if (caster == null)
        {
            return false;
        }

        SetAnimationEvent();
        return true;
    }

    public abstract void SetAnimationEvent();
}
