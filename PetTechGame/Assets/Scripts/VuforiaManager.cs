using UnityEngine;
using Vuforia;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class HitTestResultEvent : UnityEvent<HitTestResult> { }
public class VuforiaManager : MonoBehaviour
{
    public HitTestResultEvent clickEvent;
    public GameObject groundPlane;

    bool hit = false;

    public void OnInteractiveHitTest(HitTestResult result)
    {
        if (GameManager.instance.isCreating)
        {
            clickEvent.Invoke(result);
        }
    }

    public void OnAutomaticHitTest(HitTestResult result)
    {
        if (GameManager.instance.isCreating)
        {
            hit = true;

            groundPlane.transform.position = result.Position;
            groundPlane.transform.rotation = result.Rotation;
        }
    }

    void Update()
    {
        GameManager.Instance().SetText("侦测到平面", hit.ToString());

        hit = false;
    }
}
