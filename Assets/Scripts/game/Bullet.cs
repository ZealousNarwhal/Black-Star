using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Sprite sprite;
    Vector2 currentVelocity;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdatePosition();
        CheckBounds();
	}

    public void Initialize(Vector2 velocity)
    {
        currentVelocity = velocity;
    }

    private void UpdatePosition()
    {
        transform.localPosition += (new Vector3(currentVelocity.x, currentVelocity.y, 0.0f) * Time.deltaTime);
    }

    private void CheckBounds()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.localPosition);

        if((screenPos.x < -sprite.rect.width) ||
        (screenPos.x > (Screen.width + sprite.rect.width)) ||
        (screenPos.y < -sprite.rect.height) ||
        (screenPos.y > (Screen.height + sprite.rect.height)))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Asteroid")
        {
            Asteroid asteroid = col.gameObject.GetComponent<Asteroid>();
            asteroid.Break();
            Destroy(this.gameObject);
        }
    }
}
