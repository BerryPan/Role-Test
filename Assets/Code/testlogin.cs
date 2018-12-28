using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Linq;
using Google.Protobuf;

public class MySqlAccess
{

    //连接类对象
    private static MySqlConnection mySqlConnection;
    //IP地址
    private static string host;
    //端口号
    private static string port;
    //用户名
    private static string userName;
    //密码
    private static string password;
    //数据库名称
    private static string databaseName;

    public MySqlAccess(string _host, string _port, string _userName, string _password, string _databaseName)
    {
        host = _host;
        port = _port;
        userName = _userName;
        password = _password;
        databaseName = _databaseName;
        OpenSql();
    }


    public void OpenSql()
    {
        try
        {
            string mySqlString = string.Format("Database={0};Data Source={1};User Id={2};Password={3};port={4}"
                , databaseName, host, userName, password, port);
            mySqlConnection = new MySqlConnection(mySqlString);
            //if(mySqlConnection.State == ConnectionState.Closed)
            mySqlConnection.Open();

        }
        catch (Exception e)
        {
            throw new Exception("服务器连接失败，请重新检查MySql服务是否打开。" + e.Message.ToString());
        }
    }
    
    public void CloseSql()
    {
        if (mySqlConnection != null)
        {
            mySqlConnection.Close();
            mySqlConnection.Dispose();
            mySqlConnection = null;
        }
    }

    public DataSet Select(string tableName, string[] items, string[] whereColumnName,
        string[] operation, string[] value)
    {
        Debug.Log(whereColumnName[0]);
        if (whereColumnName.Length != operation.Length || operation.Length != value.Length)
        {
            throw new Exception("输入不正确：" + "要查询的条件、条件操作符、条件值 的数量不一致！");
        }
        string query = "Select " + items[0];
        for (int i = 1; i < items.Length; i++)
        {
            query += "," + items[i];
        }

        query += " FROM " + tableName + " WHERE " + whereColumnName[0] + " " + operation[0] + " '" + value[0] + "'";
        for (int i = 1; i < whereColumnName.Length; i++)
        {
            query += " and " + whereColumnName[i] + " " + operation[i] + " '" + value[i] + "'";
        }
        return QuerySet(query);

    }

    private DataSet QuerySet(string sqlString)
    {
        if (mySqlConnection.State == ConnectionState.Open)
        {
            DataSet ds = new DataSet();
            try
            {
                MySqlDataAdapter mySqlAdapter = new MySqlDataAdapter(sqlString, mySqlConnection);
                mySqlAdapter.Fill(ds);
            }
            catch (Exception e)
            {
                throw new Exception("SQL:" + sqlString + "/n" + e.Message.ToString());
            }
            finally
            {
            }
            return ds;
        }
        return null;
    }
}


public class testlogin : MonoBehaviour {
    public GameObject LOG;
    public GameObject Player;
    public GameObject PPCO;
    public CameraFindPlayerScript CFPS;
    public BulletNum BN;

    public Text target_text;
    public InputField userNameInput;

    public InputField passwordInput;
    //提示用户登录信息

    //IP地址
    public string host;
    //端口号
    public string port;
    //用户名
    public string userName;
    //密码
    public string password;
    //数据库名称
    public string databaseName;
    //封装好的数据库类
    MySqlAccess mysql;


    // Use this for initialization
    void Start () {
        Button btn = LOG.GetComponentInChildren<Button>();
        mysql = new MySqlAccess(host, port, userName, password, databaseName);
        btn.onClick.AddListener(delegate () {
            onClickLOG();
        });
    }

    private void GetTabDown()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //accountInputField当前是否有焦点并且能够处理事件。
            if (userNameInput.isFocused)
            {
                //让passwordInputInputField能够处理事件
                passwordInput.Select();
            }
            //和上面反过来
            else if (passwordInput.isFocused)
            {
                userNameInput.Select();
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
        GetTabDown();
        if (Input.GetKeyDown(KeyCode.Return))
            onClickLOG();
    }


    void onClickLOG()
    {
        mysql.OpenSql();
        DataSet ds = mysql.Select("user_tbl", new string[] { "submission_date" }, new string[] { "`" + "user_name" + "`", "`" + "password" + "`" }, new string[] { "=", "=" }, new string[] { userNameInput.text, passwordInput.text });
        if (ds != null)
        {
            DataTable table = ds.Tables[0];

            if (table.Rows.Count > 0)
            {
                target_text.text = "登录成功";
                GameObject go = Instantiate(Player);
                go.transform.name = userNameInput.text;
                go.GetComponent<HealthDemo>().PPCO = PPCO;
                PlayerShoot PS = go.GetComponent<PlayerShoot>();
                PS.BN = BN;
                PS.CFPS = CFPS;
                PS.PPC = PPCO.GetComponent<PausePanelControl>();

                CFPS.enabled = true;
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                PPCO.SetActive(false);
                Destroy(this.gameObject);
            }
            else
            {
                target_text.text = "登录失败";
            }
        }
        mysql.CloseSql();

    }
}
