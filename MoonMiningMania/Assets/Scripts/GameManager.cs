using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public int totalNumberOfAsteroids;
    public int timeBetweenAsteroidSpawns;
    public GameObject asteroidPrefab;
    public int maxSpeedOfAsteroids;

    private float minSpawnLocationAsteroid = .2f;
    private float maxSpawnLocationAsteroid = .8f;
    private float spreadOfAsteroidAngle = 30.0f;
    private List<GameObject> asteroids;
    private float lastSpawnTime;
   
	// Use this for initialization
	void Start () {
        lastSpawnTime = Time.time;
        asteroids = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {

        if (lastSpawnTime + timeBetweenAsteroidSpawns < Time.time && asteroids.Count != totalNumberOfAsteroids)
        {
            SpawnAsteroid();
            lastSpawnTime = Time.time;
        }
	}

    void SpawnAsteroid()
    {
        int side = Random.Range(1, 5);
        float startingAngle;
        Vector3 spawnLocation;
        // Spawn on bottom
        if (side == 1)
        {
            spawnLocation = new Vector3(Random.Range(minSpawnLocationAsteroid, maxSpawnLocationAsteroid), -.1f, 0);
            startingAngle = Random.Range(90 - (spreadOfAsteroidAngle/2), 90 + (spreadOfAsteroidAngle / 2));
        }
        // Spawn on left
        else if (side == 2)
        {
            spawnLocation = new Vector3(-.1f, Random.Range(minSpawnLocationAsteroid, maxSpawnLocationAsteroid), 0);
            startingAngle = Random.Range(-(spreadOfAsteroidAngle / 2), (spreadOfAsteroidAngle / 2));
        }
        // Spawn on Top
        else if (side == 3)
        {
            spawnLocation = new Vector3(Random.Range(minSpawnLocationAsteroid, maxSpawnLocationAsteroid), 1.1f, 0);
            startingAngle = Random.Range(270 - (spreadOfAsteroidAngle / 2), 270 + (spreadOfAsteroidAngle / 2));

        }
        // Spawn on Right
        else
        {
            spawnLocation = new Vector3(1.1f, Random.Range(minSpawnLocationAsteroid, maxSpawnLocationAsteroid), 0);
            startingAngle = Random.Range(180 - (spreadOfAsteroidAngle / 2), 180 + (spreadOfAsteroidAngle / 2));
        }
        spawnLocation = Camera.main.ViewportToWorldPoint(spawnLocation);
        spawnLocation.z = 0;
        GameObject newAsteroid = (GameObject)Instantiate(asteroidPrefab, spawnLocation, Quaternion.Euler(0, 180, 0));
        newAsteroid.GetComponent<Asteroid>().InitializeAsteroid(maxSpeedOfAsteroids, startingAngle);
        asteroids.Add(newAsteroid);
    }
}
