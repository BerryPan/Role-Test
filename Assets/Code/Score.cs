using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    Text data;
    public int score=0;
	// Use this for initialization
	void Start () {
        data = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        data.text = score.ToString();
	}
}
