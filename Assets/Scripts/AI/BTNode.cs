using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BTNode
{
    public enum State
    {
        Running,
        Success,
        Failure
    }

    protected State currentState;

    public BTNode()
    {
        currentState = State.Running;
    }

    public State CurrentState => currentState;

    public abstract State Execute();
}


public abstract class CompositeNode : BTNode
{
    protected List<BTNode> children = new List<BTNode>();

    public CompositeNode(List<BTNode> children)
    {
        this.children = children;
    }
}

public class Selector : CompositeNode
{
    public Selector(List<BTNode> children) : base(children) { }

    public override State Execute()
    {
        foreach (BTNode child in children)
        {
            State result = child.Execute();
            if (result == State.Success)
            {
                currentState = State.Success;
                return currentState;
            }
            else if (result == State.Running)
            {
                currentState = State.Running;
                return currentState;
            }
        }

        currentState = State.Failure;
        return currentState;
    }
}

public class Sequence : CompositeNode
{
    public Sequence(List<BTNode> children) : base(children) { }

    public override State Execute()
    {
        bool anyChildRunning = false;

        foreach (BTNode child in children)
        {
            State result = child.Execute();
            if (result == State.Failure)
            {
                currentState = State.Failure;
                return currentState;
            }
            else if (result == State.Running)
            {
                anyChildRunning = true;
            }
        }

        currentState = anyChildRunning ? State.Running : State.Success;
        return currentState;
    }
}


public class ActionNode : BTNode
{
    private Func<State> action;

    public ActionNode(Func<State> action)
    {
        this.action = action;
    }

    public override State Execute()
    {
        if (action != null)
        {
            currentState = action.Invoke();
        }
        else
        {
            currentState = State.Failure;
        }

        return currentState;
    }
}