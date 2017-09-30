using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starScript : MonoBehaviour {
    bool isFlying = true;
    bool hitplayer = false;
    private Rigidbody2D rbody;
    //private Collision2D coll;
    public float Hr;
    public float Vr;
    public float velx;
    public string eStar;
    // Use this for initialization
    void Start()
    {

        rbody = this.GetComponent<Rigidbody2D>();
        //coll = GetComponent<Collision2D>();
        rbody.GetComponent<Rigidbody2D>().gravityScale = 0;

    }

    // Update is called once per frame
    void Update()
    {
        //OnCollisionEnter2D(coll);
        if (isFlying)
        {
            
            movement(velx, 0);
        }
        else
        {
            
            movement(-velx, 0);
        }


    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == eStar)
        {
            Hr = rbody.velocity.x;
            isFlying = false;
            
            //Vr = 1; 


        }

        if (coll.gameObject.name == "stage")
        {
            isFlying = false;
            Hr = 0;
            //Vr = 0;
        }
    }

    void movement(float horizontal, float vertical)
    {
        rbody.velocity = new Vector2(horizontal, vertical);
    }
}
