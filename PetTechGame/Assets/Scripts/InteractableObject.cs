using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType { food, toy }
public class InteractableObject : MonoBehaviour
{
    public ObjectType type;

    CustomGravity customGravity;

    [HideInInspector]
    public bool isDraging;

    void Start()
    {
        customGravity = GetComponent<CustomGravity>();
    }

    public void Drag()
    {
        isDraging = true;

        customGravity.enableGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void Left()
    {
        isDraging = false;

        GetComponent<Rigidbody>().isKinematic = false;
        customGravity.enableGravity = true;
    }
}
