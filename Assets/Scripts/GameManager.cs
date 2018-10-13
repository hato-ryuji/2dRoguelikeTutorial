﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲーム管理用クラス
/// Singletonpaパターン採用。
/// 
/// </summary>
public class GameManager : MonoBehaviour {
    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    /// <summary>
    /// インスタンス（Singletonパターン）
    /// </summary>
    public static GameManager instance = null;

    public BoardManager boardScript;

    public int playerFoodPoint = 100;
    [HideInInspector] public bool playerTurn = true;

    private Text levelText;
    private GameObject levelImage;
    private int level = 1;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;

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

    private void OnLevelWasLoaded(int index) {
        level++;
        InitGame();
    }

    /// <summary>
    /// ゲーム初期化処理
    /// </summary>
    void InitGame() {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);
        enemies.Clear();
        boardScript.SetupScene(level);
    }

    /// <summary>
    /// レベルイメージを隠す
    /// </summary>
    private void HideLevelImage () {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    /// <summary>
    /// ゲームオーバー時の制御
    /// </summary>
    public void GameOver() {
        levelText.text = "After " + level + " days, you starved.";
        levelImage.SetActive(true);
        enabled = false;
    }

    void Update() {
        if (playerTurn || enemiesMoving || doingSetup) {
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
