using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public string left, right, up, down;
	public float speed, jumpForce, gravity;

	public GameObject otherNinja, stage;
	public StarScript star;

	Rigidbody2D rb;
	BoxCollider2D coll;
	Transform tf;

	Collider2D otherColl, stageColl;

	bool isJumping = false;
	bool isHolding = true;
	float dx, dy;

	// double-jump variables
	bool isDoubleJumping = false;
	protected float doubleJump_cooldown = 0.5f;
	protected int jump_count = 0;

	// double-dash variables
	protected float ButtonCooler = 0.5f;
	protected int ButtonCount = 0;
	int direction;
	int magnitude;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody2D>();
		coll = this.GetComponent<BoxCollider2D>();
		tf = this.transform;

		otherColl = otherNinja.GetComponent<Collider2D>();
		stageColl = stage.GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		MoveAndCollide();
		Jump();
		Throw();
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
		// determine direction and speed based on keys pressed
		if (Input.GetKeyDown (left)) {
			direction = -1;
			magnitude = 1;
			if (ButtonCooler > 0 && ButtonCount == 1 && !isJumping) {
				magnitude = 3;
			} else {
				ButtonCooler = 0.5f;
				ButtonCount += 1;
			}
		} else if (Input.GetKeyDown (right)) {
			direction = 1;
			magnitude = 1;
			if (ButtonCooler > 0 && ButtonCount == 1 && !isJumping) {
				magnitude = 3;
			} else {
				ButtonCooler = 0.5f;
				ButtonCount += 1;
			}
		} else if (!Input.GetKey (left) && !Input.GetKey (right)) {
			direction = 0;
			magnitude = 0;
		}

		// calculate delay for double-dash
		if (ButtonCooler > 0) {
			ButtonCooler -= Time.deltaTime;
		} else {
			ButtonCount = 0;
		}
			
		Move (magnitude * direction);

		if (Input.GetKeyDown (up) && !isJumping) {
			isJumping = true;
			dy = jumpForce;
		} else if (Input.GetKeyDown (up) && !isDoubleJumping) {
			isDoubleJumping = true;
			dy = 2f * jumpForce;
		} 
		tf.position += new Vector3(dx, 0, 0);
	}

	void Jump() {
		if (isJumping || isDoubleJumping) {
			tf.position += new Vector3(0, dy, 0);
			dy -= gravity;
			if ((coll.IsTouching(stageColl) || coll.IsTouching(otherColl)) && dy < 0) {
				isJumping = false;
				isDoubleJumping = false;
				dy = 0;
			}
		}
	}

	void Throw() {
		if (Input.GetKey(down) && isHolding) {
			isHolding = false;
			star.Throw();
		}
	}
}
