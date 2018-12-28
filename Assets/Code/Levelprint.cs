using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Levelprint : MonoBehaviour {
    Text data;
    public Text word;
    Color end,begin;
    // Use this for initialization
    void Start()
    {
        data = GetComponent<Text>();
        end = data.color;
        begin = data.color;
        begin.a = 255;
        end.a = 0;
    }
    public void LevelPrint(int level)
    {
        data.color = begin;
        data.text = level.ToString();
        word.color = begin;
    }
    // Update is called once per frame
    void Update()
    {
        if (data.color != end&&word.color!=end)
        {
            data.color = Color.Lerp(data.color, end, Time.deltaTime*2);
            word.color = Color.Lerp(word.color, end, Time.deltaTime*2);
        }
    }
}
