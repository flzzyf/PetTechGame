using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public Transform cam;
    public Transform groundPanel;
    public Transform itemHolder;
    public float itemSpeed = 1;
    [HideInInspector]
    public GameObject holdingItem;

    public List<Transform> interactableobjects;

    public GameObject prefab_food;

    public bool isCreating;

    void Start()
    {

    }

    void Update()
    {
        SetText("镜头坐标", cam.position + "");
        SetText("平面坐标", groundPanel.position + "");

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Item")
                {
                    holdingItem = hit.collider.gameObject;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            holdingItem = null;
        }

        if (holdingItem != null)
        {
            MoveToCursor(holdingItem.transform);
        }
    }
    public float holdingDistance = 1;
    void MoveToCursor(Transform _obj)
    {
        Vector3 desiredPos = GetScreenPoint(Input.mousePosition, holdingDistance);
        _obj.position = Vector3.Lerp(_obj.position, desiredPos, itemSpeed * Time.deltaTime);
    }

    Vector3 GetScreenPoint(Vector2 _point, float _distance)
    {
        Vector3 point = new Vector3(_point.x, _point.y, _distance);

        point = Camera.main.ScreenToWorldPoint(point);

        return point;
    }

    public void ToggleCreateDog(bool _create)
    {
        isCreating = _create;
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