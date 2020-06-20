using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    private static BattleManager instance = null;

    public BattleState state;

    public AudioSource audioSource;

    public PlayerController player;
    public BoardController board;
    private Image frontground;
    private WinSoundPlayer winSoundPlayer;

    public Sequence animations;

    // Enemies
    public GameObject zombie;
    public GameObject indianZombie;
    public GameObject minerZombie;
    public GameObject bat;
    public GameObject scorpion;

    private Queue<EnemyBase> toActEnemies;
    public List<EnemyBase> aimedEnemies;

    // Level control
    public int level;
    public bool enemiesLoaded;
    public bool isWin;
    public bool isPlayingWinSound;

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

        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        toActEnemies = new Queue<EnemyBase>();
        aimedEnemies = new List<EnemyBase>();

        level = 1;
        enemiesLoaded = false;
        isWin = false;
        isPlayingWinSound = false;

        locked = false;
        isAnimating = false;
        isGameOver = false;
    }

    private void Start()
    {
        LoadObjects();
        Cursor.SetCursor(player.EquippedWeapon.mouseCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void Update()
    {
        LoadObjects();

        if (winSoundPlayer.levelEnded)
            return;

        if (!enemiesLoaded && !isPlayingWinSound)
            LoadEnemies();

        if (isAnimating)
            return;

        CheckWin();
        CheckGameOver();

        switch (state)
        {
            case BattleState.PlayerMovement:
                CheckWeaponChange();
                CheckShoot();
                break;

            case BattleState.EnemyDamage:
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
                NeglectEnemies();

                foreach (var enemy in board.enemies)
                    toActEnemies.Enqueue(enemy);

                state = BattleState.EnemyMovement;

                break;

            case BattleState.EnemyMovement:
                if (toActEnemies.Count > 0)
                {
                    animations = DOTween.Sequence();

                    EnemyBase currentEnemy = toActEnemies.Peek();
                    currentEnemy.MakeMove();

                    PlayBattleAnimation();
                }

                state = BattleState.EnemyAttack;

                break;

            case BattleState.EnemyAttack:
                if (toActEnemies.Count > 0)
                {
                    animations = DOTween.Sequence();

                    EnemyBase currentEnemy = toActEnemies.Dequeue();
                    currentEnemy.MakeAttack();

                    PlayBattleAnimation();
                }

                if (toActEnemies.Count == 0)
                {
                    state = BattleState.PlayerMovement;
                    locked = false;
                    SimulateMouseEnter();
                }
                else
                {
                    state = BattleState.EnemyMovement;
                }

                break;
        }
    }

    private void LoadObjects()
    {
        if (player == null)
            player = GameObject.Find("Player").GetComponent<PlayerController>();

        if (board == null)
            board = GameObject.Find("Board").GetComponent<BoardController>();

        if (frontground == null)
            frontground = GameObject.Find("Frontground").GetComponent<Image>();

        if (winSoundPlayer == null)
            winSoundPlayer = GameObject.Find("Win Sound Player").GetComponent<WinSoundPlayer>();
    }

    private void LoadEnemies()
    {
        switch (level)
        {
            case 1:
                board.CreateEnemy(zombie, 0, 2);
                board.CreateEnemy(zombie, 2, 2);
                break;

            case 2:
                board.CreateEnemy(indianZombie, 0, 0);
                board.CreateEnemy(zombie, 1, 1);
                board.CreateEnemy(indianZombie, 2, 2);
                break;

            case 3:
                board.CreateEnemy(bat, 0, 1);
                board.CreateEnemy(indianZombie, 1, 2);
                board.CreateEnemy(zombie, 2, 0);
                board.CreateEnemy(bat, 2, 1);
                break;

            case 4:
                board.CreateEnemy(minerZombie, 0, 2);
                board.CreateEnemy(indianZombie, 1, 1);
                board.CreateEnemy(minerZombie, 1, 2);
                break;

            case 5:
                board.CreateEnemy(bat, 0, 1);
                board.CreateEnemy(bat, 1, 1);
                board.CreateEnemy(bat, 2, 1);
                board.CreateEnemy(scorpion, 1, 2);
                break;

            case 6:
                board.CreateEnemy(zombie, 1, 0);
                board.CreateEnemy(minerZombie, 1, 1);
                board.CreateEnemy(scorpion, 1, 2);
                break;
        }

        enemiesLoaded = true;
        isWin = false;
    }

    public void CheckWin()
    {
        if (board.enemies.Count == 0 && !isWin && !isPlayingWinSound)
        {
            isWin = true;

            if (level < 6)
            {
                level++;
                enemiesLoaded = false;
                isPlayingWinSound = true;

                audioSource.Pause();
                winSoundPlayer.PlayWinSound();
            }
            else
            {
                locked = true;
                frontground.color = new Color(0.341f, 0.384f, 0.49f, 0);
                frontground.DOFade(1, 0.5f).OnComplete(LoadYouWin);
            }
        }
    }

    private void LoadYouWin()
    {
        SceneManager.LoadScene("YouWin");
        Destroy(gameObject);
    }

    public void CheckGameOver()
    {
        if (isGameOver)
        {
            locked = true;
            frontground.DOFade(1, 0.5f).OnComplete(LoadGameOver);
        }
    }

    private void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
        Destroy(gameObject);
    }

    public void NewLevel()
    {
        isPlayingWinSound = false;
        audioSource.Play();

        SceneManager.LoadScene("Battle");
    }

    public void CheckWeaponChange()
    {
        bool weaponChanged = true;

        if (Input.GetKeyDown(KeyCode.A))
            player.equippedWeaponIndex = 0;
        else if (Input.GetKeyDown(KeyCode.S))
            player.equippedWeaponIndex = 1;
        else if (Input.GetKeyDown(KeyCode.D))
            player.equippedWeaponIndex = 2;
        else
            weaponChanged = false;

        if (weaponChanged)
        {
            Cursor.SetCursor(player.EquippedWeapon.mouseCursor, Vector2.zero, CursorMode.ForceSoftware);
            SimulateMouseEnter();
            player.EquippedWeapon.PlayTradeSound();
        }
    }

    private void SimulateMouseEnter()
    {
        Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);

        if (hit)
        {
            EnemyBase enemy = hit.collider.gameObject.GetComponent<EnemyBase>();
            enemy.MouseExitRoutine();
            enemy.MouseEnterRoutine();
        }
    }

    public void CheckShoot()
    {
        if (Input.GetMouseButtonDown(0) &&
                    !locked &&
                    aimedEnemies.Count > 0 &&
                    (player.EquippedWeapon.currentMagazine > 0 ||
                    player.EquippedWeapon.magazineSize == 0))
        {
            locked = true;
            player.Shoot();
            state = BattleState.EnemyDamage;
        }
    }

    public void AimEnemy(EnemyBase enemy)
    {
        aimedEnemies.Add(enemy);
    }

    public void NeglectEnemies()
    {
        ClearTiles();
        aimedEnemies.Clear();
    }

    public void ClearTiles()
    {
        foreach (var tile in board.tiles)
            tile.Neglect();
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
        EnemyMovement = 4,
        EnemyAttack = 5
    }
}
