using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Script_Regist : MonoBehaviour {
    string txt_ID;
    string txt_Psw;
    string txt_Psw_2;
    bool b_IfLoginSuccess;
    Text Txt_info;
    string filePath;
    StreamWriter sw;
    Connection conn;

    // Use this for initialization
    void Start () {
        Txt_info = GameObject.Find("Txt_Info").GetComponent<Text>();
        //filePath = Application.dataPath + "/" + "users.txt";
        conn = GameObject.Find("ConnectionObject").GetComponent<Connection>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Regist()
    {
        txt_ID = GameObject.Find("IptF_ID/Text").GetComponent<Text>().text;
        txt_Psw = GameObject.Find("IptF_Psw/Text").GetComponent<Text>().text;
        txt_Psw_2 = GameObject.Find("IptF_Psw_2/Text").GetComponent<Text>().text;

        if(txt_ID == "")
        {
            Txt_info.text = "请输入账号";
            return;
        }
        if (txt_Psw == "")
        {
            Txt_info.text = "请输入密码";
            return;
        }
        if (txt_Psw_2 == "")
        {
            Txt_info.text = "请输入密码";
            return;
        }

        if (txt_Psw != txt_Psw_2)
        {
            Txt_info.text = "两次输入的密码不一致";
        }else
        {
            if (CheckID(txt_ID,txt_Psw) == false)
            {
                Txt_info.text = "已存在相同账号";
            }
            else
            {
                Txt_info.text = "注册成功";
                //
                //WriteUserInfo(txt_ID, txt_Psw);
            }
        }
    }

    void WriteUserInfo(string id, string psw)
    {
        Dictionary<string, string> dd = new Dictionary<string, string>();
        dd.Add(id, psw);

        conn.EncodeAndSendMessageRegister(dd);



        if (!File.Exists(filePath))
        {
            sw = File.CreateText(filePath);
        }

        sw = File.AppendText(filePath);
        sw.WriteLine(id + " " + psw);
        sw.Close();
    }

    bool CheckID(string id,string psw)
    {
        Dictionary<string, string> dd = new Dictionary<string, string>();
        dd.Add(id, psw);

        conn.EncodeAndSendMessageRegister(dd);

        while (conn.NoQueryInQueue)
        {
            System.Threading.Thread.Sleep(100);
        }


        var x = conn.PopQuery;

        if (x["ResponseType"] == "Success")
        {
            return true;
        }
        else
        {
            return false;
        }

        return false;


        /*

        string[] Users = File.ReadAllLines(filePath);
        for (int i = 0; i < Users.Length; i++)
        {
            string user_id = Users[i].Split(' ')[0];
            //string user_psw = Users[i - 1].Split( )[1];
            if (id == user_id)
            {
                return false;
            }
        }
        return true;
        */
    }
}
