using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    Vector3 playerDirection;
    public float Max_Speed = 5.0f;
    public float acceleration;
    public string playerString;
    private Rigidbody2D rgb2d; // Used for moving the charcter

    //Start is called at the beginning
    void Start()
    {
        rgb2d = GetComponent<Rigidbody2D>();
        playerDirection = new Vector3(0.0f, 1.0f, 0.0f);
    }

    void Update()
    {
        if (Input.GetAxisRaw("Horizontal" + playerString) == 1)
        {
            transform.Rotate(new Vector3(0, 0, -5));
        }
        if (Input.GetAxisRaw("Horizontal" + playerString) == -1)
        {
            transform.Rotate(new Vector3(0, 0, +5));
        }
        if (Input.GetAxisRaw("Vertical" + playerString) == 1)
        {
            if (rgb2d.velocity.magnitude <= Max_Speed)
                rgb2d.velocity += new Vector2(transform.up.x * Time.deltaTime * acceleration, transform.up.y * Time.deltaTime * acceleration);
            else
            {
                Vector2 newVelocity = rgb2d.velocity + new Vector2(transform.up.x * Time.deltaTime * acceleration, transform.up.y * Time.deltaTime * acceleration);
                rgb2d.velocity = newVelocity.normalized * Max_Speed;
            }
        }
    }
}
