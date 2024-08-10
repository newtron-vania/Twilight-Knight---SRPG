using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class Character : MonoBehaviour
{
    public int CharacterID { get; private set; }
    
    [SerializeField] private Define.BehaviourState _characterState = Define.BehaviourState.Idle;
    public Define.BehaviourState CharacterState
    {
        get { return _characterState; }
        set { _characterState = value; }
    }

    
    public CharacterData _characterData;
    [FormerlySerializedAs("CharacterAnimationController")] private CharacterAnimationController characterAnimationController;

    [FormerlySerializedAs("AttackCommand")]
    public ACommandBehaviour attackCommand;

    [FormerlySerializedAs("MoveCommand")] public ACommandBehaviour moveCommand;
    [FormerlySerializedAs("SkillCommand")] public List<ACommandBehaviour> skillCommandList = new List<ACommandBehaviour>();
    private int _currentSkillIndex = 0;
    private int _maxSkillCount = 4;
    
    [SerializeField] private float currentTime = 0f;


    public void Initialize(int characterIUd)
    {
        if (CharacterID == 0) // characterIUd가 아직 초기화되지 않은 경우에만 설정
        {
            CharacterID = characterIUd;
        }

        _characterData = gameObject.GetOrAddComponent<CharacterData>();
        characterAnimationController = gameObject.GetOrAddComponent<CharacterAnimationController>();
        characterAnimationController.Init();

        InitCommand();
    }

    public void InitCommand()
    {
        if (attackCommand == null)
        {
            attackCommand = new BasicAttack();
        }

        attackCommand.Init(this, Define.CommandType.Attack);
        if (moveCommand == null)
        {
            moveCommand = new BasicMove();
        }
        moveCommand.Init(this, Define.CommandType.Move);
        foreach (var VARIABLE in skillCommandList)
        {
            VARIABLE.Init(this, Define.CommandType.Skill);
        }
    }
    public void Update()
    {
        switch(CharacterState)
        {
            case Define.BehaviourState.Idle:
                return;
            case Define.BehaviourState.Attack:
                attackCommand?.OnUpdate();
                return;
            case Define.BehaviourState.Move:
                moveCommand?.OnUpdate();
                return; 
            case Define.BehaviourState.Skill:
                skillCommandList[_currentSkillIndex]?.OnUpdate();
                return;
            case Define.BehaviourState.Hit:
                HitState();
                return;
            case Define.BehaviourState.Die:
                DieState();
                return;
        }
    }

    public void ChangeCommand(Define.CommandType commandType, ACommandBehaviour commandBehaviour)
    {
        if (commandType == Define.CommandType.Skill)
        {
            Debug.Log($"If you want to change skill command, please use ChangeSkillCommand");
        }

        switch (commandType)
        {
            case Define.CommandType.Attack:
                attackCommand = commandBehaviour;
                break;
            case Define.CommandType.Move:
                moveCommand = commandBehaviour;
                break;
        }
        commandBehaviour.Init(this, commandType);
        ChangeAnimationClip(commandType.ToString(), commandBehaviour.AnimationClipData);
    }

    public void ChangeSkillCommand(int index, ACommandBehaviour commandBehaviour)
    {
        if (skillCommandList.Count < index)
        {
            index = skillCommandList.Count;
        }
        
        if (index > _maxSkillCount)
        {
            Debug.Log($"More than the number of skills set.");
            return;
        }
        else if (index < 0)
        {
            Debug.Log("To low index number");
            return;
        }
        else if (index == skillCommandList.Count)
        {
            skillCommandList.Add(commandBehaviour);
        }
        else
        {
            skillCommandList[index] = commandBehaviour;
        }

        _currentSkillIndex = index;
        commandBehaviour.Init(this, Define.CommandType.Skill);
        
        //Hit 커맨드 필요유무에 따라 변형 실시. 현재 기본 애니메이션 클립에 설정
        
        //Die 커맨드 필요유무에 따라 변형 실시. 현재 기본 애니메이션 클립에 설정
    }

    public void ChangeAnimationClip(string originalClipName, Data.AnimationDataSO animationData)
    {
        characterAnimationController.ReplaceAnimation(originalClipName, animationData.animationClip);
    }

    public void PlayAnimation(string animationName, float normalizedTime = 0f)
    {
        characterAnimationController.Play(animationName);
    }
    
    public void PlayAnimation(string animationName, int layer, float normalizedTime = 0f)
    {
        characterAnimationController.Play(animationName, layer, normalizedTime);
    }

    public bool IsAnimationPlaying(string name)
    {
        return characterAnimationController.IsAnimationPlaying(name);
    }

    public void ChangeState(Define.BehaviourState behaviourState)
    {
        PlayAnimation(behaviourState.ToString());
        CharacterState = behaviourState;
    }

    public void ExecuteAttackCommand()
    {
        attackCommand?.Execute();
    }

    
    public void ExecuteMoveCommand()
    {
        moveCommand?.Execute();
    }

    public void ExecuteSkillCommand(int index)
    {
        if (index < 0 || index >= skillCommandList.Count)
        {
            Debug.Log($"Invalid Index");
            return;
        }

        if (skillCommandList[index] != null)
        {
            _currentSkillIndex = index;
            skillCommandList[_currentSkillIndex]?.Execute();
            ChangeAnimationClip("Skill", skillCommandList[_currentSkillIndex]?.AnimationClipData);
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (_characterData == null) return;
        if (damage <= 0) return;
        
        ChangeState(Define.BehaviourState.Hit);
        
        _characterData.Health -= damage;
        if (_characterData.Health <= 0)
        {
            Die();
        }
    }

    public void HitState()
    {
        if(CharacterState == Define.BehaviourState.Hit && !IsAnimationPlaying("Hit"))
        {
            ChangeState(Define.BehaviourState.Idle);
        }
    }


    private void Die()
    {
        if (_characterData == null) return;

        ChangeState(Define.BehaviourState.Die);
        
        
        Debug.Log($"{CharacterID} has died.");

        currentTime = 0;

    }
    [SerializeField] private float MaxdeathTime = 3f;
    
    private void DieState()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= MaxdeathTime)
        {
            GameManager.Instance.RemoveCharacter(CharacterID);
        }
    }

    private void DieEnd()
    {
        gameObject.SetActive(false);
    }

    public int GetAttackRange()
    {
        if (attackCommand == null)
        {
            return 0;
        }
        return attackCommand.Range;
    }
    
    public int GetMoveRange()
    {
        if (moveCommand == null)
        {
            return _characterData.MoveRange;
        }
        return moveCommand.Range;
    }
    
    public int GetSkillRange(int index)
    {
        if (index < 0 || index > skillCommandList.Count)
        {
            return -1;
        }
        
        return skillCommandList[index].Range;
    }
}
