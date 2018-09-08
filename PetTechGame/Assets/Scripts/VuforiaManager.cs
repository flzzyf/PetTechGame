using UnityEngine;
using Vuforia;
using UnityEngine.Events;
using UnityEngine.UI;

public class VuforiaManager : Singleton<VuforiaManager>
{
    public GameObject groundPlanePlacement;
    public GameObject prefab_panel;
    public Transform groundPanel;
    public Transform dog;

    public GameObject text_hint_ground;
    public GameObject text_hint_click;

    public Vector3 groundPoint;

    bool hit = false;

    //侦测到平面并点击平面
    public void OnInteractiveHitTest(HitTestResult result)
    {
        if (GameManager.instance.isCreating)
        {
            groundPoint = result.Position;

            dog.position = result.Position;
            dog.rotation = result.Rotation;
        }
    }

    //自动侦测到平面
    public void OnAutomaticHitTest(HitTestResult result)
    {
        if (GameManager.instance.isCreating)
        {
            hit = true;

            Instantiate(prefab_panel, result.Position, result.Rotation);

            groundPlanePlacement.transform.position = result.Position;
            groundPlanePlacement.transform.rotation = result.Rotation;
        }
    }

    void Update()
    {
        if (hit)
        {
            text_hint_ground.SetActive(false);
            text_hint_click.SetActive(true);
        }
        else
        {
            text_hint_ground.SetActive(true);
            text_hint_click.SetActive(false);
        }


        hit = false;
    }
}
