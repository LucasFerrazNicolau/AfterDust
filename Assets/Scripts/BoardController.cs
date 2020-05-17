using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    // Attributes
    private int width;
    private int height;
    private float startSize;
    private float sizeOffset;
    private float sizeCorrection;
    private float[,] xPositions;
    private float[] yPositions;

    // Objects
    public Tile[,] tiles;
    public List<EnemyBase> enemies;

    private void Awake()
    {
        InitializeAttributes();
        CreateTiles();

        enemies = new List<EnemyBase>();
    }

    private void InitializeAttributes()
    {
        width = 3;
        height = 3;

        startSize = 1;
        sizeOffset = 0.2f;

        xPositions = new float[3, 3];
        xPositions[0, 0] = -155;
        xPositions[0, 1] = -120;
        xPositions[0, 2] = -95;
        xPositions[1, 0] = -10;
        xPositions[1, 1] = -5;
        xPositions[1, 2] = 0;
        xPositions[2, 0] = 145;
        xPositions[2, 1] = 115;
        xPositions[2, 2] = 90;

        yPositions = new float[3];
        yPositions[0] = -40;
        yPositions[1] = 40;
        yPositions[2] = 115;

        sizeCorrection = Mathf.Sqrt(3);
    }

    private void CreateTiles()
    {
        tiles = new Tile[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float size = startSize - (sizeOffset * (j - 1));

                tiles[i, j] = new Tile
                {
                    indexX = i,
                    indexY = j,
                    x = xPositions[i, j],
                    y = yPositions[j],
                    z = j,
                    size = size
                };
            }
        }
    }

    public EnemyBase CreateEnemy(GameObject enemyPrefab, int x, int y)
    {
        GameObject enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
        enemy.transform.parent = transform;

        EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
        enemies.Add(enemyScript);
        enemyScript.board = this;
        MoveTo(enemyScript, x, y);

        return enemyScript;
    }

    public void MoveTo(EnemyBase enemy, int x, int y)
    {
        Tile destiny = tiles[x, y];

        if (destiny.enemy == null)
        {
            // Exit from previous tile
            Tile origin = enemy.currentTile;
            if (origin != null)
                origin.enemy = null;

            // Move to new tile
            enemy.transform.position = new Vector3(destiny.x, destiny.y, destiny.z) + transform.position;
            enemy.transform.localScale = Vector3.one * destiny.size * transform.localScale.magnitude / sizeCorrection;
            enemy.currentTile = destiny;
            destiny.enemy = enemy;
        }
        else if (enemy.currentTile.indexX != x && enemy.currentTile.indexY != y)
        {
            Debug.LogError("Enemy " + enemy + " moving to occupied tile!");
        }
    }
}
