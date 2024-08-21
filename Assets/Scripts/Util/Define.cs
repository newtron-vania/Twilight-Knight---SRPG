public class Define
{
    public enum BehaviourState
    {
        None,
        Idle,
        Waiting,
        Attack,
        Move,
        Skill,
        Hit,
        Die
    }

    public enum BGMs
    {
    }

    public enum CommandType
    {
        None,
        Idle,
        Attack,
        Move,
        Skill
    }

    public enum PopupUIGroup
    {
        Unknown,
        UI_GameMenu,
        UI_CharacterCommand
    }

    public enum SceneType
    {
        Unknown,
        GameScene,
        MainMenuScene
    }

    public enum SceneUI
    {
        Unknown,
        UI_Player,
        UI_MainMenu
    }

    public enum Sound
    {
        bgm,
        effect,
        MaxCount
    }

    public enum UIEvent
    {
        Click,
        Drag
    }

    public enum WorldObject
    {
        Unknown,
        Player,
        Enemy
    }
    
    public enum TouchEvent
    {
        TouchBegin,
        Touch,
        Drag,
        TouchEnd
    }

    public enum SelectTileColor
    {
        Blue,
        Red,
        White
    }
}