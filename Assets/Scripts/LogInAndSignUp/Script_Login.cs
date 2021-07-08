using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;


public class Script_Login : MonoBehaviour {
    private Thread _thread;
    string txt_ID;
    string txt_Psw;
    Text Txt_info;
    Connection conn;
    string filePath;
    Dictionary<string, string> recv;
    private string _username;
    private int _roomid;
    private bool _connected = false;
    private char[] sepreator = new char[] { '=', ';' };

    #region
    public string UserName
    {
        get
        {
            return _username;
        }
        set
        {
            this._username = value;
        }
    }

    public int RoomID
    {
        get
        {
            return _roomid;
        }
        set
        {
            this._roomid = value;
        }
    }

    private Dictionary<string, string> _LastQuery;

    public Dictionary<string, string> LastQuery
    {
        get
        {
            Debug.Log("requiring lastquery, status:" + (_LastQuery == null).ToString());
            return _LastQuery;
        }
        set
        {
            Debug.Log("setting lastquery, status:" + (value == null).ToString());
            _LastQuery = value;
        }

    }

    public bool Connected
    {
        get
        {
            return _connected;
        }
    }
    #endregion

    private Socket clientSocket;
    // Start is called before the first frame update


    void Connect()
    {
        IPAddress ip = IPAddress.Parse("10.21.32.43");
        //IPAddress ip = IPAddress.Parse("10.21.96.156");
        this.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        IAsyncResult result = clientSocket.BeginConnect(ip, 23456, null, null); //配置服务器IP与端口
        Debug.Log("连接中...");

        bool success = result.AsyncWaitHandle.WaitOne(2000, true);

        if (clientSocket.Connected)
        {
            clientSocket.EndConnect(result);
            Debug.Log("连接成功");
            _connected = true;
        }
        else
        {
            Debug.Log("连接失败");
            clientSocket.Close();
            _connected = false;
        }
    }

    


    // Use this for initialization
    void Start() {
        Txt_info = GameObject.Find("Txt_Info").GetComponent<Text>();
        filePath = Application.dataPath + "/" + "users.txt";
        conn = GameObject.Find("ConnectionObject").GetComponent<Connection>();
        //Connect();
        ///StartCoroutine(judge());

    }

	// Update is called once per frame
	void Update () {
		
	}

    /*
    private Dictionary<string, string> ReceiveData()
    {  // 这个函数被后台线程执行, 不断地在while循环中跑着
        Debug.Log("Entered ReceiveData function...");
        if (!_connected)  // stop the thread
            return null;
        int numberOfBytesRead = 0;
        while (_connected)
        {
            try
            {
                string recvStr = "";
                byte[] recvBytes = new byte[1024];
                int bytes;
                bytes = clientSocket.Receive(recvBytes, recvBytes.Length, 0);    //从服务器端接受返回信息 
                recvStr += Encoding.UTF8.GetString(recvBytes, 0, bytes);
                Dictionary<string, string> recvDict = new Dictionary<string, string>();
                recvDict.Add("_", recvStr);
                string[] p = recvStr.Split(sepreator);
                Debug.Log(p.Length + p.ToString());
                for (int i = 0; i < p.Length; i += 2)
                {
                    if (i + 2 >= p.Length) break;
                    Debug.Log("Dict: " + p[i] + ": " + p[i + 1]);
                    recvDict.Add(p[i], p[i + 1]);
                }
                Debug.Log(recvStr);
                LastQuery = recvDict;
                return recvDict;
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
            return null;
        }
    }
    */

    // ***********************************************************************
    public void Login()
    {
        txt_ID = GameObject.Find("IptF_ID/Text").GetComponent<Text>().text;
        txt_Psw = GameObject.Find("IptF_Psw/Text").GetComponent<Text>().text;
        if (txt_ID == "")
        {
            Txt_info.text = "请输入账号";
            return;
        }
        if (txt_Psw == "")
        {
            Txt_info.text = "请输入密码";
            return;
        }
        
        if (Check_Login(txt_ID, txt_Psw))
        {
            Txt_info.text = "登录成功";
            SceneManager.LoadScene("Desktop");
            //登陆成功
        }
        else
        {
            Txt_info.text = "账号或密码错误";
        }
    }

    

    /*
    IEnumerator judge()
    {
        string recvStr = "";
        Dictionary<string, string> recvDict = new Dictionary<string, string>();
        try
        {
            
            byte[] recvBytes = new byte[1024];
            int bytes;
            bytes = clientSocket.Receive(recvBytes, recvBytes.Length, 0);    //从服务器端接受返回信息 
            recvStr += Encoding.UTF8.GetString(recvBytes, 0, bytes);
            
            recvDict.Add("_", recvStr);
            string[] p = recvStr.Split(sepreator);
            Debug.Log(p.Length + p.ToString());
            for (int i = 0; i < p.Length; i += 2)
            {
                if (i + 2 >= p.Length) break;
                Debug.Log("Dict: " + p[i] + ": " + p[i + 1]);
                recvDict.Add(p[i], p[i + 1]);
            }
            Debug.Log(recvStr);
            LastQuery = recvDict;
            
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
        yield return recvDict;
    }
    
   */

    bool Check_Login(string id, string psw)
    {
        Dictionary<string, string> dd = new Dictionary<string, string>();
        dd.Add(id, psw);

        conn.EncodeAndSendMessageUser(dd);

        while (conn.NoQueryInQueue) {
            System.Threading.Thread.Sleep(100);
        }

        //StartMatch();

        var x = conn.PopQuery;

        if (x["ResponseType"] == "Success")
        {
            return true;
        }

        //System.Threading.Thread.Sleep(1000);


        // Dictionary<string, string>  x = conn.PopQuery;
        Debug.Log(x["ResponseType"]+"this is debug");
        /*
        try
        {
            string recvStr = "";
            byte[] recvBytes = new byte[1024];
            int bytes;
            bytes = clientSocket.Receive(recvBytes, recvBytes.Length, 0);    //从服务器端接受返回信息 
            recvStr += Encoding.UTF8.GetString(recvBytes, 0, bytes);
            Dictionary<string, string> recvDict = new Dictionary<string, string>();
            recvDict.Add("_", recvStr);
            string[] p = recvStr.Split(sepreator);
            Debug.Log(p.Length + p.ToString());

            if (p[1] == "Success")
            {
                return true;
            }


            Debug.Log(p[1]);
            for (int i = 0; i < p.Length; i += 2)
            {
                if (i + 2 >= p.Length) break;
                Debug.Log("Dict: " + p[i] + ": " + p[i + 1]);
                recvDict.Add(p[i], p[i + 1]);
            }
            Debug.Log(recvStr);
            LastQuery = recvDict;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }

    */


        //StartCoroutine(judge);
        /*
        string[] Users = File.ReadAllLines(filePath);
        for (int i = 0; i < Users.Length; i++)
        {
            string user_id = Users[i].Split(' ')[0];
            string user_psw = Users[i].Split(' ')[1];


            if (id == user_id && psw == user_psw)
            {
                return true;
            }
        }
        */
        return false;
    }
}
