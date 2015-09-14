using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterManager))]
[RequireComponent(typeof(CollisionChecker))]
public class CharacterManager : MonoBehaviour, KeyListener {

    public LayerMask collisionMask;

    float defaultMovementCooldown = 0.25f;
    float movementCooldown = 0;

    CollisionChecker collisionChecker;
    Vector2 currentDirection;

    // Movement animation control
    Vector3 initialPosition;
    Vector3 finalPosition;
    float ratio = 0;
    float multiplier = 0;

    void Start()
    {
        movementCooldown = defaultMovementCooldown;
        initialPosition = transform.position;
        finalPosition = transform.position;

        collisionChecker = GetComponent<CollisionChecker>();
    }

	void Update () {
        if (movementCooldown > 0)
        {
            movementCooldown -= Time.deltaTime;
            ratio += Time.deltaTime * multiplier;

            float x = Mathf.Lerp(initialPosition.x, finalPosition.x, ratio);
            float y = Mathf.Lerp(initialPosition.y, finalPosition.y, ratio);

            transform.position = new Vector3(x, y, transform.position.z);
        }
	}

    void KeyListener.Move(int direction)
    {
        if (movementCooldown > 0)
            return;

        int horizontal = 0;
        int vertical = 0;
        switch (direction)
        {
            case 0:
                vertical++;
                break;
            case 1:
                horizontal++;
                break;
            case 2:
                vertical--;
                break;
            case 3:
                horizontal--;
                break;
        }

        currentDirection = new Vector2(horizontal, vertical);

        bool collided = collisionChecker.WillCollide(currentDirection);
        if (collided)
            return;

        // Defines the variables that will realize the movement
        movementCooldown = defaultMovementCooldown;
        initialPosition = transform.position;
        finalPosition = new Vector3(initialPosition.x + horizontal, initialPosition.y + vertical, initialPosition.z);
        multiplier = 1 / defaultMovementCooldown;
        ratio = 0;
    }

    void KeyListener.Activate()
    {
        GameObject obj = collisionChecker.GetCollidedObject(currentDirection);
        if (obj != null)
        {
            //NPC npc = obj.GetComponent<NPC>();
            //if (npc != null)
            //{
            //    npc.Activate();
            //}
        }
    }
}
