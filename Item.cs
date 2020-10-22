using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    public string type;
    public string description;
    public Sprite icon;
    public int value;
    public string name;

    public bool pickedUp;
    public bool shouldBeDestroyed = false;
}

   
