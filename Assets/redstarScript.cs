using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class redstarScript : MonoBehaviour {   
    bool isFlying = true;
    private Rigidbody2D rbody;
    //private Collision2D coll;
    public float Hr;
    public float Vr;
    // Use this for initialization
    void Start () {
        rbody = GetComponent<Rigidbody2D>();
        //coll = GetComponent<Collision2D>();
        rbody.GetComponent<Rigidbody2D>().gravityScale = 0;
        
    }
	
	// Update is called once per frame
	void Update () {
        //OnCollisionEnter2D(coll);
        if (isFlying)
        {
            movement(-5, 0);
        }
        else
        {
            movement(0, -3);
        }
            
     
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "blueStar")
        {
            isFlying = false;
            Hr = 1;
            //Vr = 1; 


        }

        if (coll.gameObject.name == "stage")
        {
            isFlying = false;
           // Hr = 0;
            //Vr = 0;
        }
    }

    void movement(float horizontal, float vertical)
    {
        rbody.velocity = new Vector2(horizontal, vertical);
    }


}
