﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public Transform target;

    public float range_vision = 8;
    public float range_interact = 4;

    public float stat_hungry = 5;
    public float speed = 3;

    void Start()
    {

    }

    void Update()
    {
        //无目标就搜索目标
        if (target == null)
            SearchTarget();
        else
        {
            //有目标，超过交互范围，靠近
            if (Vector3.Distance(transform.position, target.position) > range_interact)
            {
                Vector3 dir = target.position - transform.position;
                transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
            }
        }
    }

    void SearchTarget()
    {
        Transform nearest = GameManager.instance.interactableobjects[0];

        for (int i = 1; i < GameManager.instance.interactableobjects.Count; i++)
        {
            if (Vector3.Distance(transform.position, GameManager.instance.interactableobjects[i].position) <
                Vector3.Distance(transform.position, GameManager.instance.interactableobjects[i - 1].position))
            {
                nearest = GameManager.instance.interactableobjects[i];
            }
        }

        target = nearest;
    }

}