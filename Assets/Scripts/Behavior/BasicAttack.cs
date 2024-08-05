using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : ACommandBehaviour, ITarget
{
    public override int BehaviourId
    {
        get { return 1; }
    }

    public override void Execute(Character caster)
    {
        FindTarget(caster);
    }

    private void FindTarget(Character caster)
    {
        
    }

    public void GetTarget()
    {
        throw new System.NotImplementedException();
    }
}
