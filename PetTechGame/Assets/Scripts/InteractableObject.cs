using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType { food, toy }
public class InteractableObject : MonoBehaviour
{
    public ObjectType type;

    CustomGravity customGravity;

    void Start()
    {
        customGravity = GetComponent<CustomGravity>();
    }

    public void Drag()
    {
        customGravity.enableGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void Left()
    {
        customGravity.enableGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;

    }
}
