using System;
using System.Collections.Generic;

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
    protected List<BTNode> children = new();

    public CompositeNode(List<BTNode> children)
    {
        this.children = children;
    }
}

public class Selector : CompositeNode
{
    public Selector(List<BTNode> children) : base(children)
    {
    }

    public override State Execute()
    {
        foreach (var child in children)
        {
            var result = child.Execute();
            if (result == State.Success)
            {
                currentState = State.Success;
                return currentState;
            }

            if (result == State.Running)
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
    public Sequence(List<BTNode> children) : base(children)
    {
    }

    public override State Execute()
    {
        var anyChildRunning = false;

        foreach (var child in children)
        {
            var result = child.Execute();
            if (result == State.Failure)
            {
                currentState = State.Failure;
                return currentState;
            }

            if (result == State.Running) anyChildRunning = true;
        }

        currentState = anyChildRunning ? State.Running : State.Success;
        return currentState;
    }
}


public class ActionNode : BTNode
{
    private readonly Func<State> action;

    public ActionNode(Func<State> action)
    {
        this.action = action;
    }

    public override State Execute()
    {
        if (action != null)
            currentState = action.Invoke();
        else
            currentState = State.Failure;

        return currentState;
    }
}