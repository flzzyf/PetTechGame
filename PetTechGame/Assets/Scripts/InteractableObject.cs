using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Rigidbody rb;
    bool draging;

    public float fallingSpeed = 10;

    void Update()
    {
        if (draging)
            return;

        if (transform.position.y > VuforiaManager.instance.groundPoint.y)
        {
            //transform.Translate(Vector3.down * fallingSpeed * Time.deltaTime, Space.World);
            rb.useGravity = true;
        }
        else
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }
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
