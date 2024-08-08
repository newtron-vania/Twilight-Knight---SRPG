using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget
{
    List<Tile> GetAccessibleTiles(Character caster, int inputRange, TileMap tileMap);
    
    Tile GetTarget();

    bool SetTarget(Tile inputTile);
}
