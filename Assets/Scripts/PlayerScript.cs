using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public string left, right, up, down;
	public float speed;

	public GameObject otherNinja;

	float dx;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(left)) {
			this.transform.localScale = new Vector3(-6, 6, 1);
			if (this.GetComponent<Collider2D>().IsTouching(otherNinja.GetComponent<Collider2D>()) 
				&& otherNinja.transform.position.x < this.transform.position.x) {
				dx = 0;
			} else {
				dx = -speed;
			}
		}
		if (Input.GetKey(right)) {
			this.transform.localScale = new Vector3(6, 6, 1);
			if (this.GetComponent<Collider2D>().IsTouching(otherNinja.GetComponent<Collider2D>()) 
				&& otherNinja.transform.position.x > this.transform.position.x) {
				dx = 0;
			} else {
				dx = speed;
			}
		}
		if (Input.GetKey(left) == Input.GetKey(right)) {
			dx = 0;
		}
		this.transform.position += new Vector3(dx, 0, 0);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject == otherNinja) {
			Debug.Log("test");
		}
	}
}
