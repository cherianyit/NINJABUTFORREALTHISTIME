using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public string left, right, up, down;
	public float speed, jumpForce, gravity, knockback, knockbackSpeed;

	public GameObject otherNinja, stage;
	public StarScript star;

	BoxCollider2D coll;
	Transform tf;

	Collider2D otherColl, stageColl;

	bool isJumping = false, isKnockedBack = false, knockbackRight;
	int timesKnockedBack = 0;

	float dx, dy;

	// Use this for initialization
	void Start () {
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

		if (isKnockedBack) {
			dx += timesKnockedBack * knockbackSpeed * (knockbackRight ? -1 : 1);
			if (dx * (knockbackRight ? -1 : 1) > 0) {
				dx = 0;
				isKnockedBack = false;
			}
		}
	}

	void MoveAndCollide() {
		if (!isKnockedBack) {
			if (Input.GetKey(left)) {
				this.transform.localScale = new Vector3(-6, 6, 1);
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
				this.transform.localScale = new Vector3(6, 6, 1);
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
			   || (coll.IsTouching(otherColl)))) {
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
		knockbackRight = (direction == 1);
		dx = timesKnockedBack * direction * knockback;
		isKnockedBack = true;
	}
}
