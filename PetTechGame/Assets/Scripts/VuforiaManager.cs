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

    public void OnInteractiveHitTest(HitTestResult result)
    {
        if (!created)
            clickEvent.Invoke(result);
    }

    bool hit = false;
    public void Hit()
    {
        hit = true;
    }

    private void LateUpdate()
    {
        text.text = "侦测到平面：" + hit.ToString();

        hit = false;
    }
}
