using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Rigidbody rb;
    bool draging;

    void Update()
    {

    }

    public void Drag()
    {
        draging = true;
        rb.useGravity = false;
    }

    public void Left()
    {
        draging = false;
        rb.useGravity = true;
    }
}
