using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroid : MonoBehaviour {

    float MAX_VELOCITY;
    public float MAX_VEL_DRAG;
    float startingAngle;
    public GameObject particlePrefab;
    public bool isHooked;
    public int points = 1;
    public int secondsOfPossession;
    public Color highLightColors;
    public string asteroid_scoreable;
    public string asteroid_unscoreable;
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
        myRigidBody.velocity = new Vector3(Mathf.Cos(radians) * MAX_VELOCITY, Mathf.Sin(radians) * MAX_VELOCITY, 0);
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
            gameObject.layer = LayerMask.NameToLayer(asteroid_unscoreable);
            myRigidBody.drag = 0.0f;
        }

        // If the asteroid is going to fast, slow it down progressivly
        if ( !isHooked && myRigidBody.velocity.magnitude > MAX_VELOCITY)
        {       
            myRigidBody.drag = MAX_VEL_DRAG;
        }
    }

    public void InitializeAsteroid(float startingAsteroidVel, float startingAsteroidAngle)
    {
        MAX_VELOCITY =startingAsteroidVel;
        startingAngle = startingAsteroidAngle;
    }

    public void HookedAsteroid (Player player)
    {
        if (asteroidOwner != null)
            asteroidOwner.GetComponent<HookShootScript>().resetHook();

        isHooked = true;
        asteroidOwner = player;
        myRigidBody.mass = .0001f;
        mySpriteRenderer.color = highLightColors;
        gameObject.layer = LayerMask.NameToLayer(asteroid_scoreable);
    }

    public void ReleasedAsteroid ()
    {
        isHooked = false;
        releaseTime = Time.time;
        myRigidBody.mass = 1.0f;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "Moon" && gameObject.layer == LayerMask.NameToLayer(asteroid_scoreable))
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
                //if (asteroidOwner.player == 1) Uncomment for asteroid possesion scoring
                {
                   
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
               // if (asteroidOwner.player == 1) Uncomment for player possesion of asteroids
                {
                 
                    for (int i = 0; i < points; i++)
                    {
                        newParticles[i] = Instantiate(particlePrefab, this.transform.position, Quaternion.identity) as GameObject;
                        newParticles[i].GetComponent<Particle>().InitializeParticle(3);
                    }
                    GameManager.Instance.AddPointsForPlayer(3, points);
                }
                else if (coll.gameObject.transform.position.x < 0)
                //else if (asteroidOwner.player == 2) //Uncomment for player possesion of asteroids
                {
                    
                    for (int i = 0; i < points; i++)
                    {
                        newParticles[i] = Instantiate(particlePrefab, this.transform.position + new Vector3(i * 0.1f, 0, 0), Quaternion.identity) as GameObject;
                        newParticles[i].GetComponent<Particle>().InitializeParticle(2);
                    }
                    GameManager.Instance.AddPointsForPlayer(2, points);
                }
                else if (coll.gameObject.transform.position.x >= 0)
                // else if (asteroidOwner.player == 3) Uncomment for player possesion of asteroids
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
