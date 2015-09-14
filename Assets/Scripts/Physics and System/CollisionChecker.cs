using UnityEngine;
using System.Collections;

public class CollisionChecker : MonoBehaviour {

    Vector2 dir; // debug only

	public bool WillCollide(Vector2 direction) {
        dir = direction;

        Collider2D collider = CastRay(direction);
        if (collider != null)
        {
            if (collider.isTrigger)
            {
                if (collider.GetComponent<TeleportToScript>() != null)
                {
                    collider.GetComponent<TeleportToScript>().Teleport(gameObject);
                }
            }

            return true;
        }

        return false;
	}

    public GameObject GetCollidedObject(Vector2 direction)
    {
        Collider2D collider = CastRay(direction);
        if (collider != null)
            return collider.gameObject;
        return null;
    }

    Collider2D CastRay(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.5f);
        if (hit != null && hit.collider != null)
        {
            if (hit.collider.isTrigger)
            {
                if (hit.collider.GetComponent<TeleportToScript>() != null)
                {
                    hit.collider.GetComponent<TeleportToScript>().Teleport(gameObject);
                }
            }

            return hit.collider;
        }

        return null;
    }

    void Update()
    {
        Debug.DrawRay(transform.position, dir);
    }
}
