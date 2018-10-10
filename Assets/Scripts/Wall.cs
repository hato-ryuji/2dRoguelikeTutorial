using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {
    /// <summary>
    /// ダメージを受けている状態の画像
    /// </summary>
    public Sprite dmgSprite;
    /// <summary>
    /// 耐久値
    /// </summary>
    public int hp = 4;

    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    /// <summary>
    /// Wallがダメージを受けた時の処理
    /// </summary>
    /// <param name="loss">ダメージの強さ（耐久度の減少数）</param>
    public void DamageWall (int loss) {
        spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        if (hp <= 0) {
            gameObject.SetActive(false);
        }
    }
}
