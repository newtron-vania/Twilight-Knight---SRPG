using System.Collections.Generic;

public interface ITarget
{
    List<Tile> GetAccessibleTiles(Character caster, int inputRange, TileMap tileMap);

    Tile GetTarget();

    bool SetTarget(Tile inputTile);
}