using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroid : MonoBehaviour {

    float asteroidVelocity;
    float startingAngle;
    public GameObject particlePrefab;
    public bool isHooked;
    public int points = 1;
    private GameObject[] newParticles;

    Rigidbody2D myRigidBody;

    // Use this for initialization
    void Start()
    {
        myRigidBody = this.GetComponent<Rigidbody2D>();
        float radians = (Mathf.PI * startingAngle / 180);
        myRigidBody.velocity = new Vector3(Mathf.Cos(radians) * asteroidVelocity, Mathf.Sin(radians) * asteroidVelocity, 0);
        isHooked = false;
        newParticles = new GameObject[points];
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

            if (points == 3)
            {
                GameManager.Instance.asteroidsGold--;
                GameManager.Instance.lastDestroyedTimeGold = Time.time;
            }


            GameObject newParticle = Instantiate(particlePrefab, this.transform.position, Quaternion.identity) as GameObject;

            if (GameManager.Instance.players == 2)
                if (coll.gameObject.transform.position.x > 0)
                {
                    for(int i = 0; i < points; i++)
                    {
                        newParticles[i] = Instantiate(particlePrefab, this.transform.position, Quaternion.identity) as GameObject;
                        newParticles[i].GetComponent<Particle>().InitializeParticle(1);
                    }
                    GameManager.Instance.AddPointsForPlayer(1, points);
                }
                else
                {
                    for (int i = 0; i < points; i++)
                    {
                        newParticles[i] = Instantiate(particlePrefab, this.transform.position + new Vector3(i*0.1f, 0, 0), Quaternion.identity) as GameObject;
                        newParticles[i].GetComponent<Particle>().InitializeParticle(2);
                    }
                    GameManager.Instance.AddPointsForPlayer(2, points);
                }
            else if (GameManager.Instance.players == 3)
            {
                if (coll.gameObject.transform.position.y <= -.87)
                {
                    for (int i = 0; i < points; i++)
                    {
                        newParticles[i] = Instantiate(particlePrefab, this.transform.position, Quaternion.identity) as GameObject;
                        newParticles[i].GetComponent<Particle>().InitializeParticle(3);
                    }
                    GameManager.Instance.AddPointsForPlayer(3, points);
                }
                else if (coll.gameObject.transform.position.x < 0)
                {
                    for (int i = 0; i < points; i++)
                    {
                        newParticles[i] = Instantiate(particlePrefab, this.transform.position + new Vector3(i * 0.1f, 0, 0), Quaternion.identity) as GameObject;
                        newParticles[i].GetComponent<Particle>().InitializeParticle(2);
                    }
                    GameManager.Instance.AddPointsForPlayer(2, points);
                }
                else if (coll.gameObject.transform.position.x >= 0)
                {
                    for (int i = 0; i < points; i++)
                    {
                        newParticles[i] = Instantiate(particlePrefab, this.transform.position + new Vector3(i * 0.1f, 0, 0), Quaternion.identity) as GameObject;
                        newParticles[i].GetComponent<Particle>().InitializeParticle(1);
                    }
                    GameManager.Instance.AddPointsForPlayer(1, points);
                }
            }
        }
    }
}
