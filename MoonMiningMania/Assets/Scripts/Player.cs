using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    Vector3 playerDirection;
    public float Max_Speed = 5.0f;
    public float moveSpeed = 0.0f;
    private Rigidbody2D rgb2d; // Used for moving the charcter

    //Start is called at the beginning
    void Start()
    {
        rgb2d = GetComponent<Rigidbody2D>();
        playerDirection = new Vector3(0.0f, 1.0f, 0.0f);
    }

    //FixedUpdate is called for each Physics step
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerDirection = Quaternion.Euler(0, 0, -5) * playerDirection;
            playerDirection.Normalize();
            transform.Rotate(new Vector3(0, 0, -5));
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerDirection = Quaternion.Euler(0, 0, +5) * playerDirection;
            playerDirection.Normalize();
            transform.Rotate(new Vector3(0, 0, +5));

        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 force = playerDirection * 2;

            if ((force + new Vector3(rgb2d.velocity.x, rgb2d.velocity.y, 0.0f)).magnitude <= Max_Speed)
            {
                rgb2d.AddForce(force);
                moveSpeed = rgb2d.velocity.magnitude;
            }
            else
            {
                rgb2d.AddForce(force);
                rgb2d.AddForce(playerDirection * -2);
            }
        }
    }
}
