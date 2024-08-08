using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : Singleton<GameManager>
{
    private TileMap TileMap;
    
    public TileMap getTileMap()
    {
        if (TileMap == null)
        {
            int x = 30;
            int y = 30;
            int z = 10;
            TileMap = new TileMap(x, y, z);
        }
        
        return TileMap;
    }
    
    public int currentTurn = 0;

    private Dictionary<int, Character> characters = new Dictionary<int, Character>();
    private int playerCharacterCount = 0;
    private int enemyCharacterCount = 0;
    private string defaultCharactername = "MaleCharacterPBR";
    private Character CreateCharacter(bool isPlayerCharacter, string characterName = null)
    {
        GameObject characterObject;

        if (string.IsNullOrEmpty(characterName))
        {
            characterObject = ResourceManager.Instance.Instantiate($"Characters/{defaultCharactername}");
        }
        else
        {
            // 지정된 이름의 프리팹을 로드
            characterObject = ResourceManager.Instance.Instantiate($"Characters/{characterName}");

            if (characterObject == null)
            {
                // 지정된 이름의 프리팹이 없으면 기본 프리팹 사용
                characterObject = ResourceManager.Instance.Instantiate(defaultCharactername);
            }
        }

        Character character = characterObject.AddComponent<Character>();
        int characterId = isPlayerCharacter ? (1 << 8) + playerCharacterCount++ : enemyCharacterCount++;
        character.Initialize(characterId);
        return character;
    }

    public Character AddCharacter(bool isPlayerCharacter, string characterName = null)
    {
        Character character = CreateCharacter(isPlayerCharacter, characterName);
        characters[character.CharacterID] = character;
        UpdateCharacterCounts();
        return character;
    }

    public void RemoveCharacter(int characterId)
    {
        if (characters.ContainsKey(characterId))
        {
            characters.Remove(characterId);
            UpdateCharacterCounts();
        }
    }

    public Character GetCharacterById(int characterId)
    {
        if (characters.ContainsKey(characterId))
        {
            return characters[characterId];
        }
        return null; // 해당 캐릭터가 없을 경우
    }

    private void UpdateCharacterCounts()
    {
        playerCharacterCount = 0;
        enemyCharacterCount = 0;

        foreach (var character in characters.Values)
        {
            if ((character.CharacterID & (1 << 8)) != 0)
            {
                playerCharacterCount++;
            }
            else
            {
                enemyCharacterCount++;
            }
        }
    }

    public int GetPlayerCharacterCount()
    {
        return playerCharacterCount;
    }

    public int GetEnemyCharacterCount()
    {
        return enemyCharacterCount;
    }
    
    public int GetCharacterAttackRange(int characterID)
    {
        var character = GetCharacterById(characterID);
        if (character != null)
        {
            return character.GetAttackRange();
        }
        return -1; // 잘못된 ID 처리
    }

    public int GetCharacterMoveRange(int characterID)
    {
        var character = GetCharacterById(characterID);
        if (character != null)
        {
            return character.GetMoveRange();
        }
        return -1; // 잘못된 ID 처리
    }

    public int GetCharacterSkillRange(int characterID, int skillIndex)
    {
        var character = GetCharacterById(characterID);
        if (character != null)
        {
            return character.GetSkillRange(skillIndex);
        }
        return -1; // 잘못된 ID 처리
    }
}
