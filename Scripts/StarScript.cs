using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour {

	public float speed, fallSpeed, force;

	public GameObject owner, stage, enemy, otherStar, camera;
	public PlayerScript enemyScript;

	Collider2D coll, ownerColl, enemyColl, stageColl;

	Rigidbody2D rb;

	bool isHeld = true, isThrown = false, isFalling = false, alreadyThrown = false, alreadyThrown45 = false;
    public bool isJumped = false;
	int direction;
	float dx, dy;

	Vector3 offset, throwVector, lastPos;
	Vector2 zero;

	// Use this for initialization
	void Start () {
		float radius = this.transform.localScale.x * this.GetComponent<CircleCollider2D>().radius;
		float ownerWidth = Mathf.Abs(owner.transform.localScale.x) * owner.GetComponent<BoxCollider2D>().size.x;
		offset = new Vector3(ownerWidth / 2.0f + radius, 0, 0);
		throwVector = new Vector3(speed, 0, 0);
		rb = this.GetComponent<Rigidbody2D>();
		zero = new Vector2(0, 0);

		coll = this.GetComponent<Collider2D>();
		ownerColl = owner.GetComponent<Collider2D>();
		enemyColl = enemy.GetComponent<Collider2D>();
		stageColl = stage.GetComponent<Collider2D>();
	}

	// Update is called once per frame
	void Update () {
        if (owner.GetComponent<PlayerScript>().isJumping && isThrown)
        {
            isJumped = true;
        }
        else
       {
            isJumped = false;
        }

         if (coll.IsTouching(stageColl))
        {
            isJumped = false;
            isThrown = false;
        }

        if (owner.GetComponent<PlayerScript>().ninjaDied){
			isHeld = true;
			owner.GetComponent<PlayerScript>().ninjaDied = false;
			lastPos = owner.GetComponent<PlayerScript>().GetInitialPosition() + (offset * direction);
		}

		if (isHeld) {
			direction = (int)Mathf.Sign(owner.transform.localScale.x);
			this.GetComponent<Collider2D>().enabled = false;
			this.GetComponent<SpriteRenderer>().enabled = false;
			this.transform.position = owner.transform.position + (offset * direction);
		} 
		else if (isThrown && (!isJumped || alreadyThrown) && !alreadyThrown45) {
			this.GetComponent<Collider2D>().enabled = true;
			this.GetComponent<SpriteRenderer>().enabled = true;
			this.transform.position += (throwVector * direction);
			alreadyThrown = true;

			Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
			if (pos.x <= 0.0 || 1.0 <= pos.x) {
				direction *= -1;
				this.transform.position += (throwVector * direction);
			}
		}
		else if (isThrown && (isJumped || alreadyThrown45) && !alreadyThrown)
        {

            this.GetComponent<Collider2D>().enabled = true;
            this.GetComponent<SpriteRenderer>().enabled = true;
            this.transform.position += new Vector3(direction * speed * (float)0.71, -speed * (float)0.71, 0);
			alreadyThrown45 = true;

            Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
            if (pos.x <= 0.0 || 1.0 <= pos.x)
            {
                direction *= -1;
                this.transform.position += (new Vector3(direction * speed * (float)0.71, -speed * (float)0.71, 0) * direction);
            }
        }


        else if (isFalling) {
			this.transform.position += new Vector3(0, dy, 0);
			if (coll.IsTouching(stageColl)) {
				isFalling = false;
                isJumped = false;
				dy = 0;
			}
		} else {
			transform.position = lastPos;
			if (lastPos.y < transform.position.y && !(coll.IsTouching(stageColl))) {
				transform.position = new Vector3(transform.position.x, lastPos.y, transform.position.z);
			}
		}

		if (!isHeld && coll.IsTouching(ownerColl)) {
			Retrieve();
		}
		
		lastPos = transform.position;
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject == enemy && isThrown) {
			isThrown = false;
            
			isFalling = true;
			dy = -fallSpeed;
			enemyScript.KnockBack(direction);
		}
		if (col.gameObject == otherStar) {
			direction *= -1;
		}
        
	}

	public void Throw() {
		isHeld = false;
		isThrown = true;
	}

	public bool IsHeld() {
		return isHeld;
	}

	public void Retrieve() {
		alreadyThrown = false;
		alreadyThrown45 = false;
		isHeld = true;
		isThrown = false;
        isJumped = false;
		isFalling = false;
		this.GetComponent<SpriteRenderer>().enabled = false;
		lastPos = owner.GetComponent<PlayerScript>().transform.position + (offset * direction);
		transform.position = lastPos;
	}
}
