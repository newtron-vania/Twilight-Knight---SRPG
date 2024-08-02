using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABehaviour : MonoBehaviour
{
    public abstract int BehaviourId { get; }

    public int range = 0;
    public abstract void Execute(Character caster);
}
