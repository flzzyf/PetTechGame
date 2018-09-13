using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public enum State { scanning, hit, created };

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
    public State state;

    public GameObject text_hint_ground;
    public GameObject text_hint_click;
    public GameObject panel_created;
    public float holdingDistance = 1;

    void Update()
    {
        //SetText("镜头坐标", cam.position + "");

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Item")
                {
                    holdingItem = hit.collider.gameObject;
                    holdingItem.GetComponent<InteractableObject>().draging = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            holdingItem.GetComponent<InteractableObject>().draging = false;
            holdingItem = null;
        }

        if (holdingItem != null)
        {
            MoveToCursor(holdingItem.transform);
        }
    }

    //移动到屏幕点击位置
    void MoveToCursor(Transform _obj)
    {
        Vector3 desiredPos = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(1);
        _obj.position = Vector3.Lerp(_obj.position, desiredPos, itemSpeed * Time.deltaTime);
    }

    public void CreateFood()
    {
        float range = 2;
        Vector3 pos = Vector3.one;
        pos.x *= Random.Range(0, range);
        pos.y *= Random.Range(0, range);
        pos.z *= Random.Range(0, range);

        print(pos);

        GameObject go = Instantiate(prefab_food, pos, Quaternion.identity);
        interactableobjects.Add(go.transform);
    }

    public void ChangeState()
    {
        text_hint_ground.SetActive(false);
        text_hint_click.SetActive(false);
        panel_created.SetActive(false);

        if (state == State.scanning)
        {
            text_hint_ground.SetActive(true);
        }
        else if (state == State.hit)
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