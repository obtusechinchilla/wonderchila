using UnityEngine;
using System.Collections;

public class Camera2DFollow : MonoBehaviour {
	
	public Transform target;
	public float damping = 1;
	public float lookAheadFactor = 3;
	public float lookAheadReturnSpeed = 0.5f;
	public float lookAheadMoveThreshold = 0.1f;
	
	public Transform bottomLeftMax;
	public Transform upperRightMax;
	
	float offsetZ;
	Vector3 lastTargetPosition;
	Vector3 currentVelocity;
	Vector3 lookAheadPos;
	
	// Use this for initialization
	void Start () {
        lastTargetPosition = target.position;
		offsetZ = (transform.position - target.position).z;
		transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateCamera();	
	}

    public void UpdateCamera()
    {
        if (target == null)
            return;

        // only update lookahead pos if accelerating or changed direction
        float xMoveDelta = (target.position - lastTargetPosition).x;

        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

        if (updateLookAheadTarget)
        {
            lookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
        }
        else
        {
            lookAheadPos = Vector3.MoveTowards(lookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
        }

        Vector3 aheadTargetPos = target.position + lookAheadPos + Vector3.forward * offsetZ;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, damping);

        // sets the bounds the camera can show if the reference tiles are not null
        if (bottomLeftMax != null && upperRightMax != null)
        {
            float camHorizontalExtend = Camera.main.orthographicSize * Screen.width / Screen.height;
            float camVerticalExtend = Camera.main.orthographicSize;

            // calculate the x position where the camera can see the edge of the sprite
            Sprite sprite = bottomLeftMax.GetComponent<SpriteRenderer>().sprite;
            float spriteWidth = sprite.rect.width / sprite.bounds.size.x;
            spriteWidth = sprite.bounds.size.x / spriteWidth;

            float edgeVisiblePositionRight = upperRightMax.position.x + 0.5f - camHorizontalExtend;
            float edgeVisiblePositionLeft = bottomLeftMax.position.x - 0.5f + camHorizontalExtend;
            float edgeVisiblePositionDown = bottomLeftMax.position.y - 0.5f + camVerticalExtend;
            float edgeVisiblePositionUp = upperRightMax.position.y + 0.5f - camVerticalExtend;

            newPos = new Vector3(Mathf.Clamp(newPos.x, edgeVisiblePositionLeft, edgeVisiblePositionRight), Mathf.Clamp(newPos.y, edgeVisiblePositionDown, edgeVisiblePositionUp), newPos.z);
        }

        transform.position = newPos;

        lastTargetPosition = target.position;
    }
}
