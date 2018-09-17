using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public enum State { none, scanning, click, created };

public class GameManager : Singleton<GameManager>
{
    public Transform cam;
    public Transform itemHolder;
    public float itemSpeed = 1;
    [HideInInspector]
    public GameObject holdingItem;

    public List<Transform> interactableobjects;

    public GameObject prefab_food;
    [HideInInspector]
    public State state = State.none;

    public GameObject text_hint_scanning;
    public GameObject text_hint_click;
    public GameObject panel_created;
    public float holdingDistance = 1;

    public float throwTriggerDistance = 1;

    Vector2 mousePreviousPos;

    public Slider slider_hungry;
    public Slider slider_happiness;

    public GameObject prefab_ball;

    public Transform world;

    void Start()
    {
        ChangeState(State.scanning);

        world = new GameObject("World").transform;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Item" &&
                    !hit.collider.gameObject.GetComponent<InteractableObject>().isDraging)
                {
                    holdingItem = hit.collider.gameObject;
                    holdingItem.GetComponent<InteractableObject>().Drag();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (holdingItem != null)
            {
                holdingItem.GetComponent<InteractableObject>().Left();
                //投掷判定
                //GameManager.instance.SetText("触屏", Input.mousePosition.y + ", " + mousePreviousPos.y);
                if (Input.mousePosition.y - mousePreviousPos.y > 0)
                {
                    float forceAmount = (Input.mousePosition.y - mousePreviousPos.y) / Screen.height;

                    Vector3 force = Camera.main.transform.forward + Camera.main.transform.up;
                    force = force.normalized * forceAmount * 20;
                    //GameManager.instance.SetText("qwe", force + "");

                    holdingItem.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
                }

                holdingItem = null;
            }
        }

        if (Input.GetMouseButton(0))
        {
            mousePreviousPos = Input.mousePosition;
        }

        //拖动物体
        if (holdingItem != null)
        {
            MoveToCursor(holdingItem.transform);
        }

        //两指缩放
        TouchToScaleWorld();
    }

    public float touchesSensitivity = 0.2f;
    float previousTouchesDistance;
    void TouchToScaleWorld()
    {
        if (Input.touchCount == 2)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved ||
                                     Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                float scaleValue = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position) -
                                    previousTouchesDistance;
                scaleValue = scaleValue / Screen.width;
                SetText("缩放值", scaleValue + "");
                world.localScale = world.localScale + Vector3.one * scaleValue;

                previousTouchesDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                SetText("缩放比率", previousTouchesDistance + "");
            }


        }
    }

    //移动到屏幕点击位置
    void MoveToCursor(Transform _obj)
    {
        Vector3 desiredPos = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(holdingDistance);
        _obj.position = Vector3.Lerp(_obj.position, desiredPos, itemSpeed * Time.deltaTime);
    }

    public void CreateFood()
    {
        Vector3 pos = GetRandomPointToCreateObject();

        GameObject go = Instantiate(prefab_food, pos, Quaternion.identity, world);
        interactableobjects.Add(go.transform);
    }

    public void CreateBall()
    {
        Vector3 pos = GetRandomPointToCreateObject();

        GameObject go = Instantiate(prefab_ball, pos, Quaternion.identity, world);
        interactableobjects.Add(go.transform);
    }
    //获取物体随机生成点
    Vector3 GetRandomPointToCreateObject()
    {
        Vector2 screenPoint = new Vector2(Random.Range(0f, 1f) * Screen.width,
                                            Random.Range(1f, 1.2f) * Screen.height);
        Vector3 pos = Camera.main.ScreenPointToRay(screenPoint).GetPoint(Random.Range(1.5f, 2.5f));
        return pos;
    }

    public void ChangeState(State _state)
    {
        if (state == _state)
            return;
        state = _state;

        text_hint_scanning.SetActive(false);
        text_hint_click.SetActive(false);
        panel_created.SetActive(false);

        if (state == State.scanning)
        {
            text_hint_scanning.SetActive(true);
        }
        else if (state == State.click)
        {
            text_hint_click.SetActive(true);
        }
        else if (state == State.created)
        {
            panel_created.SetActive(true);
        }
    }

    #region 多行文字系统
    public Text text;
    Dictionary<string, string> textDictionary = new Dictionary<string, string>();

    public void SetText(string _key, string _text)
    {
        if (!textDictionary.ContainsKey(_key))
        {
            //未含此键，添加
            textDictionary.Add(_key, _text);
        }
        else
        {
            //已含有，修改
            textDictionary[_key] = _text;
        }

        UpdateText();
    }

    void UpdateText()
    {
        string t = "";

        foreach (string item in textDictionary.Keys)
        {
            string s = item + ": " + textDictionary[item];
            t += s + "\n";
        }
        text.text = t;
    }
}
#endregion