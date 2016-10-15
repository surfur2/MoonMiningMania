using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    Vector3 playerDirection;
    public float Max_Speed = 30.0f;
    private Rigidbody2D rgb2d; // Used for moving the charcter

    //Start is called at the beginning
    void Start()
    {
        rgb2d = GetComponent<Rigidbody2D>();
        playerDirection = new Vector3(0.0f, 1.0f, 0.0f);
    }

    //Update is called every frame
    void FixedUpdate()
    {       
        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerDirection = Quaternion.Euler(0, 0, -5) * playerDirection;
            playerDirection.Normalize();
            transform.Rotate(new Vector3(0, 0, -5));
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerDirection = Quaternion.Euler(0, 0, +5) * playerDirection;
            playerDirection.Normalize();
            transform.Rotate(new Vector3(0, 0, +5));

        }


        if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 force = playerDirection * 2;

            if((force + new Vector3(rgb2d.velocity.x, rgb2d.velocity.y, 0.0f)).magnitude <= Max_Speed)
            {
                rgb2d.AddForce(force);
            }
            else
            {
                rgb2d.AddForce(force);
                rgb2d.AddForce(playerDirection * -2);
            }

        }
    }
}
