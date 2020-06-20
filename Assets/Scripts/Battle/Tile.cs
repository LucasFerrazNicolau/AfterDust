using UnityEngine;

public class Tile
{
    public int indexX;
    public int indexY;
    public float x;
    public float y;
    public int z;
    public float size;
    public EnemyBase enemy;
    public SpriteRenderer pad;

    public void Aim()
    {
        pad.enabled = true;
    }

    public void Neglect()
    {
        pad.enabled = false;
    }
}
