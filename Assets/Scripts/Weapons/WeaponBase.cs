using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    // Stats
    public int damage;
    public int magazineSize;
    public int currentMagazine;
    public int cost;

    // Abstract methods
    public abstract void InitializeStats();
    public abstract List<Tile> GetWeaponRange(int x, int y);

    private void Awake()
    {
        InitializeStats();
        currentMagazine = magazineSize;
    }
}
