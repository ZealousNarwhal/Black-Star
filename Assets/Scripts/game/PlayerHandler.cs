using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public GameManager gameManager;

    public AudioSource Audio;
    public AudioClip LaserClip;
    public AudioClip ExplosionClip;

    public SpriteRenderer sprite;

    public float Speed = 0.5f;
    public float SpeedCap = 0.5f;
    public float SpeedDecaySpeed = 2.0f;

    public float BulletSpeed = 5.0f;

    Vector2 currentVelocity;

    private bool isImploding = false;
    private bool isExploding = false;

    private float size;

    private float explosionTime = 1.0f;
    private float currentExplosionTime = 0.0f;

	// Use this for initialization
	void Start ()
    {
        size = transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (gameManager.IsGameActive())
        {
            if (isImploding)
            {
                Imploding();
            }

            else if (isExploding)
            {
                Exploding();
            }

            else
            {
                UpdateRotation();
            }

            UpdateVelocity();
            UpdatePosition();

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Fire();
            }
        }
	}

    private void UpdateRotation()
    {
        Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetVector = (targetPos - playerPos);

        float angle = (Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg - 90);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void UpdateVelocity()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (!isImploding && !isExploding)
            {
                Vector2 forwardVec = transform.TransformDirection(Vector3.up).normalized;
                forwardVec *= (Speed * Time.deltaTime);

                currentVelocity = new Vector2(Mathf.Min((currentVelocity.x + forwardVec.x), SpeedCap),
                                              Mathf.Min((currentVelocity.y + forwardVec.y), SpeedCap));
            }
        }

        else
        {
            currentVelocity = currentVelocity / (1 + (SpeedDecaySpeed * Time.deltaTime));
        }
    }

    public void AddVelocity(Vector2 velocity)
    {
        currentVelocity += velocity;
    }

    private void UpdatePosition()
    {
        transform.position += new Vector3(currentVelocity.x, currentVelocity.y, 0.0f);
    }

    private void Fire()
    {
        if (sprite.enabled)
        {
            Vector2 direction = transform.TransformDirection(Vector2.up).normalized;

            gameManager.SpawnBullet((new Vector2(transform.localPosition.x, transform.localPosition.y) + (direction * (transform.localScale.y / 2))),
                                    direction,
                                    BulletSpeed);

            Audio.clip = LaserClip;
            Audio.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Asteroid")
        {
            Explode();
            gameManager.SpawnExplosion((transform.localPosition + col.transform.localPosition) / 2);
            sprite.enabled = false;
            Audio.clip = ExplosionClip;
            Audio.Play();
        }
    }

    private void Explode()
    {
        isExploding = true;
    }

    public void Implode()
    {
        isImploding = true;
    }

    private void Exploding()
    {
        if(currentExplosionTime < explosionTime)
        {
            currentExplosionTime += Time.deltaTime;
        }

        else
        {
            gameManager.GameOver();
        }
    }

    private void Imploding()
    {
        if(transform.localScale.x > 0)
        {
            float newSize = Mathf.Max(transform.localScale.x - (size * (Time.deltaTime)), 0.0f);
            transform.localScale = new Vector3(newSize, newSize, 1.0f);
        }

        else
        {
            gameManager.GameOver();
        }
    }
}
