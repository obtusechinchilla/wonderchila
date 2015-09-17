using UnityEngine;
using System.Collections;

public class ProjectileHandler : MonoBehaviour {

    public Vector2 direction;
    public LayerMask layerMask;

    CollisionChecker collisionChecker;

    float remainingLife;
    float lifeSpan = 0.35f;
    int distance = 3;

    Vector2 initialPos;
    Vector2 finalPos;

    float multiplier;
    float ratio;

    void Start()
    {
        remainingLife = lifeSpan;
        ratio = 0;
        multiplier = 1 / lifeSpan;

        initialPos = transform.position;
        finalPos = new Vector3(initialPos.x + (distance * direction.x), initialPos.y + (distance * direction.y), transform.position.z);

        collisionChecker = GetComponent<CollisionChecker>();
        collisionChecker.layerMask = layerMask;
    }

	void Update ()
    {
        if (remainingLife <= 0 || collisionChecker.WillCollide(direction))
        {
            DestroyImmediate(gameObject);
            return;
        }

        //Vector2 pos = transform.position;
        //pos += direction * speed * Time.deltaTime;
        //transform.position = new Vector3(pos.x, pos.y, transform.position.z);

        float x = Mathf.Lerp(initialPos.x, finalPos.x, ratio);
        float y = Mathf.Lerp(initialPos.y, finalPos.y, ratio);
        transform.position = new Vector3(x, y, transform.position.z);

        ratio += Time.deltaTime * multiplier;

        remainingLife -= Time.deltaTime;
	}
}
