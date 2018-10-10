using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    /// <summary>
    /// インスタンス（Singletonパターン）
    /// </summary>
    public static GameManager instance = null;

    public BoardManager boardScript;

    private int level = 3;

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

        boardScript = GetComponent<BoardManager>();
        InitGame();
	}

    /// <summary>
    /// ゲーム初期化処理
    /// </summary>
    void InitGame() {
        boardScript.SetupSence(level);
    }
}
