using UnityEngine;
using Vuforia;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class HitTestResultEvent : UnityEvent<HitTestResult> { }
public class VuforiaManager : MonoBehaviour
{
    public HitTestResultEvent clickEvent;
    public GameObject groundPlanePlacement;

    bool hit = false;

    public void OnInteractiveHitTest(HitTestResult result)
    {
        if (GameManager.instance.isCreating)
        {
            //clickEvent.Invoke(result);
            print("qwe");
            GameManager.instance.groundPanel.position = groundPlanePlacement.transform.position;
            GameManager.instance.groundPanel.GetComponentInChildren<SpriteRenderer>().enabled = true;
        }
    }

    public void OnAutomaticHitTest(HitTestResult result)
    {
        if (GameManager.instance.isCreating)
        {
            hit = true;

            groundPlanePlacement.transform.position = result.Position;
            groundPlanePlacement.transform.rotation = result.Rotation;
        }
    }

    void Update()
    {
        GameManager.Instance().SetText("侦测到平面", hit.ToString());

        hit = false;
    }
}
