using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObjct {

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDeray = 1f;

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

        horizontal = (int)Input.GetAxis("Horizontal");
        vertical = (int)Input.GetAxis("Vertical");

        if (horizontal != 0) {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0) {
            AttempMove<Wall>(horizontal, vertical);
        }
    }

    protected override void AttempMove<T>(int xDir, int yDir) {
        food--;
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
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda") {
            food += pointsPerSoda;
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCanMove<T>(T component) {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
    }

    protected void Restart() {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LoseFood(int loss) {
        animator.SetTrigger("playerHit");
        food -= loss;
        CheckIfGameOver();
    }

    private void CheckIfGameOver() {
        if (food <= 0) {
            GameManager.instance.GameOver();
        }
    }
}
