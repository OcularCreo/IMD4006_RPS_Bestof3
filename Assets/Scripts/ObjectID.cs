using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectID : MonoBehaviour
{

    [SerializeField] private int objectID;

    //returns id of the object
    public int getID()
    {
        return objectID;
    }
}
