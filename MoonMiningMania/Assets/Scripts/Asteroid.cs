using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    public float asteroidVelocity;
    public float startingAngle;

    Rigidbody2D myRigidBody;

    // Use this for initialization
    void Start()
    {
        float radians = (Mathf.PI * startingAngle / 180);
        myRigidBody = this.GetComponent<Rigidbody2D>();
        myRigidBody.velocity = new Vector3(Mathf.Cos(radians) * asteroidVelocity, (Mathf.Sin(radians) * asteroidVelocity), 0);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
