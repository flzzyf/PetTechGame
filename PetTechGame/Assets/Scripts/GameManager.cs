using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public Text text;
    Dictionary<string, string> textDictionary = new Dictionary<string, string>();

    public Transform cam;

    public Transform groundPanel;
    public Transform itemHolder;
    public float itemSpeed = 1;

    void Update()
    {
        SetText("镜头坐标", cam.position + "");
        SetText("平面坐标", groundPanel.position + "");

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(" you clicked on " + hit.collider.gameObject.name);
                if (hit.collider.gameObject.tag == "Item")
                {
                    Vector3 targetdir = itemHolder.position - hit.collider.transform.position;
                    targetdir.Normalize();
                    hit.collider.transform.Translate(targetdir * itemSpeed * Time.deltaTime);
                }
            }
        }
    }

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
