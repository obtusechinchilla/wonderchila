using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PlayerManager : CharacterManager
{
    public int hits = 0;
    public override void TakeHit()
    {
        hits++;

        if (hits == 1)
            GetComponent<SpriteRenderer>().color = Color.gray;
        else if (hits == 2)
            GetComponent<SpriteRenderer>().color = Color.magenta;
        else if (hits == 3)
            GetComponent<SpriteRenderer>().color = Color.red;
        else
            DestroyImmediate(gameObject);

        return;
    }
}
