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

    void Start()
    {

    }

    void Update()
    {
        SetText("镜头坐标", cam.position + "");
        SetText("平面坐标", groundPanel.position + "");
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
