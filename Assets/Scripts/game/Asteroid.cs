using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public GameManager gameManager;
    private Vector3 currentVelocity;
    private float PullSpeed = 0.5f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (gameManager.IsGameActive())
        {
            for (int i = 0; i < gameManager.BlackHoles.Count; i++)
            {
                BlackHole script = gameManager.BlackHoles[i].GetComponent<BlackHole>();
                Vector2 vec = (transform.localPosition - gameManager.BlackHoles[i].transform.localPosition);

                if (vec.magnitude < (script.currentSize / 2))
                {
                    Feed(script);
                }

                else if (vec.magnitude < script.currentPullSize)
                {
                    if (script.isActive)
                    {
                        Gravitate(gameManager.BlackHoles[i].transform.position, script.currentSize, script.currentPullSize);
                    }
                }
            }

            UpdatePosition();
        }
	}

    public void Initialize(GameManager manager, Vector2 velocity)
    {
        gameManager = manager;
        currentVelocity = new Vector3(velocity.x, velocity.y, 0.0f);
    }

    private void UpdatePosition()
    {
        transform.position += (currentVelocity * Time.deltaTime);

        if (currentVelocity.magnitude > 0.2f)
        {
            currentVelocity /= 1 + (0.95f * Time.deltaTime);
        }
    }

    private void Gravitate(Vector3 other, float size, float pullSize)
    {
        float distance = (transform.position - other).magnitude;
        float velocity = (((pullSize - distance) * (pullSize - distance) * (pullSize - distance)) / (pullSize - size)) * PullSpeed;
        Vector3 direction = (other - transform.position).normalized;

        currentVelocity += (velocity * Time.deltaTime) * direction;
    }

    private void Feed(BlackHole hole)
    {
        hole.Grow(transform.localScale.x, true);
        Destroy(this.gameObject);
    }

    public void Break()
    {
        gameManager.SpawnExplosion(gameObject.transform.localPosition);
        gameManager.Audio.Play();

        if (transform.localScale.x > 0.2f)
        {
            transform.localScale = new Vector3((transform.localScale.x - 0.1f), (transform.localScale.y - 0.1f), 1.0f);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Vector3 vector = (transform.localPosition - col.transform.localPosition);
        float mag = currentVelocity.magnitude * col.transform.localScale.x * 1.0f;

        currentVelocity += (vector * mag);
    }
}
