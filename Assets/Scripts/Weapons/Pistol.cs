using System.Collections.Generic;
using UnityEngine;

public class Pistol : WeaponBase
{
    public override void InitializeStats()
    {
        damage = 4;
        magazineSize = 6;
        cost = 2;
    }

    public override List<Tile> GetWeaponRange(int x, int y)
    {
        List<Tile> tiles = new List<Tile>
        {
            BattleManager.Instance.board.tiles[x, y]
        };
        return tiles;
    }
}
