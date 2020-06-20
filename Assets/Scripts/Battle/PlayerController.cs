using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isDead;

    // Initial Weapons
    public GameObject pistol;
    public GameObject shotgun;
    public GameObject sniper;
    public GameObject punch;
    public GameObject knife;

    // Battle Stats
    public int maxHealth;
    public int currentHealth;
    public int defense;

    // Non Battle Stats
    public int money;

    // Weapons
    public WeaponBase[] weapons;
    public int equippedWeaponIndex;
    public WeaponBase EquippedWeapon
    {
        get
        {
            return weapons[equippedWeaponIndex];
        }
    }

    private void Awake()
    {
        InitializeStats();

        LoadWeapons();

        isDead = false;
    }

    private void Update()
    {
        if (isDead)
            BattleManager.Instance.isGameOver = true;
    }

    public void InitializeStats()
    {
        maxHealth = 20;
        currentHealth = maxHealth;
        defense = 1;
        money = 0;
    }

    public void LoadWeapons()
    {
        switch (BattleManager.Instance.level)
        {
            case 1:
                InitializeWeapons(pistol, shotgun, punch);
                break;

            case 2:
                InitializeWeapons(pistol, shotgun, punch);
                break;

            case 3:
                InitializeWeapons(pistol, sniper, punch);
                break;

            case 4:
                InitializeWeapons(pistol, shotgun, knife);
                break;

            case 5:
                InitializeWeapons(pistol, sniper, knife);
                break;

            case 6:
                InitializeWeapons(pistol, shotgun, knife);
                break;
        }
    }

    private void InitializeWeapons(GameObject w1, GameObject w2, GameObject w3)
    {
        weapons = new WeaponBase[3];
        equippedWeaponIndex = 0;

        InitializeWeapon(w1, 0);
        InitializeWeapon(w2, 1);
        InitializeWeapon(w3, 2);
    }

    private void InitializeWeapon(GameObject weapon, int index)
    {
        GameObject w = Instantiate(weapon, Vector3.zero, Quaternion.identity);
        w.transform.parent = transform;

        SpriteRenderer sr = weapon.GetComponent<SpriteRenderer>();
        sr.enabled = false;

        weapons[index] = w.GetComponent<WeaponBase>();
    }

    public int TakeDamage(EnemyBase enemy)
    {
        int damage = Mathf.CeilToInt(enemy.attack * 1.0f / defense);
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        if (currentHealth == 0)
            isDead = true;

        return damage;
    }

    public void Shoot()
    {
        if (EquippedWeapon.magazineSize > 0)
        {
            EquippedWeapon.currentMagazine--;
        }

        EquippedWeapon.PlayAttackSound();
    }
}
