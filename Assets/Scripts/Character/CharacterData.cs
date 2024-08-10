using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private int attackPower = 10;
    [SerializeField] private int moveRange = 5;
    private Tile currentTile = new Tile(0,0,0);

    public int Health
    {
        get => health;
        set => health = value;
    }

    public int AttackPower
    {
        get => attackPower;
        set => attackPower = value;
    }

    public int MoveRange
    {
        get => moveRange;
        set => moveRange = value;
    }

    public Tile CurrentTile
    {
        get => currentTile;
        set
        {
            TileMap tileMap = GameManager.Instance.getTileMap();
            tileMap.SetTileObject(currentTile, null);
            currentTile = value;
            tileMap.SetTileObject(currentTile, gameObject);
            transform.position = GameManager.Instance.getTileMap().GetWorldPositionByTile(currentTile);
        }
    }
}
