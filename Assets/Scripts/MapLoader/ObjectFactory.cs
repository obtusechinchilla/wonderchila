using MapReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ObjectFactory
{
    public static GameObject InstantiateObject(ObjectGroup objectGroup, MapObject someMapObject, float scale)
    {
        GameObject newObject = null;
        
        if (objectGroup.name == "Collisions")
        {
            return null;

            //newObject = new GameObject();
            //newObject.name = objectGroup.name;
            //newObject.AddComponent<BoxCollider2D>().size = new Vector2(someMapObject.width / scale, someMapObject.height / scale);
            //newObject.GetComponent<BoxCollider2D>().offset = new Vector2(someMapObject.width / 2 / scale - 0.5f, someMapObject.height / 2 / scale - 0.5f);
        }
        else if (someMapObject.properties != null)
        {
            if (someMapObject.type == "Teleport")
            {
                newObject = new GameObject();
                newObject.name = objectGroup.name + " - TeleportTo: " + someMapObject.properties["TeleportTo"];
                TeleportToScript script = newObject.AddComponent<TeleportToScript>();
                string[] values = ((string)someMapObject.properties["TeleportTo"]).Split(',');
                script.mapName = values[0];
                script.position = new Vector2(float.Parse(values[1]), float.Parse(values[2]));

                string[] args = new string[1];
                args[0] = "Player";
                script.mask = LayerMask.GetMask(args);

                BoxCollider2D collider = newObject.AddComponent<BoxCollider2D>();
                collider.size = new Vector2(someMapObject.width / scale, someMapObject.height / scale);
                collider.offset = new Vector2(someMapObject.width / 2 / scale - 0.5f, someMapObject.height / 2 / scale - 0.5f);
                collider.isTrigger = true;
            }
        }

        return newObject;
    }
}