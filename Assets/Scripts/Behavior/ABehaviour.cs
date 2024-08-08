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
    
    public abstract Data.AnimationDataSO AnimationClipData { get; }
    public abstract void Execute(Character caster);

    public abstract bool Undo(Character caster);
    
    protected Character _caster;

    public bool SetCaster(Character caster)
    {
        if (caster == null)
        {
            return false;
        }

        return true;
    }
}
