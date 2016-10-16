using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    float asteroidVelocity;
    float startingAngle;
    public GameObject particlePrefab;
    public float points = 1.0f;
    public bool isHooked;

    Rigidbody2D myRigidBody;

    // Use this for initialization
    void Start()
    {
        myRigidBody = this.GetComponent<Rigidbody2D>();
        float radians = (Mathf.PI * startingAngle / 180);
        myRigidBody.velocity = new Vector3(Mathf.Cos(radians) * asteroidVelocity, Mathf.Sin(radians) * asteroidVelocity, 0);
        isHooked = false;
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void InitializeAsteroid(float startingAsteroidVel, float startingAsteroidAngle)
    {
        asteroidVelocity =startingAsteroidVel;
        startingAngle = startingAsteroidAngle;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "Moon")
        {
            Destroy(gameObject, 0f);

            GameObject newParticle = Instantiate(particlePrefab, this.transform.position, Quaternion.identity) as GameObject;

            if (coll.gameObject.transform.position.x > 0)
            {
                GameManager.Instance.AddPointsForPlayer(1, (int)points);
                newParticle.GetComponent<Particle>().InitializeParticle(1);
            }
            else
            {
                GameManager.Instance.AddPointsForPlayer(2, (int)points);
                newParticle.GetComponent<Particle>().InitializeParticle(2);
            }
        }
    }
}
