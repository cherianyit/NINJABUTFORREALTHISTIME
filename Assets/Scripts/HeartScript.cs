﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartScript : MonoBehaviour {
	public PlayerScript player;
	public int heartNumber;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (player.GetLives() >= heartNumber) {
			this.GetComponent<SpriteRenderer>().enabled = true;
		} else {
			this.GetComponent<SpriteRenderer>().enabled = false;
		}
	}
}