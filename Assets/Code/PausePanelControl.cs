using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausePanelControl : MonoBehaviour {
    public GameObject RST;
    public GameObject CTN;
    public GameObject HL;
    public GameObject PW;
    public GameObject DMG;
    public GameObject CL;
    public GameObject CST;
    public GameObject BN;

    public Score S;

    Text HLt;
    Text PWt;
    Text DMGt;
    Text CLt;
    Text CSTt;
    Text BNt;

    public float health=100;
    public float power=10f;
    public float damage=20;
    public float cooling=2f;
    public float cost=30;
    public float bulletnum = 5;
    // Use this for initialization
    void Start () {
        Button btn = RST.GetComponentInChildren<Button>();
        btn.onClick.AddListener(delegate () {
            onClickRST(RST);
        });
        btn = CTN.GetComponentInChildren<Button>();
        btn.onClick.AddListener(delegate () {
            onClickCTN(CTN);
        });
        btn = HL.GetComponentInChildren<Button>();
        btn.onClick.AddListener(delegate (){
            onClick(ref health, 10, 100, 300, 50, HLt);
        });
        btn = PW.GetComponentInChildren<Button>();
        btn.onClick.AddListener(delegate () {
            onClick(ref power, 1, 10, 30, 50, PWt);
        });
        btn = DMG.GetComponentInChildren<Button>();
        btn.onClick.AddListener(delegate () {
            onClick(ref damage, 5, 20, 40, 400, DMGt);
        });
        btn = CL.GetComponentInChildren<Button>();
        btn.onClick.AddListener(delegate () {
            onClick(ref cooling, -0.1f, 1, 2, 200, CLt);
        });
        btn = CST.GetComponentInChildren<Button>();
        btn.onClick.AddListener(delegate () {
            onClick(ref cost, -5, 10, 30, 500, CSTt);
        });
        btn = BN.GetComponentInChildren<Button>();
        btn.onClick.AddListener(delegate () {
            onClick(ref bulletnum, 1, 5, 12, 200, BNt);
        });
        HLt =HL.GetComponentsInChildren<Text>()[1];
        PWt =PW.GetComponentsInChildren<Text>()[1];
        DMGt =DMG.GetComponentsInChildren<Text>()[1];
        CLt =CL.GetComponentsInChildren<Text>()[1];
        CSTt = CST.GetComponentsInChildren<Text>()[1];
        BNt=BN.GetComponentsInChildren<Text>()[1];

        InitColor();
    }
    public void InitColor()
    {
        HLt.color = Color.white;
        PWt.color = Color.white;
        DMGt.color = Color.white;
        CLt.color = Color.white;
        CSTt.color = Color.white;
        BNt.color = Color.white;
    }
	
	// Update is called once per frame
	void Update () {
        HLt.text = health.ToString("000");
        PWt.text = power.ToString("00");
        DMGt.text = damage.ToString("00");
        CLt.text = cooling.ToString("0.0");
        CSTt.text = cost.ToString("00");
        BNt.text = bulletnum.ToString("00");
    }
    void onClickRST(GameObject obj)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void onClickCTN(GameObject obj)
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        this.gameObject.SetActive(false);
    }
    void onClick(ref float data, float bonus, float lband, float hband, int cost,Text index)
    {
        if (S.score < cost) return;
        if (data >= hband && bonus > 0) return;
        if (data <= lband && bonus < 0) return;
        S.score -= cost;
        data += bonus;
        index.color = Color.green;
    }

}
