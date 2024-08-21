using UnityEngine;

public class UI_TestUI : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private int[] pos = new int[3];

    private void Update()
    {
        var targetTile = GameManager.Instance.getTileMap().GetTile(pos[0], pos[1], pos[2]);
        if (_character == null || targetTile == null)
            return;
        if (_character.attackCommand == null)
        {
            Debug.LogError("Attack Command is not initialized.");
            return;
        }

        var attackTarget = _character.attackCommand as ITarget;
        if (attackTarget == null)
        {
            Debug.LogError("Attack Command does not implement ITarget.");
            return;
        }

        (_character.attackCommand as ITarget).SetTarget(targetTile);
        (_character.moveCommand as ITarget).SetTarget(targetTile);
    }

    public void StartAttackCommand()
    {
        _character.ExecuteAttackCommand();
    }

    public void StartMoveCommand()
    {
        _character.ExecuteMoveCommand();
    }
}