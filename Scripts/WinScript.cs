using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScript : MonoBehaviour {
	public GameObject otherWin, losingPlayer;

	// Use this for initialization
	void Start () {
		this.GetComponent<SpriteRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (losingPlayer.GetComponent<PlayerScript>().GetLives() == 0 && !otherWin.GetComponent<SpriteRenderer>().enabled) {
			this.GetComponent<SpriteRenderer>().enabled = true;
		}
	}
}
