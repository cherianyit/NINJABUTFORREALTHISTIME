using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public string left, right, up, down;
	public float speed, jumpForce, gravity, knockback, knockbackSpeed;

	public GameObject otherNinja, stage;
	public PlayerScript otherNinjaScript;
	public StarScript star;

	Rigidbody2D rb;
	BoxCollider2D coll;
	Transform tf;
	Collider2D otherColl, stageColl;

	bool isJumping = false, isKnockedBack = false, knockbackRight;
	int timesKnockedBack = 0;
	int lives = 3;

	float dx, dy;
	Vector3 initialPosition;

	// double press manager
	protected float ButtonCooler = 0.5f;
	protected int ButtonCount = 0;
	int direction;
	int magnitude;

	// Use this for initialization
	void Start () {
		magnitude = 0;
		direction = 0;

		rb = this.GetComponent<Rigidbody2D>();
		coll = this.GetComponent<BoxCollider2D>();
		tf = this.transform;

		initialPosition = this.transform.position;

		otherColl = otherNinja.GetComponent<Collider2D>();
		stageColl = stage.GetComponent<Collider2D>();
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

		Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
		if (pos.y <= 0.0 || 1.0 <= pos.y) {
			this.transform.position = (initialPosition + otherNinjaScript.GetInitialPosition()) / 2;
			lives--;
		}
	}


	void Move(int xDirection) {
		this.transform.localScale = new Vector3(Mathf.Sign(xDirection) * 6, 6, 1);
		if (coll.IsTouching(otherColl) && otherNinja.transform.position.x > tf.position.x 
			&& Mathf.Abs(otherNinja.transform.position.y - tf.position.y) < tf.localScale.y * coll.size.y) {
			dx = 0;
			if (!coll.IsTouching(stageColl)) {
				isJumping = true;
			}
		} else {
			dx = xDirection * speed;
		}
	}

	void MoveAndCollide() {
		if (!isKnockedBack) {
			if (Input.GetKeyDown (left)) {
				direction = -1;
				magnitude = 1;
				if (ButtonCooler > 0 && ButtonCount == 1) {
					magnitude = 3;
				} else {
					ButtonCooler = 0.5f;
					ButtonCount += 1;
				}
			} else if (Input.GetKeyDown (right)) {
				direction = 1;
				magnitude = 1;
				if (ButtonCooler > 0 && ButtonCount == 1) {
					magnitude = 3;
				} else {
					ButtonCooler = 0.5f;
					ButtonCount += 1;
				}
			} else if (!Input.GetKey (left) && !Input.GetKey (right)) {
				direction = 0;
				magnitude = 0;
			}

			if (ButtonCooler > 0) {
				ButtonCooler -= Time.deltaTime;
			} else {
				ButtonCount = 0;
			}

			Move (magnitude * direction);

			if (Input.GetKey(up) && !isJumping) {
				isJumping = true;
				dy = jumpForce;
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
