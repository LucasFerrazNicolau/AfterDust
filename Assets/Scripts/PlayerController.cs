using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool isDead;

    public Texture2D mouseCursor;

    public Image ranged1;
    public Image ranged2;
    public Image melee;

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

    private void Start()
    {
        isDead = false;

        Cursor.SetCursor(mouseCursor, Vector2.zero, CursorMode.ForceSoftware);

        InitializeStats();
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

    public void InitializeWeapons(GameObject w1, GameObject w2, GameObject w3)
    {
        // Temporary development weapons
        weapons = new WeaponBase[3];
        equippedWeaponIndex = 0;

        InitializeWeapon(w1, 0, ranged1);
        InitializeWeapon(w2, 1, ranged2);
        InitializeWeapon(w3, 2, melee);
    }

    private void InitializeWeapon(GameObject weapon, int index, Image uiImage)
    {
        GameObject w = Instantiate(weapon, Vector3.zero, Quaternion.identity);
        w.transform.parent = transform;

        SpriteRenderer sr = w.GetComponent<SpriteRenderer>();
        sr.enabled = false;
        uiImage.sprite = sr.sprite;
        uiImage.enabled = true;

        weapons[index] = w.GetComponent<WeaponBase>();
    }

    public void TakeDamage(EnemyBase enemy)
    {
        int damage = Mathf.CeilToInt(enemy.attack * 1.0f / defense);
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        Debug.Log(enemy + " deals " + damage + " damage to player");

        if (currentHealth == 0)
            isDead = true;
    }
}
