using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    Vector3 playerDirection;
    Vector3 playerVelocity;
    static Vector3 Max_Velocity;
    private Rigidbody2D rgb2d; // Used for moving the charcter

    //Start is called at the beginning
    void Start()
    {
        rgb2d = GetComponent<Rigidbody2D>();
        playerDirection = new Vector3(0.0f, 1.0f, 0.0f);
        playerVelocity = new Vector3(0.0f, 0.0f, 0.0f);
        Max_Velocity = new Vector3(30.0f, 30.0f, 0.0f);
    }

    //FixedUpdate is called for each Physics step
    void FixedUpdate()
    {
        float rotateDirection = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 force = playerDirection * 2;

            if((playerVelocity + force).sqrMagnitude <= Max_Velocity.sqrMagnitude)
            {
                playerVelocity += force;
                rgb2d.AddForce(force);
            }  

        }
    }
}
