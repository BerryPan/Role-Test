using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Linq;
using Google.Protobuf;

public class HealthDemo : MonoBehaviour 
{
    public GameObject PPCO;
    public float maxSP;
    public float currentPower;
    public float falshSpeed = 2f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);
    public GameObject Player;

    float currentHealth;
    PausePanelControl PPC;
	Slider healthSlider;
    Slider powerSlider;
    Image damageImage;
	Color help1,help2;
	Text deadText;
	Animator anim;
	PlayerMovement playerMovement;
	bool isDead;
	bool damaged;
    float maxHP;
    float timer = 0.1f;
    
	// Use this for initialization
	private void Start()
    {
        PPC = PPCO.GetComponent<PausePanelControl>();
        anim = GetComponent<Animator>();
		playerMovement = GetComponent<PlayerMovement>();
        healthSlider = GameObject.Find("Canvas/HealthLine").GetComponent<Slider>();
        powerSlider = GameObject.Find("Canvas/PowerLine").GetComponent<Slider>();
        damageImage = GameObject.Find("Canvas/damageImage").GetComponent<Image>(); 
        deadText = GameObject.Find("Canvas/deadText").GetComponent<Text>();

        ConncetServer_local();
        maxHP = PPC.health;
        maxSP = PPC.power;
		currentHealth = PPC.health;
        currentPower = maxSP;
		help1 = damageImage.color;
		help1.a = 255;
		help2 = deadText.color;
		help2.a = 255;
    }
	
	// Update is called once per frame
	private void Update ()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
        {
            ConncetServer_local();
            timer = 0.1f;
        }
        
		if(Input.GetKeyDown(KeyCode.P))
		{
            PPC.InitColor();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            PPCO.SetActive(true);
        }
		if(isDead==true&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9)
		{
			damageImage.color = Color.Lerp(damageImage.color,help1,Time.deltaTime*falshSpeed/5);
			deadText.color = Color.Lerp(deadText.color,help2,Time.deltaTime*falshSpeed/5);
            PPCO.SetActive(true);
            GameObject.Find("Continue").SetActive(false);

            PPC.InitColor();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            GetComponent<Animator>().enabled = false;
            GetComponent<HealthDemo>().enabled = false;
        }
		if(isDead==false&&damaged)
		{
			damageImage.color = flashColor;
		}
		else
		{
			damageImage.color = Color.Lerp(damageImage.color,Color.clear,Time.deltaTime*falshSpeed);
		}
		damaged = false;
        healthSlider.value = currentHealth;
        powerSlider.value = currentPower;
        if(maxSP!=PPC.power)
        {
            maxSP = PPC.power;
            powerSlider.maxValue = maxSP;
        }
        if (maxHP != PPC.health)
        {
            maxHP = PPC.health; 
            healthSlider.maxValue = maxHP;
        }
        if (transform.position.y<-5) TakeDamage(10);
        if (transform.position.y > 10) TakeDamage(Time.deltaTime * (transform.position.y - 9)*10);
    }
    void ConncetServer_local()
    {
        IPAddress ipAdr = IPAddress.Parse("101.132.135.198");
        IPEndPoint ipEp = new IPEndPoint(ipAdr, 1234);

        Socket clientScoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientScoket.Connect(ipEp);

        float xx = GetComponent<Transform>().position.x;
        float yy = GetComponent<Transform>().position.y;
        float zz = GetComponent<Transform>().position.z;
        float rx = GetComponent<Transform>().localEulerAngles.x;
        float ry = GetComponent<Transform>().localEulerAngles.y;
        float rz = GetComponent<Transform>().localEulerAngles.z;
        Local loc = new Local { PosX = xx, PosY = yy, PosZ = zz, RotX = rx, RotY = ry, RotZ = rz, Name = name, Hp = currentHealth };
        byte[] concent = loc.ToByteArray();
        clientScoket.Send(concent);
        byte[] response = new byte[1024];
        int len_recv = clientScoket.Receive(response);
        if (len_recv > 0)
        {
            byte[] data = response.Take(len_recv).ToArray();
            Another another = Another.Parser.ParseFrom(data);

            if (GameObject.Find(another.Name))
            {
                GameObject.Find(another.Name).GetComponent<Transform>().position = new Vector3(another.PosX, another.PosY, another.PosZ);
                GameObject.Find(another.Name).GetComponent<Transform>().localEulerAngles = new Vector3(another.RotX, another.RotY, another.RotZ);
                if(another.Hp == 0)
                {
                    Destroy(GameObject.Find(another.Name));
                }
            }
            else
            {
                if (another.Name != null && another.Hp != 0)
                {
                    GameObject go = Instantiate(Player);
                    go.transform.name = another.Name;
                    GameObject.Find(another.Name).GetComponent<Transform>().position = new Vector3(another.PosX, another.PosY, another.PosZ);
                    GameObject.Find(another.Name).GetComponent<Transform>().localEulerAngles = new Vector3(another.RotX, another.RotY, another.RotZ);
                }
            }
        }


        clientScoket.Shutdown(SocketShutdown.Both);
        clientScoket.Close();
    }

    public void TakeDamage(float amount)
	{
		if(amount>0) damaged = true;
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, PPC.health);
		if(currentHealth == 0 && !isDead)
		{
            Death();
		}
	}
	void Death()
	{
		isDead = true;
		playerMovement.enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        GetComponent<Rigidbody>().freezeRotation=true;
        anim.SetTrigger("Die");
	}
}
