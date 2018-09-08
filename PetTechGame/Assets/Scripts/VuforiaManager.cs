using UnityEngine;
using Vuforia;
using UnityEngine.Events;
using UnityEngine.UI;

public class VuforiaManager : Singleton<VuforiaManager>
{
    public GameObject groundPlanePlacement;
    public GameObject prefab_dog;

    [HideInInspector]
    public Vector3 groundPoint;

    bool hit = false;

    GameObject dog;

    //侦测到平面并点击平面
    public void OnInteractiveHitTest(HitTestResult result)
    {
        if (GameManager.instance.state != State.created)
        {
            GameManager.instance.state = State.created;
            groundPoint = result.Position;

            dog = Instantiate(prefab_dog, result.Position, result.Rotation);

            GameManager.instance.ChangeState();

            groundPlanePlacement.SetActive(false);
        }
    }

    //自动侦测到平面
    public void OnAutomaticHitTest(HitTestResult result)
    {
        if (GameManager.instance.state != State.created)
        {
            hit = true;

            groundPlanePlacement.transform.position = result.Position;
            groundPlanePlacement.transform.rotation = result.Rotation;
        }
    }

    void Update()
    {
        if (GameManager.instance.state == State.created)
            return;

        if (hit)
        {
            GameManager.instance.state = State.hit;
        }
        else
        {
            GameManager.instance.state = State.scanning;
        }

        GameManager.instance.ChangeState();

        hit = false;


    }
}
