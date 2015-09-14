using UnityEngine;
using System.Collections;

public class KeyboardListener : MonoBehaviour {

    public KeyListener listener;
    public bool avoidListening = false;
    public bool waitKeyRelease = false;

    void Awake()
    {
        listener = (KeyListener)gameObject.GetComponent<CharacterManager>();
    }

    public void StopListening()
    {
        avoidListening = true;

        if (Input.anyKey)
        {
            waitKeyRelease = true;
        }
    }

	void Update () {
        // Checks action
        if (Input.GetKeyUp(KeyCode.Return))
        {
            listener.Activate();
        }

        // Doesn't process keyboard events
        if (waitKeyRelease)
        {
            if (!Input.anyKey)
            {
                waitKeyRelease = false;
            }
            else
                return;
        }

        if (avoidListening)
            return;

        // Checks movement
        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        int vertical = (int)Input.GetAxisRaw("Vertical");

        // Send movement events
        if (horizontal > 0)
            listener.Move(1);
        else if (horizontal < 0)
            listener.Move(3);
        else if (vertical > 0)
            listener.Move(0);
        else if (vertical < 0)
            listener.Move(2);
	}
}

public interface KeyListener
{
    void Activate();
    void Move(int direction);
}