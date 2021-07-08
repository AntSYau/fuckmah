using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;

public class Connection : MonoBehaviour
{
    private string _username;
    private int _roomid;

    private bool _connected = false;

    private const int BUFFER_SIZE = 1024;
    private static byte[] buffer;

    private ConcurrentQueue<Dictionary<string,string>> _queries = new ConcurrentQueue<Dictionary<string, string>>();

    public bool NoQueryInQueue {
        get
        {
            return _queries.IsEmpty;
        }
    }

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

    public Dictionary<string, string> PopQuery {
        get
        {
            Dictionary<string, string> _;
            var x = "";
            foreach(var __ in _queries)
            {
                x+=__["ResponseType"];
            }
            Debug.Log("before pop, there are " + _queries.Count + " queries, they are " + x);
            _queries.TryDequeue(out _);
            Debug.Log("requiring lastquery, Type:" + _["ResponseType"]);
            return _;
        }
        set
        {
            Debug.Log("setting lastquery, status:" + (value == null).ToString());
            _queries.Enqueue(value);
        }
    }

    public Dictionary<string, string> TopQuery
    {
        get
        {
            Dictionary<string, string> _;
            _queries.TryPeek(out _);
            Debug.Log("requiring lastquery, Type:" + _["ResponseType"]);
            return _;
        }
        set
        {
            Debug.Log("setting lastquery, status:" + (value == null).ToString());
            _queries.Enqueue(value);
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
    void Start()
    {
        UserName = UnityEngine.Random.Range(100000, 999999).ToString();
        Connect();
        Receive();
    }

    void Connect()
    {
        // IPAddress ip = IPAddress.Parse("10.21.32.43");
        IPAddress ip = IPAddress.Parse("10.21.96.156");
        // IPAddress ip = IPAddress.Parse("10.21.50.3");
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

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ConnectionObject");
        if (objs.Length>1)
        {
            Debug.Log("not initing new connection");
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EncodeAndSendMessage(Dictionary<string,string> message, string DebugType="none")
    {
        try
        {
            string sendMessage = "PlayerName=" + UserName
                + ";RoomID=" + this.RoomID + ";debug=" + DebugType + ";";
            foreach (var m in message) 
            {
                sendMessage += m.Key + "=" + m.Value + ";";
            }
            sendMessage += "\n";
            Debug.Log(sendMessage);
            clientSocket.Send(Encoding.UTF8.GetBytes(sendMessage));
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
        }
    }

    public void EncodeAndSendMessageRegister(Dictionary<string, string> message)
    {
        try
        {
            string sendMessage = "RequestType=Register;Username=";
            foreach (var m in message)
            {
                sendMessage += m.Key + ";Password=" + m.Value + ";";
            }
            sendMessage += "\n";
            Debug.Log(sendMessage);
            clientSocket.Send(Encoding.UTF8.GetBytes(sendMessage));
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }
    public void EncodeAndSendMessageUser(Dictionary<string, string> message)
    {
        try
        {
            string sendMessage = "RequestType=Login;Username=";
            foreach (var m in message)
            {
                sendMessage += m.Key + ";Password=" + m.Value + ";";
            }
            sendMessage += "\n";
            Debug.Log(sendMessage);
            clientSocket.Send(Encoding.UTF8.GetBytes(sendMessage));
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

        public void Receive()
    {
        while (!clientSocket.Connected)
            Connect();

        buffer = new byte[BUFFER_SIZE];
        try
        {
            clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, new AsyncCallback(Receive_Callback), clientSocket);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void Receive_Callback(IAsyncResult ar)
    {
        if (!clientSocket.Connected)
        {
            return;
        }

        int read = clientSocket.EndReceive(ar);
        if (read > 0)
        {
            string responses = Encoding.UTF8.GetString(buffer);
            Debug.Log("Recv: "+responses);  //接收到的消息内容
            Dictionary<string, string> recvDict = new Dictionary<string, string>();
            recvDict.Add("_", responses);
            string[] p = responses.Split(sepreator);
            var pp = "";
            foreach(var ppp in p) {
                pp += ppp;
            }
            Debug.Log("Parse: " + pp);
            for (int i = 0; i < p.Length; i += 2)
            {
                if (i + 2 >= p.Length) break;
                recvDict.Add(p[i], p[i + 1]);
            }
            PopQuery = recvDict;
            Receive();
        }
    }
}
