using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuckwildStageScript : MonoBehaviour {

	Quaternion lastRotation;

	// Use this for initialization
	void Start () {
		lastRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs(transform.rotation.z) > 0.3) {
			transform.rotation = lastRotation;
		}
		lastRotation = transform.rotation;
	}
}
