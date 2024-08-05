using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMove : ACommandBehaviour, IMove, ITarget
{
    public override int BehaviourId
    {
        get { return 1 << 8 | 1; }
    }

    public override void Execute(Character caster)
    {
        Move();
    }

    public void Move()
    {
        
    }

    public void GetTarget()
    {
        throw new System.NotImplementedException();
    }
}
