using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public float fallingSpeed = 1;
    [HideInInspector]
    public bool draging;

    void Update()
    {
        if (draging)
            return;

        if (transform.position.y > VuforiaManager.instance.groundPoint.y)
        {
            transform.Translate(Vector3.down * fallingSpeed * Time.deltaTime, Space.World);
        }
    }
}
