using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public GameObject startScreen;
    public GameObject earthGraphic;
    public GameObject playerPrefab;
    public Sprite[] shipSprites;
    public Sprite[] worldSprites;
    public string sceneName;
    [HideInInspector]
    public int players;


    private int[] playerScores;
    private float minSpawnLocationAsteroid = .1f;
    private float maxSpawnLocationAsteroid = .9f;
    private float asteroidSpawnDeadZone = 120.0f;
    private float asteroidSpawnBuffer = 10.0f;
    private int asteroids;
    private int asteroidsGold;
    private float lastSpawnTime;
    private AudioSource sound;

    private bool gameStarted = false;
   
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

        Time.timeScale = 1.0f;

        sound = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {

        if (!gameStarted)
        {
            //HANDLE START SCREEN
            if (Input.GetKeyDown(KeyCode.Alpha2)) StartGame(2);
            else if (Input.GetKeyDown(KeyCode.Alpha3)) StartGame(3);
        }
        else
        {
            //HANDLE ASTEROIDS
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

    void StartGame(int numPlayers)
    {
        startScreen.SetActive(false);
        gameStarted = true;

        makeShip(1, numPlayers);
        makeShip(2, numPlayers);
        if (numPlayers > 2) makeShip(3, numPlayers);

        if (numPlayers == 2) earthGraphic.GetComponent<SpriteRenderer>().sprite = worldSprites[0];
        else if (numPlayers == 3) earthGraphic.GetComponent<SpriteRenderer>().sprite = worldSprites[1];

        players = numPlayers;
        playerScores = new int[players];
        for (int i = 0; i < players; i++)
        {
            playerScores[i] = 0;
            playerTextScores[i].gameObject.SetActive(true);
            playerTextScores[i].text = "Player " + (i + 1) + " Score: 0";
        }


        //Set up anything else important for the change
    }

    void makeShip(int playerNumber, int maxPlayers)
    {
        //Decide starting position
        Vector3 playerPosition;
        if(maxPlayers == 2)
        {
            if (playerNumber == 1) playerPosition = new Vector3(-2.5f, 0, 0);
            else playerPosition = new Vector3(2.5f, 0, 0);
        }
        else
        {
            if (playerNumber == 1) playerPosition = new Vector3(-2, 2, 0);
            else if (playerNumber == 2) playerPosition = new Vector3(0, -2.4f, 0);
            else playerPosition = new Vector3(2, 2, 0);
        }

        //Make ship
        GameObject newPlayer = (GameObject)Instantiate(playerPrefab, playerPosition, Quaternion.identity);

        //Set ship by playerNumber
        newPlayer.GetComponent<Player>().playerString = "_P" + playerNumber;
        newPlayer.GetComponent<HookShootScript>().playerString = "_P" + playerNumber;
        newPlayer.GetComponent<SpriteRenderer>().sprite = shipSprites[playerNumber - 1];

    }

    void GameOver(int player)
    {
        Time.timeScale = 0.0f;
        Text gameOverText = gameOverPanel.GetComponentInChildren<Text>();

        gameOverText.text = "Player" + player + "'s Side of Earth Prospers Thanks to Their Great Effort: \n";
        for (int i = 0; i < playerScores.Length; i++)
            gameOverText.text += playerTextScores[i].text + "\n";

        gameOverPanel.SetActive(true);

        Destroy(instance);

        SceneManager.LoadScene(sceneName);
    }
}
