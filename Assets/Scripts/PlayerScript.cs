using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public string left, right, up, down;
	public float speed, jumpForce, gravity, knockback, knockbackSpeed, dashWindow, dashMultiplier;

	public GameObject otherNinja, stage, enemyStar;
	public PlayerScript otherNinjaScript;
	public StarScript star;
    public bool isJumping = false;

    public bool ninjaDied = false;

	BoxCollider2D coll;
	Transform tf;
	Rigidbody2D rb;

	Collider2D otherColl, stageColl, enemyStarColl;

<<<<<<< HEAD
	bool isJumping = false, isKnockedBack = false, isRunning = false, wasJustWalking = false, wasJustRunning = false;
	bool knockbackRight, lastMoveRight;
=======
	bool  isKnockedBack = false, knockbackRight;
>>>>>>> origin/proper-use-of-branches
	int timesKnockedBack = 0;
	int lives = 3;
	int framesSinceLetGo = 0;
	int framesOfLastWalk = 0;

	float dx, dy;
	Vector3 initialPosition;
	Quaternion lastRotation;

	// Use this for initialization
	void Start () {
		coll = this.GetComponent<BoxCollider2D>();
		tf = this.transform;
		rb = this.GetComponent<Rigidbody2D>();

		initialPosition = this.transform.position;

		otherColl = otherNinja.GetComponent<Collider2D>();
		stageColl = stage.GetComponent<Collider2D>();
		enemyStarColl = enemyStar.GetComponent<Collider2D>();
		lastRotation = transform.rotation;
	}

	// Update is called once per frame
	void Update () {
		MoveAndCollide();
		Jump();
		Throw();

		if (isKnockedBack) {
			dx += timesKnockedBack * knockbackSpeed * (knockbackRight ? -1 : 1);
			if (dx * (knockbackRight ? -1 : 1) > 0) {
				dx = 0;
				isKnockedBack = false;
			}
		}

		Death();

		if (coll.IsTouching(stageColl)) {
			this.transform.rotation = stage.transform.rotation;
		} else {
			transform.rotation = lastRotation;
		}

		lastRotation = transform.rotation;
	}

	public void Death(){
		if ((tf.position.y < -9 || tf.position.x< -10 || tf.position.x > 10) && (lives > 0)) {
			lives--;
			timesKnockedBack = 0;
			tf.position = (initialPosition + otherNinjaScript.GetInitialPosition()) / 2;
			ninjaDied = true;
			if (lives <= 0) {
				this.GetComponent<SpriteRenderer>().enabled = false;
				transform.position = new Vector3(100, 100, 100);
			}
		}
	}

	void MoveAndCollide() {
		if (!isKnockedBack && lives > 0) {
			if (Input.GetKey(left)) {
				this.transform.localScale = new Vector3(-Mathf.Abs(this.transform.localScale.x), 
					this.transform.localScale.y, this.transform.localScale.z);
				if (coll.IsTouching(otherColl) && otherNinja.transform.position.x < tf.position.x
				   && Mathf.Abs(otherNinja.transform.position.y - tf.position.y) < tf.localScale.y * coll.size.y) {
					dx = 0;
					if (!coll.IsTouching(stageColl)) {
						isJumping = true;
					}
				} else {
					if (!lastMoveRight && !wasJustRunning && (isRunning || 
						(framesSinceLetGo > 0 && framesOfLastWalk <= dashWindow && framesSinceLetGo <= dashWindow))) {
						isRunning = true;
						dx = -speed * dashMultiplier;
					} else {
						dx = -speed;
						wasJustRunning = false;
						if (!wasJustWalking) {
							framesOfLastWalk = 0;
						}
						wasJustWalking = true;
						framesOfLastWalk++;
					}
					framesSinceLetGo = 0;
					lastMoveRight = false;
				}
			}
			if (Input.GetKey(right)) {
				this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), 
					this.transform.localScale.y, this.transform.localScale.z);
				if (coll.IsTouching(otherColl) && otherNinja.transform.position.x > tf.position.x
				   && Mathf.Abs(otherNinja.transform.position.y - tf.position.y) < tf.localScale.y * coll.size.y) {
					dx = 0;
					if (!coll.IsTouching(stageColl)) {
						isJumping = true;
					}
				} else {
					if (lastMoveRight && !wasJustRunning && (isRunning || 
						(framesSinceLetGo > 0 && framesOfLastWalk <= dashWindow && framesSinceLetGo <= dashWindow))) {
						isRunning = true;
						dx = speed * dashMultiplier;
					} else {
						dx = speed;
						wasJustRunning = false;
						if (!wasJustWalking) {
							framesOfLastWalk = 0;
						}
						wasJustWalking = true;
						framesOfLastWalk++;
					}
					framesSinceLetGo = 0;
					lastMoveRight = true;
				}
			}
			if (Input.GetKey(left) == Input.GetKey(right)) {
				dx = 0;
			}
			if (Input.GetKey(up) && !isJumping && (coll.IsTouching(stageColl)
				|| coll.IsTouching(otherColl) || coll.IsTouching(enemyStarColl))) {
				isJumping = true;
				dy = jumpForce;
			}
			if (!(Input.GetKey(left) || Input.GetKey(right))) {
				if (isRunning) {
					wasJustRunning = true;
				}
				isRunning = false;
				wasJustWalking = false;
				framesSinceLetGo++;
			}
		}
		tf.position += new Vector3(dx, 0, 0);
	}

	void Jump() {
		if (isJumping) {
			tf.position += new Vector3(0, dy, 0);
			dy -= gravity;
			if ((coll.IsTouching(stageColl) || coll.IsTouching(otherColl)) && dy < 0) {
				isJumping = false;
				dy = 0;
			}
		}
	}

	void Throw() {
		if (Input.GetKey(down) && star.IsHeld()) {
			star.Throw();
		}
	}

	public void KnockBack(int direction) {
		timesKnockedBack++;
		if (timesKnockedBack >= 6) {
			timesKnockedBack = 5;
		}
		knockbackRight = (direction == 1);
		dx = timesKnockedBack * direction * knockback;
		isKnockedBack = true;
	}

	public int GetLives() {
		return lives;
	}

	public Vector3 GetInitialPosition() {
		return initialPosition;
	}
}
