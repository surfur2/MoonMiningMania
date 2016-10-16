using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private static GameManager instance = null;
    public static GameManager Instance { get { return instance; } }

    public int totalNumberOfAsteroids;
    public int totalNumberOfGoldenAsteroids;
    public int timeBetweenAsteroidSpawns;
    public GameObject asteroidPrefab;
    public GameObject asteroidGoldPrefab;
    public int maxSpeedOfAsteroids;
    public int scoreToWin = 10;
    public Text[] playerTextScores;
    public GameObject gameOverPanel;
    public AudioClip scoreSound;

    private int[] playerScores;
    private float minSpawnLocationAsteroid = .1f;
    private float maxSpawnLocationAsteroid = .9f;
    private float asteroidSpawnDeadZone = 120.0f;
    private float asteroidSpawnBuffer = 10.0f;
    private int asteroids;
    private int asteroidsGold;
    private float lastSpawnTime;
    private AudioSource sound;
   
	// Use this for initialization
	void Start () {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
        }
        gameOverPanel.SetActive(false);
        lastSpawnTime = Time.time;
        asteroids = 0;
        asteroidsGold = 0;
        int players = gameObject.GetComponentsInChildren<Player>().Length;
        playerScores = new int[players];
        for (int i = 0; i < players; i++)
        {
            playerScores[i] = 0;
            playerTextScores[i].gameObject.SetActive(true);
            playerTextScores[i].text = "Player " + (i + 1) + " Score: 0";
        }

        sound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

        if (lastSpawnTime + timeBetweenAsteroidSpawns < Time.time && asteroids != totalNumberOfAsteroids)
        {
            SpawnAsteroid(false);
            lastSpawnTime = Time.time;
        }
        if (lastSpawnTime + timeBetweenAsteroidSpawns < Time.time && asteroidsGold != totalNumberOfGoldenAsteroids)
        {
            SpawnAsteroid(true);
            lastSpawnTime = Time.time;
        }
    }

    void SpawnAsteroid(bool isGolden)
    {
        float a, b, c, d;
        int side = Random.Range(1, 5);
        float startingAngle;
        Vector3 spawnLocation;

        // Spawn on bottom
        if (side == 1)
        {
            // Four boundaries for the two spawn zones
            a = asteroidSpawnBuffer;
            b = (180 - asteroidSpawnDeadZone) / 2;
            c = (asteroidSpawnDeadZone + b);
            d = (180 - asteroidSpawnBuffer);

            spawnLocation = new Vector3(Random.Range(minSpawnLocationAsteroid, maxSpawnLocationAsteroid), 0, 0);
        }
        // Spawn on left side
        else if (side == 2)
        {
            // Four boundaries for the two spawn zones
            a = -90 + asteroidSpawnBuffer;
            b = -90 + (180 - asteroidSpawnDeadZone) / 2;
            c = (asteroidSpawnDeadZone + b);
            d = (90 - asteroidSpawnBuffer);

            spawnLocation = new Vector3(0, Random.Range(minSpawnLocationAsteroid, maxSpawnLocationAsteroid), 0);
        }
        // Spawn on Top
        else if (side == 3)
        {
            // Four boundaries for the two spawn zones
            a = 180 + asteroidSpawnBuffer;
            b = 180 + (180 - asteroidSpawnDeadZone) / 2;
            c = (asteroidSpawnDeadZone + b);
            d = (360 - asteroidSpawnBuffer);

            spawnLocation = new Vector3(Random.Range(minSpawnLocationAsteroid, maxSpawnLocationAsteroid), 1.0f, 0);
        }
        // Spawn on Right side
        else
        {
            // Four boundaries for the two spawn zones
            a = 90 + asteroidSpawnBuffer;
            b = 90 + (180 - asteroidSpawnDeadZone) / 2;
            c = (asteroidSpawnDeadZone + b);
            d = (270 - asteroidSpawnBuffer);

            spawnLocation = new Vector3(1.0f, Random.Range(minSpawnLocationAsteroid, maxSpawnLocationAsteroid), 0);
        }

        int direction = Random.Range(1, 3);
        if (direction == 1)
        {
            // Spawn shooting right
            startingAngle = Random.Range(a, b);
        }
        else
        {
            // Spawn shooting left
            startingAngle = Random.Range(c, d);
        }

        spawnLocation = Camera.main.ViewportToWorldPoint(spawnLocation);
        spawnLocation.z = 0;
        
        if (!isGolden)
        {
            GameObject newAsteroid = (GameObject)Instantiate(asteroidPrefab, spawnLocation, Quaternion.Euler(0, 180, 0));
            newAsteroid.GetComponent<Asteroid>().InitializeAsteroid(maxSpeedOfAsteroids, startingAngle);
            asteroids++;
        }
        else
        {
            GameObject newAsteroidGold = (GameObject)Instantiate(asteroidGoldPrefab, spawnLocation, Quaternion.Euler(0, 180, 0));
            newAsteroidGold.GetComponent<Asteroid>().InitializeAsteroid(maxSpeedOfAsteroids, startingAngle);
            asteroidsGold++;
        }
    }

    public void AddPointsForPlayer(int player, int score)
    {
        int indexForPlayer = player - 1;
        playerScores[indexForPlayer] += score;
        playerTextScores[indexForPlayer].text = "Player " + player + " Score: " + playerScores[indexForPlayer];

        asteroids--;

        if (playerScores[player - 1] >= scoreToWin)
        {
            GameOver(player);
        }

        sound.PlayOneShot(scoreSound);
    }

    void GameOver(int player)
    {
        Time.timeScale = 0.0f;
        Text gameOverText = gameOverPanel.GetComponentInChildren<Text>();

        gameOverText.text = "Player" + player + "'s Side of Earth Prospers Thanks to Their Great Effort: \n";
        for (int i = 0; i < playerScores.Length; i++)
            gameOverText.text += playerTextScores[i].text + "\n";

        gameOverPanel.SetActive(true);
    }
}
