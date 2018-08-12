using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    none,
    left,
    right,
    up,
    down
}

public class WrappableObject : MonoBehaviour
{
    public Sprite sprite;

	// Update is called once per frame
	void Update ()
    {
        switch (isOutOfBounds())
        {
            case Direction.left:
                Wrap(Direction.left);
                break;

            case Direction.right:
                Wrap(Direction.right);
                break;

            case Direction.up:
                Wrap(Direction.up);
                break;

            case Direction.down:
                Wrap(Direction.down);
                break;

            default:
                break;
        }
	}

    Direction isOutOfBounds()
    { 
        if(Camera.main.WorldToScreenPoint(transform.localPosition).x < -(sprite.rect.width * 2.0f))
        {
            return Direction.left;
        }

        else if (Camera.main.WorldToScreenPoint(transform.localPosition).x > (Screen.width + (sprite.rect.width * 2.0f)))
        {
            return Direction.right;
        }

        else if (Camera.main.WorldToScreenPoint(transform.localPosition).y < -(sprite.rect.height * 2.0f))
        {
            return Direction.up;
        }

        else if (Camera.main.WorldToScreenPoint(transform.localPosition).y > (Screen.height + (sprite.rect.height * 2.0f)))
        {
            return Direction.down;
        }

        else
        {
            return Direction.none;
        }
    }

    void Wrap(Direction direction)
    {
        Vector3 pos = transform.localPosition;

        switch (direction)
        {
            case Direction.left:
                pos.x = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, 0.0f)).x + (transform.localScale.x * 0.5f);
                break;

            case Direction.right:
                pos.x = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).x - (transform.localScale.x * 0.5f);
                break;

            case Direction.up:
                pos.y = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, 0.0f)).y + (transform.localScale.y * 0.5f);
                break;

            case Direction.down:
                pos.y = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).y + (transform.localScale.y * 0.5f);
                break;

            default:
                break;
        }

        transform.localPosition = pos;
    }
}
