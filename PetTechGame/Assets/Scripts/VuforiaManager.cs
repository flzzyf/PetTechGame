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

    public GameObject prefab_plane;

    //侦测到平面并点击平面
    public void OnInteractiveHitTest(HitTestResult result)
    {
        if (GameManager.instance.state != State.created)
        {
            groundPoint = result.Position;

            dog = Instantiate(prefab_dog, result.Position, result.Rotation);
            Instantiate(prefab_plane, result.Position, result.Rotation);

            GameManager.instance.ChangeState(State.created);

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
            GameManager.instance.ChangeState(State.click);
        }
        else
        {
            GameManager.instance.ChangeState(State.scanning);
        }

        hit = false;
    }
}
