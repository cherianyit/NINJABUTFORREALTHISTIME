using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuckwildPlayerScript : MonoBehaviour {

	public string left, right, up, down;
	public float speed, jumpForce, gravity;

	public GameObject otherNinja, stage;
	public BuckwildPlayerScript otherNinjaScript;

	public bool ninjaDied = false;

	BoxCollider2D coll;
	Transform tf;

	Collider2D otherColl, stageColl;

	bool isJumping = false;
	int lives = 3;

	float dx, dy;
	Vector3 initialPosition;
	Quaternion lastRotation;

	// Use this for initialization
	void Start () {
		coll = this.GetComponent<BoxCollider2D>();
		tf = this.transform;

		initialPosition = this.transform.position;

		otherColl = otherNinja.GetComponent<Collider2D>();
		stageColl = stage.GetComponent<Collider2D>();
		lastRotation = transform.rotation;
	}

	// Update is called once per frame
	void Update () {
		MoveAndCollide();
		Jump();

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
			tf.position = (initialPosition + otherNinjaScript.GetInitialPosition()) / 2;
			ninjaDied = true;
			if (lives <= 0) {
				this.GetComponent<SpriteRenderer>().enabled = false;
				transform.position = new Vector3(100, 100, 100);
			}
		}
	}

	void MoveAndCollide() {
		if (lives > 0) {
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
					dx = -speed;
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
					dx = speed;
				}
			}
			if (Input.GetKey(left) == Input.GetKey(right)) {
				dx = 0;
			}
			if (Input.GetKey(up) && !isJumping && (coll.IsTouching(stageColl)
				|| coll.IsTouching(otherColl))) {
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

	public int GetLives() {
		return lives;
	}

	public Vector3 GetInitialPosition() {
		return initialPosition;
	}
}
