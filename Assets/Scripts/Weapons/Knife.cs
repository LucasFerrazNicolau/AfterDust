using System.Collections.Generic;
using UnityEngine;

public class Knife : WeaponBase
{
    public override void InitializeStats()
    {
        damage = 10;
        magazineSize = 0;
        cost = 4;
    }

    public override List<Tile> GetWeaponRange(int x, int y)
    {
        if (y == 0)
        {
            List<Tile> tiles = new List<Tile>
            {
                BattleManager.Instance.board.tiles[x, y]
            };
            return tiles;
        }
        else
        {
            return null;
        }
    }
}
