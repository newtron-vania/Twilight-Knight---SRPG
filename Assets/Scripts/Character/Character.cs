using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Character : MonoBehaviour
{
    public int CharacterID { get; private set; }
    
    public CharacterData _characterData;
    [FormerlySerializedAs("CharacterAnimationController")] private CharacterAnimationController characterAnimationController;
    
    [FormerlySerializedAs("AttackCommand")] public ACommandBehaviour attackCommand;
    [FormerlySerializedAs("MoveCommand")] public ACommandBehaviour moveCommand;
    [FormerlySerializedAs("SkillCommand")] public List<ACommandBehaviour> skillCommandList = new List<ACommandBehaviour>();
    private int maxSkillCount = 4;
    
    public void Initialize(int characterIUd)
    {
        if (CharacterID == 0) // characterIUd가 아직 초기화되지 않은 경우에만 설정
        {
            CharacterID = characterIUd;
        }

        characterAnimationController = gameObject.GetOrAddComponent<CharacterAnimationController>();
        characterAnimationController.Init();
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

        commandBehaviour.SetCaster(this);
        ChangeAnimationClip(commandType.ToString(), commandBehaviour.AnimationClipData);
    }

    public void ChangeSkillCommand(int index, ACommandBehaviour commandBehaviour)
    {
        if (skillCommandList.Count < index)
        {
            index = skillCommandList.Count;
        }
        
        if (index > maxSkillCount)
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

        commandBehaviour.SetCaster(this);
    }

    public void ChangeAnimationClip(string originalClipName, Data.AnimationDataSO animationData)
    {
        characterAnimationController.ReplaceAnimation(originalClipName, animationData.animationClip);
    }

    public void Play(string animationName, float normalizedTime)
    {
        characterAnimationController.Play(animationName, normalizedTime);
    }

    public void Attack()
    {
        attackCommand?.Execute(this);
    }
    // Update is called once per frame
    public void Move()
    {
        moveCommand?.Execute(this);
    }

    public void UseSkill(int index)
    {
        if (index < 0 || index >= skillCommandList.Count)
        {
            Debug.Log($"Invalid Index");
            return;
        }

        if (skillCommandList[index] != null)
        {
            ChangeAnimationClip("Skill", skillCommandList[index]?.AnimationClipData);
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (_characterData == null) return;

        _characterData.Health -= damage;

        if (_characterData.Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // 캐릭터가 사망했을 때의 로직 구현
        Debug.Log($"{CharacterID} has died.");
        // 예: 게임 오브젝트 비활성화, 사망 애니메이션 재생, 게임 상태 업데이트 등
        //gameObject.SetActive(false);
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
    
    public Tile GetCurrentTile()
    {
        return _characterData.CurrentTile;
    }
}
