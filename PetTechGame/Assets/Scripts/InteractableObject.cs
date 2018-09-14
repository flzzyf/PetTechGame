using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public CustomGravity customGravity;

    void Start()
    {
        customGravity = GetComponent<CustomGravity>();
    }

    public void Drag()
    {
        customGravity.enableGravity = false;
    }

    public void Left()
    {
        customGravity.enableGravity = true;
    }
}
