using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum WorldObject
    {
        Unknown,
        Player,
        Enemy
    }

    public enum PopupUIGroup
    {
        Unknown,
        UI_GameMenu,
        UI_ItemBoxOpen,
        UI_LevelUp,
        UI_GameOver,
        UI_GameVictory,
        UI_TimeStop,
        UI_CharacterSelect
    }

    public enum SceneUI
    {
        Unknown,
        UI_Player,
        UI_MainMenu,
    }

    public enum Sound
    {
        bgm,
        effect,
        MaxCount,
    }
    public enum BGMs
    {
        
    }
    public enum UIEvent
    {
        Click,
        Drag,
    }
    public enum SceneType
    {
        Unknown,
        GameScene,
        MainMenuScene
    }

    public enum CommandType
    {
        Attack,
        Move,
        Skill
    }
    
    public enum BehaviourState
    {
        Idle,
        Waiting,
        Attack,
        Move,
        Skill,
        Hit,
        Die,
    }
}
