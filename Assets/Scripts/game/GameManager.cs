using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioSource Audio;
    private bool isGameActive = true;
    public GameObject Overlay;

    public GameObject Player;
    public GameObject BlackHolePrefab;
    public GameObject AsteroidPrefab;
    public GameObject BulletPrefab;
    public List<GameObject> BlackHoles;
    public Text TimeDisplay;
    public GameObject ExplosionPrefab;

    public RectTransform GameOverUI;
    public Text GameOverTime;

    private float timeElapsed = 0.0f;
    private int secondsElapsed = 0;
    private int minutesElapsed = 0;
    private int AsteroidSpawnRate = 5;
    private float AsteroidSpeedModifier = 0.0f;
    private float AsteroidSizeModifier = 0.0f;
    private int numberOfAsteroidsToSpawn = 1;

    public string TitleScene;

	// Use this for initialization
	void Start ()
    {
        BlackHoles = new List<GameObject>();

        Overlay.transform.localScale = (Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 1.0f)) * 2);

        Initialization();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (IsGameActive())
        {
            UpdateTime();
            TimeDisplay.text = GetTime();
        }
	}

    public void Initialization()
    {
        TimeDisplay.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0.0f,
                                                                                        Screen.height * 0.45f,
                                                                                        0.0f);



        SpawnBlackHole();
        SpawnObjects();
        ScaleGameOverUI();
    }

    private void ScaleGameOverUI()
    {
        GameOverUI.transform.localPosition = new Vector3(0.0f, (Screen.height * 0.1f), 1.0f);
    }

    void SpawnBlackHole()
    {
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.2f, 0.8f),
                                                                        Random.Range(0.2f, 0.8f),
                                                                        10.0f));


        BlackHoles.Add(GameObject.Instantiate(BlackHolePrefab, pos, Quaternion.identity));
        BlackHoles[BlackHoles.Count - 1].GetComponent<BlackHole>().Initialize(this, Player, 0.5f, 2.0f);
    }

    void SpawnAsteroid(Vector2 pos, Vector2 velocity, float size)
    {
        GameObject temp = GameObject.Instantiate(AsteroidPrefab, pos, Quaternion.identity);
        temp.GetComponent<Asteroid>().Initialize(this, velocity);
        temp.transform.localScale = new Vector3(size, size, 1.0f);
    }

    public void SpawnBullet(Vector2 pos, Vector2 dir, float speed)
    {
        Vector2 spawnPos = pos;
        spawnPos += (dir * (BulletPrefab.transform.localScale.y / 2));

        GameObject temp = GameObject.Instantiate(BulletPrefab, 
                                                spawnPos, 
                                                Quaternion.FromToRotation(Vector2.up, dir));

        temp.GetComponent<Bullet>().Initialize(dir * speed);
    }

    public void SpawnExplosion(Vector2 pos)
    {
        GameObject temp = GameObject.Instantiate(ExplosionPrefab, pos, Quaternion.identity);
    }

    private void UpdateTime()
    {
        timeElapsed += Time.deltaTime;

        if ((timeElapsed - secondsElapsed) > 1.0f)
        {
            secondsElapsed++;

            SpawnObjects();
            IncreaseDifficulty();

            if (secondsElapsed >= (60 * (minutesElapsed + 1)))
            {
                minutesElapsed++;
            }
        } 
    }

    private string GetTime()
    {
        string result = "";

        int activeSeconds = (secondsElapsed - (60 * minutesElapsed));

        string displaySeconds = "";
        string displayMinutes = "";

        if (activeSeconds < 10)
        {
            displaySeconds = "0" + activeSeconds.ToString();
        }

        else
        {
            displaySeconds = activeSeconds.ToString();
        }

        if (minutesElapsed < 10)
        {
            displayMinutes = "0" + minutesElapsed.ToString();
        }

        else
        {
            displayMinutes = minutesElapsed.ToString();
        }

        result = displayMinutes + ":" + displaySeconds;

        return result;
    }

    private void SpawnObjects()
    {
        Vector3 SpawnPos;
        int spawnLocation;

        for (int i = 0; i < numberOfAsteroidsToSpawn; i++)
        {
            float spawnSize = Random.Range(0.3f, 0.8f) + AsteroidSizeModifier;

            if ((secondsElapsed % AsteroidSpawnRate) == 0)
            {
                spawnLocation = Random.Range(0, 4);

                switch (spawnLocation)
                {
                    case 0: //left
                        SpawnPos = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, Random.Range(0.0f, 1.0f), 0.0f));
                        SpawnAsteroid(SpawnPos, new Vector2((Random.Range(0.0f, 1.5f) + AsteroidSpeedModifier), Random.Range(-0.5f, 0.5f)), spawnSize);
                        break;

                    case 1: //right
                        SpawnPos = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, Random.Range(0.0f, 1.0f), 0.0f));
                        SpawnAsteroid(SpawnPos, new Vector2((Random.Range(-1.5f, 0.0f) - AsteroidSpeedModifier), Random.Range(-0.5f, 0.5f)), spawnSize);
                        break;

                    case 2: //up
                        SpawnPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.0f, 1.0f), 0.0f, 0.0f));
                        SpawnAsteroid(SpawnPos, new Vector2(Random.Range(-0.5f, 0.5f), (Random.Range(0.0f, 1.5f) + AsteroidSpeedModifier)), spawnSize);
                        break;

                    case 3: //down
                        SpawnPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.0f, 1.0f), 1.0f, 0.0f));
                        SpawnAsteroid(SpawnPos, new Vector2(Random.Range(-0.5f, 0.5f), (Random.Range(-1.5f, 0.0f) - AsteroidSpeedModifier)), spawnSize);
                        break;
                }
            }
        }
    }

    private void IncreaseDifficulty()
    {
        if (secondsElapsed % 120 == 0)
        {
            if (AsteroidSpawnRate > 1 && secondsElapsed != 0)
            {
                AsteroidSpawnRate--;
            }
        }

        if((secondsElapsed + 60) % 120  == 0)
        {
            numberOfAsteroidsToSpawn++;
        }

        if(secondsElapsed % 120 == 0)
        {
            SpawnBlackHole();
        }

        AsteroidSpeedModifier = Mathf.Min((AsteroidSpeedModifier + 0.001f), 0.5f);
        AsteroidSizeModifier = Mathf.Min((AsteroidSizeModifier + 0.002f), 1.0f);
    }

    public bool IsGameActive()
    {
        return isGameActive;
    }

    public void GameOver()
    {
        isGameActive = false;
        Overlay.SetActive(true);
        GameOverUI.gameObject.SetActive(true);
        TimeDisplay.enabled = false;
        GameOverTime.text = GetTime();
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene(TitleScene);
    }
}
