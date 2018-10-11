using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public float turnDelay = .1f;
    /// <summary>
    /// インスタンス（Singletonパターン）
    /// </summary>
    public static GameManager instance = null;

    public BoardManager boardScript;

    public int playerFoodPoint = 100;
    [HideInInspector] public bool playerTurn = true;


    private int level = 3;
    private List<Enemy> enemies;
    private bool enemiesMoving;

	// Use this for initialization
	void Awake () {

        //Singletonパターン
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        InitGame();
	}

    /// <summary>
    /// ゲーム初期化処理
    /// </summary>
    void InitGame() {
        enemies.Clear();
        boardScript.SetupSence(level);
    }

    public void GameOver() {
        enabled = false;
    }

    void Update() {
        if (playerTurn || enemiesMoving) {
            return;
        }
        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script) {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies() {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0) {
            yield return new WaitForSeconds(turnDelay);
        }
        foreach (var enemy in enemies) {
            enemy.MoveEnemy();
            yield return new WaitForSeconds(turnDelay);
        }
        playerTurn = true;
        enemiesMoving = false;
    }
}
