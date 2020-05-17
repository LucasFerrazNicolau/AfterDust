using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public bool isDead;

    private SpriteRenderer sr;

    public Material defaultMaterial;
    public Material aimMaterial;

    // Battle Stats
    public int maxHealth;
    public int currentHealth;
    public int attack;
    public int defense;

    // Non Battle Stats
    public int reward;

    // Objects
    public BoardController board;
    public Tile currentTile;

    // Abstract methods
    public abstract void InitializeStats();
    public abstract void MakeMove();
    public abstract void MakeAttack();

    private void Awake()
    {
        isDead = false;

        InitializeStats();
        currentHealth = maxHealth;
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isDead)
            Destroy(gameObject);
    }

    private void OnMouseEnter()
    {
        if (!BattleManager.Instance.locked &&
            BattleManager.Instance.state == BattleManager.BattleState.PlayerMovement)
        {
            int x = currentTile.indexX;
            int y = currentTile.indexY;
            List<Tile> aimedTiles = BattleManager.Instance.player.EquippedWeapon.GetWeaponRange(x, y);

            BattleManager.Instance.NeglectEnemies();

            if (aimedTiles != null && aimedTiles.Count > 0)
            {
                foreach (var tile in aimedTiles)
                    if (tile.enemy != null)
                        BattleManager.Instance.AimEnemy(tile.enemy);
            }
            else
            {
                // show to player that there are no targets
            }

            // treat when no enemies are hitted
        }
    }

    private void OnMouseExit()
    {
        if (!BattleManager.Instance.locked &&
            BattleManager.Instance.state == BattleManager.BattleState.PlayerMovement)
        {
            BattleManager.Instance.NeglectEnemies();
        }
    }

    public void TakeDamage()
    {
        int damage = Mathf.CeilToInt(BattleManager.Instance.player.EquippedWeapon.damage * 1.0f / defense);
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        Debug.Log("Player deal " + damage + " damage to " + this);

        PlayDamageAnimation(damage);
    }

    public void Die()
    {
        currentTile.enemy = null;
        board.enemies.Remove(this);

        BattleManager.Instance.player.money += reward;

        //isDead = true;
        PlayeDeathAnimation();
    }

    public void Terminate()
    {
        isDead = true;
    }

    public void PlayDamageAnimation(int damage)
    {
        BattleManager.Instance.animations.Insert(0,
            sr.DOFade(0, 0.1f).SetLoops(4, LoopType.Yoyo)
        );
    }

    public void PlayeDeathAnimation()
    {
        BattleManager.Instance.animations.Insert(0,
            sr.DOFade(0, 1).OnComplete(Terminate)
        );
    }

    public void PlayAttackAnimation(int damage)
    {

    }
}
