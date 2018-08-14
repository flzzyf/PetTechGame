using UnityEngine;
using Vuforia;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class HitTestResultEvent : UnityEvent<HitTestResult> { }
public class VuforiaManager : MonoBehaviour
{
    public HitTestResultEvent clickEvent;

    public Text text;

    bool created = false;

    public GameObject groundPlane;

    public void OnInteractiveHitTest(HitTestResult result)
    {
        if (!created)
            clickEvent.Invoke(result);
    }

    bool hit = false;
    public void OnAutomaticHitTest(HitTestResult result)
    {
        hit = true;

        groundPlane.transform.position = result.Position;
        groundPlane.transform.rotation = result.Rotation;
    }

    private void LateUpdate()
    {
        text.text = "侦测到平面：" + hit.ToString();

        hit = false;
    }
}
