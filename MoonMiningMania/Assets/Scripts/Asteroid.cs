using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroid : MonoBehaviour {

    float asteroidVelocity;
    float startingAngle;
    public GameObject particlePrefab;
    public bool isHooked;
    public int points = 1;
    public int secondsOfPossession;
    public Color[] playerColors;
    private GameObject[] newParticles;
    private Color originalColor;
    private Player asteroidOwner;

    [HideInInspector]
    public float releaseTime;
    Rigidbody2D myRigidBody;
    SpriteRenderer mySpriteRenderer;

    // Use this for initialization
    void Start()
    {
        myRigidBody = this.GetComponent<Rigidbody2D>();
        mySpriteRenderer = this.GetComponent<SpriteRenderer>();
        float radians = (Mathf.PI * startingAngle / 180);
        myRigidBody.velocity = new Vector3(Mathf.Cos(radians) * asteroidVelocity, Mathf.Sin(radians) * asteroidVelocity, 0);
        isHooked = false;
        newParticles = new GameObject[points];
        asteroidOwner = null;
        originalColor = mySpriteRenderer.color;
    }

    // Update is called once per frame
    void Update () {
	    if (!isHooked && (releaseTime + secondsOfPossession <= Time.time))
        {
            asteroidOwner = null;
            mySpriteRenderer.color = originalColor;
        }

        /*if ( !isHooked && myRigidBody.velocity.magnitude > asteroidVelocity)
        {
            myRigidBody.velocity = myRigidBody.velocity - ((-1) * myRigidBody.velocity.normalized * .5f * Time.deltaTime);
        }*/
    }

    public void InitializeAsteroid(float startingAsteroidVel, float startingAsteroidAngle)
    {
        asteroidVelocity =startingAsteroidVel;
        startingAngle = startingAsteroidAngle;
    }

    public void HookedAsteroid (Player player)
    {
        if (asteroidOwner != null)
            asteroidOwner.GetComponent<HookShootScript>().resetHook();

        isHooked = true;
        asteroidOwner = player;
        myRigidBody.mass = .0001f;
        mySpriteRenderer.color = playerColors[player.player - 1];
        asteroidOwner = player;
    }

    public void ReleasedAsteroid ()
    {
        isHooked = false;
        releaseTime = Time.time;
        myRigidBody.mass = 1.0f;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "Moon")
        {
            GameObject newParticle = Instantiate(particlePrefab, this.transform.position, Quaternion.identity) as GameObject;

            if (GameManager.Instance.players == 2)
                if (coll.gameObject.transform.position.x > 0)
                //if (asteroidOwner.player == 1) Uncomment for asteroid possesion scoring
                {
                    Destroy(gameObject, 0f);

                    if (points == 3)
                    {
                        GameManager.Instance.asteroidsGold--;
                        GameManager.Instance.lastDestroyedTimeGold = Time.time;
                    }

                    for (int i = 0; i < points; i++)
                    {
                        newParticles[i] = Instantiate(particlePrefab, this.transform.position, Quaternion.identity) as GameObject;
                        newParticles[i].GetComponent<Particle>().InitializeParticle(1);
                    }
                    GameManager.Instance.AddPointsForPlayer(1, points);
                }
                //else if (asteroidOwner.player == 2) Uncomment for asteroid possesion scoring
                else
                {
                    Destroy(gameObject, 0f);

                    if (points == 3)
                    {
                        GameManager.Instance.asteroidsGold--;
                        GameManager.Instance.lastDestroyedTimeGold = Time.time;
                    }


                    for (int i = 0; i < points; i++)
                    {
                        newParticles[i] = Instantiate(particlePrefab, this.transform.position + new Vector3(i*0.1f, 0, 0), Quaternion.identity) as GameObject;
                        newParticles[i].GetComponent<Particle>().InitializeParticle(2);
                    }
                    GameManager.Instance.AddPointsForPlayer(2, points);
                }
            else if (GameManager.Instance.players == 3)
            {
                //if (coll.gameObject.transform.position.y <= -.87)
                if (asteroidOwner.player == 1)
                {
                    for (int i = 0; i < points; i++)
                    {
                        newParticles[i] = Instantiate(particlePrefab, this.transform.position, Quaternion.identity) as GameObject;
                        newParticles[i].GetComponent<Particle>().InitializeParticle(3);
                    }
                    GameManager.Instance.AddPointsForPlayer(3, points);
                }
                //else if (coll.gameObject.transform.position.x < 0)
                else if (asteroidOwner.player == 2)
                {
                    for (int i = 0; i < points; i++)
                    {
                        newParticles[i] = Instantiate(particlePrefab, this.transform.position + new Vector3(i * 0.1f, 0, 0), Quaternion.identity) as GameObject;
                        newParticles[i].GetComponent<Particle>().InitializeParticle(2);
                    }
                    GameManager.Instance.AddPointsForPlayer(2, points);
                }
                //else if (coll.gameObject.transform.position.x >= 0)
                else if (asteroidOwner.player == 3)
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
