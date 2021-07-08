using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ListenCardList : MonoBehaviour
{
    private Button _pengButton;
    private Button _gangButton;
    private Button _huButton;
    private Button _guoButton;
    private Button _chiButton;

    protected Connection conn;

    #region 属性方法
    public Button PengButton
    {
        get
        {
            return _pengButton;
        }

        set
        {
            _pengButton = value;
        }
    }

    public Button GangButton
    {
        get
        {
            return _gangButton;
        }

        set
        {
            _gangButton = value;
        }
    }

    public Button HuButton
    {
        get
        {
            return _huButton;
        }

        set
        {
            _huButton = value;
        }
    }

    public Button GuoButton
    {
        get
        {
            return _guoButton;
        }

        set
        {
            _guoButton = value;
        }
    }

    public Button ChiButton
    {
        get
        {
            return _chiButton;
        }

        set
        {
            _chiButton = value;
        }
    }
    #endregion
    private void Awake()
    {
        PengButton = transform.Find("Grid/Peng").GetComponent<Button>();
        GangButton = transform.Find("Grid/Gang").GetComponent<Button>();
        HuButton = transform.Find("Grid/Hu").GetComponent<Button>();
        GuoButton = transform.Find("Grid/Guo").GetComponent<Button>();
        ChiButton = transform.Find("Grid/Chi").GetComponent<Button>();
    }
    private void Start()
    {
        conn = GameObject.Find("ConnectionObject").GetComponent<Connection>();
        Dictionary<string, string> message = new Dictionary<string, string>();
        message.Add("Peng", "0");
        message.Add("RequestType", "Decision");
        message.Add("Gang", "-1");
        message.Add("Hu", "0");
        PengButton.onClick.AddListener(delegate()
        {
            PengButton.gameObject.SetActive(false);
            message["Peng"] = "1";
            conn.EncodeAndSendMessage(message); 
            message["Peng"] = "0";
            transform.parent.SendMessage("PengCard");
        });
        GangButton.onClick.AddListener(delegate ()
        {
            GangButton.gameObject.SetActive(false);
            int x = transform.parent.GetComponent<PlayerPanel>().isGang;
            message["Gang"] = x.ToString();
            conn.EncodeAndSendMessage(message);
            message["Gang"] = "-1";
            transform.parent.SendMessage("GangCard");
        });
        HuButton.onClick.AddListener(delegate ()
        {
            HuButton.gameObject.SetActive(false);
            message["Hu"] = "1";
            conn.EncodeAndSendMessage(message);
            message["Hu"] = "0";
            transform.parent.SendMessage("HuCard");
        });
        GuoButton.onClick.AddListener(delegate ()
        {
            GuoButton.gameObject.SetActive(false);
            conn.EncodeAndSendMessage(message);
            transform.parent.SendMessage("GuoCard");
        });
        ChiButton.onClick.AddListener(delegate ()
        {
            transform.parent.SendMessage("ChiCard");
        });
    }
   
}
