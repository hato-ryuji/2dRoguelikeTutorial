using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームのロード
/// 作成したScriptの中で初めに実行される。
/// </summary>
public class Loader : MonoBehaviour {

    public GameObject gameManager;

	// Use this for initialization
	void Awake () {
        if (GameManager.instance == null) {
            Instantiate(gameManager);
        }
	}
}
