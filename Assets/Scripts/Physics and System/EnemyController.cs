using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class EnemyController : MonoBehaviour
{
    KeyListener listener;
    public Transform target;

    float attackCooldown = 0.5f;
    float currentCooldown = 0;

    void Awake()
    {
        listener = GetComponent<CharacterManager>();
    }
    void Update()
    {   
        int movement = 0;

        if (target != null)
        {
            Vector2 diff = transform.position - target.position;
            if (Math.Abs(diff.x) > Math.Abs(diff.y))
            {
                if (diff.x > 0)
                    movement = 3;
                else
                    movement = 1;
            }
            else
            {
                if (diff.y > 0)
                    movement = 2;
                else
                    movement = 0;
            }
        }
        else
        {
            movement = new System.Random().Next(0, 3);
        }

        if (currentCooldown <= 0)
        {
            listener.Attack(movement);
            currentCooldown = attackCooldown;
        }

        currentCooldown -= Time.deltaTime;

        listener.Move(movement);
    }
}
