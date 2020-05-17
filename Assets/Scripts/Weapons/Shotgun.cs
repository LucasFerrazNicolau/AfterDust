using System.Collections.Generic;
using UnityEngine;

public class Shotgun : WeaponBase
{
    public override void InitializeStats()
    {
        damage = 7;
        magazineSize = 3;
        cost = 4;
    }

    public override List<Tile> GetWeaponRange(int x, int y)
    {
        if (y == 0)
        {
            List<Tile> tiles = new List<Tile>
            {
                BattleManager.Instance.board.tiles[x, y],
                BattleManager.Instance.board.tiles[x, y + 1]
            };

            if (x > 0)
                tiles.Add(BattleManager.Instance.board.tiles[x - 1, y + 1]);

            if (x < 2)
                tiles.Add(BattleManager.Instance.board.tiles[x + 1, y + 1]);

            return tiles;
        }
        else
        {
            return null;
        }
    }
}
