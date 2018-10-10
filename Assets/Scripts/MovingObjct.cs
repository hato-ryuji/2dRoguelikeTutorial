using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObjct : MonoBehaviour {

    /// <summary>
    /// 1回の移動にかかる時間（秒）
    /// </summary>
    public float moveTime = 0.1f;
    public LayerMask blockingLayer;


    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    /// <summary>
    /// 移動時間の逆数
    /// </summary>
    private float inverseMoveTime;


	// Use this for initialization
	protected virtual void Start () {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1 / moveTime;
	}

    /// <summary>
    /// ゲームオブジェクト移動処理
    /// </summary>
    /// <param name="xDir">x軸の移動量</param>
    /// <param name="yDir">y軸の移動量</param>
    /// <param name="hit">接触したゲームオブジェクト（移動先にすでにゲームオブジェクトがいた場合）</param>
    /// <returns>
    ///     true:移動が行われた
    ///     false:移動を行わなかった
    /// </returns>
    protected bool Move(int xDir, int yDir, out RaycastHit2D hit) {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform  == null) {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }
	
    /// <summary>
    /// キャラクターを滑らかに移動させる
    /// </summary>
    /// <param name="end">移動目標座標</param>
    /// <returns></returns>
    protected IEnumerator SmoothMovement(Vector3 end) {
        /* 
         * 現在値の差分のベクトルの二乗を取得
         * 現在座標と終了座標が同じ場所になれば０になると思われる。
         */
        float sqrRemainningDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainningDistance > float.Epsilon) {
            //この辺は移動処理だと思われる
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainningDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    protected virtual void AttempMove<T>(int xDir, int yDir)
        where T : Component {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null) {
            return;
        }

        T hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null) {
            OnCanMove(hitComponent);
        }
    }

    protected abstract void OnCanMove<T>(T component)
        where T : Component;
}
