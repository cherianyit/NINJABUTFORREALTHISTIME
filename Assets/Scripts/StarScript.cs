using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour {

	public float speed;

	public GameObject owner, stage, enemy, otherStar;
	public PlayerScript ownerScript, enemyScript;

	Collider2D coll, ownerColl, enemyColl, stageColl;

	bool isHeld = true, isThrown = false;
	int direction;
	float dx, dy;

	Vector3 offset, throwVector;

	// Use this for initialization
	void Start () {
		float radius = this.transform.localScale.x * this.GetComponent<CircleCollider2D>().radius;
		float ownerWidth = Mathf.Abs(owner.transform.localScale.x) * owner.GetComponent<BoxCollider2D>().size.x;
		offset = new Vector3(ownerWidth / 2.0f + radius, 0, 0);
		throwVector = new Vector3(speed, 0, 0);

		coll = this.GetComponent<Collider2D>();
		ownerColl = owner.GetComponent<Collider2D>();
		enemyColl = enemy.GetComponent<Collider2D>();
		stageColl = stage.GetComponent<Collider2D>();
	}

	// Update is called once per frame
	void Update () {
		if (isHeld) {
			direction = (int)Mathf.Sign(owner.transform.localScale.x);
			this.GetComponent<Collider2D>().enabled = false;
			this.GetComponent<SpriteRenderer>().enabled = false;
			this.transform.position = owner.transform.position + (offset * direction);
		} else if (isThrown) {
			this.GetComponent<Collider2D>().enabled = true;
			this.GetComponent<SpriteRenderer>().enabled = true;
			this.transform.position += (throwVector * direction);
			if (coll.IsTouching(enemyColl)) {
				// enemy.KnockBack();
				isThrown = false;
				dy = -0.01f;
			}
		} else /*if (!coll.IsTouching(stageColl))*/ {
			transform.position += new Vector3(0, dy, 0);
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		
	}

	public void Throw() {
		isHeld = false;
		isThrown = true;
	}
}
