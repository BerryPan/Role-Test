using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletNum : MonoBehaviour {
    Text data;
    public PausePanelControl PPC;
    public float num = 5;
    // Use this for initialization
    void Start () {
        data = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        num = Mathf.Clamp(num + Time.deltaTime/PPC.cooling, 0, PPC.bulletnum);
        data.text = num.ToString("0.00");
    }
}
