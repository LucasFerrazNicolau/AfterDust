using System.Collections.Generic;
using UnityEngine;

public class Sniper : WeaponBase
{
    public override void InitializeStats()
    {
        damage = 9;
        magazineSize = 3;
        cost = 9;
    }

    public override List<Tile> GetWeaponRange(int x, int y)
    {
        List<Tile> tiles = new List<Tile>
        {
            BattleManager.Instance.board.tiles[x, 0],
            BattleManager.Instance.board.tiles[x, 1],
            BattleManager.Instance.board.tiles[x, 2]
        };
        return tiles;
    }
}
