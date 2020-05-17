using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private static BattleManager instance = null;

    public BattleState state;

    public PlayerController player;
    public BoardController board;
    public Sequence animations;
    public int waitTimer;

    // Temporary
    public GameObject zombie;
    public GameObject indianZombie;
    public GameObject pistol;
    public GameObject shotgun;
    public GameObject punch;

    private Queue<EnemyBase> toActEnemies;
    public List<EnemyBase> aimedEnemies;

    // Locking state variables
    public bool locked;
    public bool isAnimating;
    public bool isGameOver;

    public static BattleManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameObject("BM").AddComponent<BattleManager>();
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);

        state = BattleState.PlayerMovement;
        waitTimer = 0;

        toActEnemies = new Queue<EnemyBase>();
        aimedEnemies = new List<EnemyBase>();

        locked = false;
        isAnimating = false;
        isGameOver = false;
    }

    private void Start()
    {
        // Easy
        //board.CreateEnemy(zombie, 1, 2);

        // Medium
        //board.CreateEnemy(zombie, 1, 0);
        //board.CreateEnemy(indianZombie, 1, 1);

        // Hard
        board.CreateEnemy(zombie, 0, 2);
        board.CreateEnemy(zombie, 2, 2);
        board.CreateEnemy(indianZombie, 2, 0);

        player.InitializeWeapons(pistol, shotgun, punch);
    }

    private void Update()
    {
        if (isAnimating)
            return;

        if (board.enemies.Count == 0)
        {
            Debug.Log("You win");
            locked = true;
        }

        if (isGameOver)
        {
            Debug.Log("Game Over");
            locked = true;
        }

        switch (state)
        {
            case BattleState.PlayerMovement:
                if (Input.GetKeyDown(KeyCode.A))
                    player.equippedWeaponIndex = 0;

                if (Input.GetKeyDown(KeyCode.S))
                    player.equippedWeaponIndex = 1;

                if (Input.GetKeyDown(KeyCode.D))
                    player.equippedWeaponIndex = 2;

                if (Input.GetMouseButtonDown(0) &&
                    !locked &&
                    aimedEnemies.Count > 0 &&
                    (player.EquippedWeapon.currentMagazine > 0 ||
                    player.EquippedWeapon.magazineSize == 0))
                {
                    locked = true;

                    if (player.EquippedWeapon.magazineSize > 0)
                        player.EquippedWeapon.currentMagazine--;

                    state = BattleState.EnemyDamage;
                }

                break;

            case BattleState.EnemyDamage:
                //NeglectEnemies() NO -> remove downlight YES

                animations = DOTween.Sequence();

                foreach (var enemy in aimedEnemies)
                    enemy.TakeDamage();

                PlayBattleAnimation();

                state = BattleState.EnemyDead;

                break;

            case BattleState.EnemyDead:
                animations = DOTween.Sequence();

                foreach (var enemy in aimedEnemies)
                    if (enemy.currentHealth == 0)
                        enemy.Die();

                PlayBattleAnimation();

                state = BattleState.EnemySetup;

                break;

            case BattleState.EnemySetup:
                //NeglectEnemis()

                foreach (var enemy in board.enemies)
                    toActEnemies.Enqueue(enemy);

                state = BattleState.EnemyMovement;

                break;

            case BattleState.EnemyMovement:
                if (toActEnemies.Count > 0)
                {
                    EnemyBase currentEnemy = toActEnemies.Dequeue();
                    currentEnemy.MakeMove();
                    currentEnemy.MakeAttack();
                }

                if (!isGameOver && toActEnemies.Count == 0)
                {
                    state = BattleState.PlayerMovement;
                    locked = false;
                }

                break;
        }
    }

    public void AimEnemy(EnemyBase enemy)
    {
        // insert enemy in aimed list and highlight it
        aimedEnemies.Add(enemy);
        Debug.Log("Aiming in " + enemy);
    }

    public void NeglectEnemies()
    {
        foreach (var enemy in aimedEnemies)
        {
            Debug.Log("Aiming out " + enemy);
        }
        aimedEnemies.Clear();
    }

    public void PlayBattleAnimation()
    {
        isAnimating = true;
        animations.OnComplete(EndBattleAnimation);
        animations.Play();
    }

    public void EndBattleAnimation()
    {
        isAnimating = false;
    }

    public enum BattleState
    {
        PlayerMovement = 0,
        EnemyDamage = 1,
        EnemyDead = 2,
        EnemySetup = 3,
        EnemyMovement = 4
    }
}
