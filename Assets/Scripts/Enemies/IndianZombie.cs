using UnityEngine;

public class IndianZombie : EnemyBase
{
    public override void InitializeStats()
    {
        maxHealth = 7;
        attack = 2;
        defense = 1;
        reward = 2;
    }

    public override void MakeAttack()
    {
        int damage = BattleManager.Instance.player.TakeDamage(this);
        PlayAttackAnimation(damage);
        PlayAttackSound();
    }

    public override void MakeMove()
    {
        int xPos = currentTile.indexX;
        int yPos = currentTile.indexY;

        if (yPos < 2 && board.tiles[xPos, yPos + 1].enemy == null)
        {
            board.MoveTo(this, xPos, yPos + 1);
        }
        else
        {
            int[] xPosRngs = new int[3];
            int maxIndex = 0;

            if (xPos > 0 && board.tiles[xPos - 1, yPos].enemy == null)
                xPosRngs[0] = Random.Range(0, 100);

            xPosRngs[1] = Random.Range(0, 100);

            if (xPosRngs[1] >= xPosRngs[maxIndex])
                maxIndex = 1;

            if (xPos < 2 && board.tiles[xPos + 1, yPos].enemy == null)
                xPosRngs[2] = Random.Range(0, 100);

            if (xPosRngs[2] > xPosRngs[maxIndex])
                maxIndex = 2;

            board.MoveTo(this, xPos + maxIndex - 1, yPos);
        }

        if (xPos != currentTile.indexX || yPos != currentTile.indexY)
        {
            PlayMovementWaitAnimation();
            PlayMovementSound();
        }
    }
}
