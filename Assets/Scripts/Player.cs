﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MovingObjct {

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDeray = 1f;
    public Text foodText;

    private Animator animator;
    private int food;

	// Use this for initialization
	protected override void Start () {
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoint;

        base.Start();
	}

    private void OnDisable() {
        GameManager.instance.playerFoodPoint = food;
    }

    // Update is called once per frame
    void Update() {
        if (!GameManager.instance.playerTurn) {
            return;
        }
        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0) {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0) {
            AttempMove<Wall>(horizontal, vertical);
        }
    }

    protected override void AttempMove<T>(int xDir, int yDir) {
        food--;
        foodText.text = "Food: " + food;

        base.AttempMove<T>(xDir, yDir);
        RaycastHit2D hit;
        CheckIfGameOver();
        GameManager.instance.playerTurn = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Exit") {
            Invoke("Restart", restartLevelDeray);
            enabled = false;
        }
        else if (other.tag == "Food") {
            food += pointsPerFood;
            foodText.text = "+ " + pointsPerFood + "Food: " + food;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda") {
            food += pointsPerSoda;
            foodText.text = "+ " + pointsPerSoda + "Food: " + food;
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCanMove<T>(T component) {
        Debug.Log("OnCanMove 開始");
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
        Debug.Log("OnCanMove　終了");
    }

    protected void Restart() {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LoseFood(int loss) {
        animator.SetTrigger("playerHit");
        food -= loss;
        foodText.text = "- " + loss + "Food: " + food;
        CheckIfGameOver();
    }

    private void CheckIfGameOver() {
        if (food <= 0) {
            GameManager.instance.GameOver();
        }
    }
}
