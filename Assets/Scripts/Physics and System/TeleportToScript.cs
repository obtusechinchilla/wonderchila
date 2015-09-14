using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TeleportToScript : MonoBehaviour {

    public string mapName;
    public Vector2 position;
    public LayerMask mask;

    bool teleporting = false;

	void Update () {
	    
	}

    public void Teleport(GameObject player)
    {
        // For when we animate the teleporting
        if (teleporting)
            return;

        // Once it's teleporting, don't try to teleport again
        teleporting = true;

        // Stops if doesn't find the map
        GameObject map = GameObject.Find(mapName);
        if (map == null)
        {
            teleporting = false;
            return;
        }

        // Stops the damping so the camera doesn't move with the player
        Camera2DFollow follow = Camera.main.GetComponent<Camera2DFollow>();
        float damping = follow.damping;
        follow.damping = 0;
        
        // Moves the player around
        Vector3 newPos = map.transform.TransformPoint(position);
        newPos.z = player.transform.position.z;
        player.transform.position = newPos;

        // Cancels keyboard input
        KeyboardListener keyboard = player.GetComponent<KeyboardListener>();
        keyboard.StopListening();

        // TODO: cancels NPC movement

        // Update the camera and puts the damping again
        follow.UpdateCamera();
        follow.damping = damping;

        keyboard.avoidListening = false;

        teleporting = false;
    }
}
