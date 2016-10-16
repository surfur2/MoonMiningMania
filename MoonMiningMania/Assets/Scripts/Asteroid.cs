using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroid : MonoBehaviour {

    float asteroidVelocity;
    float startingAngle;
    public GameObject particlePrefab;
    private GameObject[] particles;
    public Sprite goldSprite;
    public int points = 1;

    Rigidbody2D myRigidBody;

    // Use this for initialization
    void Start()
    {
        myRigidBody = this.GetComponent<Rigidbody2D>();
        float radians = (Mathf.PI * startingAngle / 180);
        myRigidBody.velocity = new Vector3(Mathf.Cos(radians) * asteroidVelocity, Mathf.Sin(radians) * asteroidVelocity, 0);
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void InitializeAsteroid(float startingAsteroidVel, float startingAsteroidAngle, bool isGolden = false)
    {
        asteroidVelocity =startingAsteroidVel;
        startingAngle = startingAsteroidAngle;
        if (isGolden)
        {
            GetComponent<SpriteRenderer>().sprite = goldSprite;
            points = 3;
        }
        particles = new GameObject[points];
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "Moon")
        {
            Destroy(gameObject, 0f);

            if (coll.gameObject.transform.position.x > 0)
            {
                GameManager.Instance.AddPointsForPlayer(1, points);
                for (int i = 0; i < points; i++)
                {
                    particles[i] = Instantiate(particlePrefab, this.transform.position, Quaternion.identity) as GameObject;
                    particles[i].GetComponent<Particle>().InitializeParticle(1);
                }
            }
            else
            {
                GameManager.Instance.AddPointsForPlayer(2, points);
                for (int i = 0; i < points; i++)
                {
                    particles[i] = Instantiate(particlePrefab, this.transform.position, Quaternion.identity) as GameObject;
                    particles[i].GetComponent<Particle>().InitializeParticle(2);
                }
            }
        }
    }
}
