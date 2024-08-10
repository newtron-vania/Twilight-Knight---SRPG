using Data;

public abstract class ACommandBehaviour
{
    protected AnimationDataSO _animationClipData;

    protected Character _caster;

    protected int _minValue = -99;
    protected int _range = -99;
    public abstract int BehaviourId { get; }

    public Define.CommandType CommandType { get; private set; } = Define.CommandType.None;

    public abstract int Range { get; }
    public abstract AnimationDataSO AnimationClipData { get; }
    public abstract void Execute();

    public abstract void OnUpdate();

    public abstract bool Undo();

    public bool Init(Character caster, Define.CommandType state)
    {
        if (caster == null) return false;
        _caster = caster;
        CommandType = state;
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
    }
}