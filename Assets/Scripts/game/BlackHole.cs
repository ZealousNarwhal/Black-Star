using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject Center;
    public GameObject PullRadius;
    public float currentSize;
    public float currentPullSize;
    public float GrowthRate = 0.2f;
    public float CenterSize;
    public float PullSize;
    public float PullStrength = 0.1f;

    private GameObject playerObj;
    private PlayerHandler playerHandler;

    public bool isActive = false;

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (gameManager.IsGameActive())
        {
            UpdateGraphics();

            float distance = (transform.position - playerObj.transform.position).magnitude;

            if (!isActive)
            {
                Form();
            }

            else
            {
                if (distance < currentSize)
                {
                    playerHandler.Implode();
                }

                if (distance < (currentPullSize / 2))
                {
                    Pull(distance);
                }
            }
        }
	}

    public void Initialize(GameManager manager, GameObject player, float activeSize, float activePullSize)
    {
        gameManager = manager;
        playerObj = player;
        playerHandler = player.transform.GetComponent<PlayerHandler>();

        CenterSize = activeSize;
        PullSize = activePullSize;

        currentSize = 0.0f;
        currentPullSize = 0.0f;

        Center.transform.localScale = new Vector3(currentSize, currentSize, 1.0f);
        PullRadius.transform.localScale = new Vector3(currentPullSize, currentPullSize, 1.0f);

        isActive = false;
    }

    void UpdateGraphics()
    {
        PullRadius.transform.Rotate(0.0f, 0.0f, -1.0f);
    }

    void Pull(float distance)
    {
        float velocity = ((((currentPullSize - distance) * (currentPullSize - distance)) / (currentPullSize - currentSize)) * PullStrength);
        Vector2 direction = (transform.position - playerObj.transform.position).normalized;

        playerHandler.AddVelocity((velocity * Time.deltaTime) * direction);
    }

    public void Grow(float size, bool growCenter)
    {
        if (growCenter)
        {
            currentSize += (size * 0.5f);
            Center.transform.localScale = new Vector3(currentSize, currentSize, 0.0f);
        }

        currentPullSize += (size * 1.0f);
        PullRadius.transform.localScale = new Vector3(currentPullSize, currentPullSize, 0.0f);
    }

    private void Form()
    {
        float growthThisFrame = GrowthRate * Time.deltaTime;

        currentPullSize = Mathf.Min(((currentPullSize + growthThisFrame) * (1 + growthThisFrame)), PullSize);

        if ((PullSize - CenterSize) < currentPullSize)
        {
            currentSize = Mathf.Min(((currentSize + growthThisFrame) * (1 + growthThisFrame)), CenterSize);
        }

        Center.transform.localScale = new Vector3(currentSize, currentSize, 1.0f);
        PullRadius.transform.localScale = new Vector3(currentPullSize, currentPullSize, 1.0f);

        if(Center.transform.localScale.x == CenterSize)
        {
            isActive = true;
        }
    }
}
