using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Dict = System.Collections.Generic.Dictionary<string, string>;
using Unity.Jobs;
using Unity.Collections;

public class MahjonManage : MonoBehaviour
{
    public static MahjonManage _instance;
    private FanCalculate f = new FanCalculate();

    private MahjonAlgorithm _algorithm = new MahjonAlgorithm();

    private static string[] w = new string[9]
        {
            "mingmah_1","mingmah_2","mingmah_3","mingmah_4","mingmah_5",
                "mingmah_6","mingmah_7","mingmah_8","mingmah_9",
        };
    private static string[] ti = new string[9]
        {
            "mingmah_11","mingmah_12","mingmah_13","mingmah_14","mingmah_15",
                "mingmah_16","mingmah_17","mingmah_18","mingmah_19",
        };
    private static string[] to = new string[9]
        {
            "mingmah_21","mingmah_22","mingmah_23","mingmah_24","mingmah_25",
                "mingmah_26","mingmah_27","mingmah_28","mingmah_29",
        };
    private static string[] zi = new string[7]
            {"zi1","zi2","zi3","zi4","zi5","zi6","zi7"};
    private static string[] soundwan = new string[9]
        { "man11", "man12", "man13", "man14","man15",
            "man16", "man17", "man18", "man19" };
    private static string[] soundtiao = new string[9]
        { "man21", "man22", "man23", "man24","man25",
            "man26", "man27", "man28", "man29" };
    private static string[] soundtong = new string[9]
        { "man31", "man32", "man33", "man34","man35",
            "man36", "man37", "man38", "man39" };
    private static string[] soundzi = new string[7]
            {"zi1","zi2","zi3","zi4","zi5","zi6","zi7"};
    private static string[] zi_name = new string[7]
    { "东风", "西风", "南风", "北风", "红中", "发财", "白板" };

    private List<Card> _cards = new List<Card>();

    private PlayerPanel[] players;
    private StartMatch startMatch;

    private int dealer;
    private int inWhoGetCard;
    private int inWhoGetCardIndex;

    private int currentOutCardIndex;

    private Transform biaojiPoint;
    private Transform newOutPoint;

    Connection conn;
    Dict _recv;

    #region 属性方法
    Dict recv
    {
        get
        {
            return _recv;
        }
        set
        {
            _recv = value;
            Debug.Log("value changed"+(value==null).ToString());
        }
    }


    public MahjonAlgorithm Algorithm
    {
        get
        {
            return _algorithm;
        }

        set
        {
            _algorithm = value;
        }
    }


    #endregion

    private void Awake()
    {
        _instance = this;
        players = GetComponentsInChildren<PlayerPanel>();
        startMatch = transform.Find("StartButton").GetComponent<StartMatch>();
        biaojiPoint = transform.Find("Biaoji").GetComponent<Transform>();
    }

    private void Start()
    {
        conn = GameObject.Find("ConnectionObject").GetComponent<Connection>();
    }

    private void Update()
    {
        if (newOutPoint != null)
        {
            biaojiPoint.gameObject.SetActive(true);
            biaojiPoint.position = new Vector3(newOutPoint.position.x, newOutPoint.position.y + 30, newOutPoint.position.z);
        }
    }

    public void StartMatch()
    {
        StartCoroutine(_StartMatch());
    }

    private IEnumerator _StartMatch()
    {
        Debug.Log("Start Monitoring");
        Debug.Log("MM:128 Listening");
        while (conn.NoQueryInQueue)
        {
            yield return new WaitForSecondsRealtime(0.1f);
        }
        Debug.Log("Start Match");
        GameAudio.instance.PlayaudioSourceUI("start_game");
        recv = conn.PopQuery;
        Debug.Log("Received " + recv["ResponseType"]);
        StartCoroutine(InitMatch());
        yield break;
    }
    private void InitCards()
    {
        GameAudio.instance.PlayaudioSourceBG("backMusic01");
    }

    public static Card ParseCard(string card)
    {
        int cardNum = int.Parse(card);
        Debug.Log("AI出牌啦!!!!!!!!!" + card);
        int cardCat = cardNum / 10;
        int cardSize = cardNum % 10;
        string cardName = "", cardMark = "", imageName = "", soundName = "";
        switch (cardCat)
        {
            case 1:
                cardName = cardSize + "万";
                cardMark = "万";
                imageName = w[cardSize - 1];
                soundName = soundwan[cardSize - 1];
                break;
            case 2:
                cardName = cardSize + "条";
                cardMark = "条";
                imageName = ti[cardSize - 1];
                soundName = soundtiao[cardSize - 1];
                break;
            case 3:
                cardName = cardSize + "筒";
                cardMark = "筒";
                imageName = to[cardSize - 1];
                soundName = soundtong[cardSize - 1];
                break;
            case 4:
                cardName = zi_name[cardSize - 1];
                cardMark = "字";
                soundName = soundzi[cardSize - 1];
                imageName = zi[cardSize - 1];
                cardSize = cardSize * 8 + 1;
                break;
        }
        return new Card(cardName, cardSize, cardMark, imageName, soundName);
    }


    public static string EncodeCard(Card card)
    {
        int ret=-1;
        switch(card.CardMark) {
            case "万":
                ret = 10 + card.Size;
                break;
            case "条":
                ret = 20 + card.Size;
                break;
            case "筒":
                ret = 30 + card.Size;
                break;
            case "字":
                ret = 40 + (card.Size-1)/8;
                break;
        }
        return ret.ToString();
    }


    private void SetSeat()
    {
        Debug.Log(recv == null);
        int pos = int.Parse(recv["Pos"]);
        //1=东，2=南，3=西。4=北  0是我 1是下家 2 是对家 3是上家
        switch (pos)
        {
            case 1:
                players[0].PosText.text = "E";
                players[0].MyPosIndex = 1;
                players[1].PosText.text = "S";
                players[1].MyPosIndex = 2;
                players[2].PosText.text = "W";
                players[2].MyPosIndex = 3;
                players[3].PosText.text = "N";
                players[3].MyPosIndex = 4;
                break;
            case 2:
                players[0].PosText.text = "S";
                players[0].MyPosIndex = 2;
                players[1].PosText.text = "W";
                players[1].MyPosIndex = 3;
                players[2].PosText.text = "N";
                players[2].MyPosIndex = 4;
                players[3].PosText.text = "E";
                players[3].MyPosIndex = 1;
                break;
            case 3:
                players[0].PosText.text = "W";
                players[0].MyPosIndex = 3;
                players[1].PosText.text = "N";
                players[1].MyPosIndex = 4;
                players[2].PosText.text = "E";
                players[2].MyPosIndex = 1;
                players[3].PosText.text = "S";
                players[3].MyPosIndex = 2;
                break;
            case 4:
                players[0].PosText.text = "N";
                players[0].MyPosIndex = 4;
                players[1].PosText.text = "E";
                players[1].MyPosIndex = 1;
                players[2].PosText.text = "S";
                players[2].MyPosIndex = 2;
                players[3].PosText.text = "W";
                players[3].MyPosIndex = 3;
                break;
        }
        for (int i = 0; i < 4; i++)
        {
            // players[i].name = recv["dir_" + players[i].MyPosIndex];
        }
    }

    /// <summary>
    ///我们在这里用本地模式随机获得庄家
    /// </summary>
    private void SetDealer()
    {
        // TODO: 指定一个装甲
        dealer = int.Parse(recv["Zhuang"]); //1=东，2=南，3=西。4=北
    }

    /// <summary>
    /// 获取当前的庄家并播放骰子动画
    /// </summary>
    private void SetHitPoint()
    {
        //通过摇骰子设置拿牌位置
        // TODO: 服务器返回两个骰子
        int touziOne = int.Parse(recv["Dice1"]);
        int touziTwo = int.Parse(recv["Dice2"]);

        Touzi._instancce.TouziOne.LastImage = Resources.Load("UIs/Touzi/touzi" + touziOne, typeof(Sprite)) as Sprite;
        Touzi._instancce.TouziTwo.LastImage = Resources.Load("UIs/Touzi/touzi" + touziTwo, typeof(Sprite)) as Sprite;
        GameAudio.instance.PlayaudioSourceRole("touzi");
        Touzi._instancce.PlayerAllAnimation();

        //先找到庄家然后从庄家开始循环骰子点 结束后在获得在谁玩家手上拿牌
        int who = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (dealer == players[i].MyPosIndex)
            {
                who = i++;
                for (int j = 0; j < touziOne + touziTwo; j++)
                {
                    who--;
                    if (who < 0)
                    {
                        who = 3;
                    }
                }
                break;
            }
        }
        inWhoGetCard = who;
        if (touziOne > touziTwo)
        {
            inWhoGetCardIndex = touziTwo;
        }
        else
        {
            inWhoGetCardIndex = touziOne;
        }
    }
    private void CardDesktopShow()
    {
        players[0].AwaitCard.AddCardInTable("mingmah_2", players[0].MyPosIndex);
        players[1].AwaitCard.AddCardInTable("mingmah_1", players[1].MyPosIndex);
        players[2].AwaitCard.AddCardInTable("mingmah_2", players[2].MyPosIndex);
        players[3].AwaitCard.AddCardInTable("mingmah_1", players[3].MyPosIndex);
    }
    private bool CardDesktopShowOff()
    {
        //摸牌应该逆时针摸
        for (int i = 0; i < players.Length; i++)
        {
            if (inWhoGetCardIndex > players[inWhoGetCard].AwaitCard.IndexDownList.Count)
            {
                inWhoGetCard--;
                if (inWhoGetCard < 0)
                {
                    inWhoGetCard = 3;
                }
                inWhoGetCardIndex = 1;
            }
            bool up = players[inWhoGetCard].AwaitCard.IndexUpList[inWhoGetCardIndex - 1].enabled;
            bool down = players[inWhoGetCard].AwaitCard.IndexDownList[inWhoGetCardIndex - 1].enabled;
            if (up == true)
            {
                players[inWhoGetCard].AwaitCard.IndexUpList[inWhoGetCardIndex - 1].enabled = false;
                return true;
            }
            else if (down == true)
            {
                players[inWhoGetCard].AwaitCard.IndexDownList[inWhoGetCardIndex - 1].enabled = false;
                return true;
            }

            inWhoGetCardIndex++;
        }
        return false;
    }
    private int InitPlayersData()
    {
        //如果联网的话 其实我们只需要设置自己的牌就行了 你的对手直接显示手牌界面就行了
        //生成设置全部玩家的牌
        // 把下面换成服务器发牌

        int cout;
        int zhuang = 0;
        string[] cards = recv["HandCardList"].Split(',');
        foreach(var _card in cards) {
            var __card = ParseCard(_card);
            AssignCard(players[0], __card);
        }
        for (int i = 1; i < players.Length; i++)
        {
            cout = 13;
            if (dealer == players[i].MyPosIndex)
            {
                zhuang = i;
                cout = 14;
            }
            for (int j = 0; j < cout; j++)
            {
                AssignCard(players[i], ParseCard("11"));
            }
        }
        //第一次庄家不摸牌
        currentOutCardIndex = players[zhuang].MyPosIndex;
        players[zhuang].InfoDealerImage.enabled = true;
        players[zhuang].HandCarde.IsNoGetCard = true;
        return zhuang;
    }
    private Card AssignCard(PlayerPanel player, Card card)
    {
        Debug.Log("Assign Card, " + player.name);
        switch (player.name)
        {
            case "Me":
                player.HandCarde.AddToHandCard(card, card.ImageName);
                break;
            case "Next":
                player.HandCarde.AddToHandCard(card, "hand_right");
                break;
            case "Across":
                player.HandCarde.AddToHandCard(card, "hand_top");
                break;
            case "Last":
                player.HandCarde.AddToHandCard(card, "hand_left");
                break;
        }
        CardDesktopShowOff();
        return card;
    }

    private Card newAssignCard(PlayerPanel player, Card card)
    {
        Debug.Log("Assign Card, " + player.name);
        switch (player.name)
        {
            case "Me":
                player.HandCarde.AddToHandCard(card, card.ImageName);
                break;
        }
        CardDesktopShowOff();
        return card;
    }

    public void SetBiaojiPoint(Transform outtPoint)
    {
        this.newOutPoint = outtPoint;
    }
    public void StartTimer(int who)
    {
        //有些时候我们调用的时候不需要摸牌（碰牌的情况下）
        if (players[who].HandCarde.IsNoGetCard)
        {
            Debug.Log("Start timer!!" + players[who].name
                + ", isnogetcard: " + players[who].HandCarde.IsNoGetCard);
            players[who].HandCarde.IsNoGetCard = false;
            TimerPanel._instance.UpdateTimer(who);
            players[who].HandCarde.IsOutCard = true;
            if (players[who].name != "Me")
            {
                players[who].HandCarde.AutoOutCard();
            }
            else
            {
                Dictionary<string, string> message = new Dictionary<string, string>();
                message.Add("Peng", "0");
                message.Add("Gang", "-1");
                message.Add("Hu", "0");
                message.Add("RequestType", "Decision");
                conn.EncodeAndSendMessage(message,"StartTimer:424,"+players[who].MyPosIndex);
            }
        }
        else
        {
            StartCoroutine(ParseAssignOut(who));
        }
    }

    IEnumerator ParseAssignOut(int who)
    {
        Debug.Log("ParseAssignOut,"+who);
        yield return new WaitForEndOfFrame();
        Card _card = ParseCard("11");
        if (who == 0)
        {
            // 把这样的代码替换成下面的代码：recv = conn.GetMessageAndDecode();
            Debug.Log("MM:439 Listening");
            while (conn.NoQueryInQueue)
            {
                yield return new WaitForEndOfFrame();
            }
            recv = conn.PopQuery;
            while (recv["ResponseType"]!="InCard")
            {
                while (conn.NoQueryInQueue)
                {
                    yield return new WaitForEndOfFrame();
                }
                recv = conn.PopQuery;
            }
            Debug.Log(recv["_"]);
            Debug.Log("Received " + recv["ResponseType"]);
            _card = ParseCard(recv["InCard"]);
        }
        Card card = newAssignCard(players[who], _card);
        if (card != null)
        {
            TimerPanel._instance.UpdateTimer(who);
            players[who].HandCarde.IsOutCard = true;
            players[who].ListenTouchCard(card);
        }
        else
        {
            Debug.Log("牌都打完了哦 牌局结束了");
            EndMatch();
        }
    }

    public void OverTimer()
    {
        //我们在这里开始检测出牌顺序和监听 1 是自己 2 是下家 3 是对家 4 是上家

        if (currentOutCardIndex == players[0].MyPosIndex)
        {
            players[0].HandCarde.AutoOutCard();
        }
    }
    public void SetCurrentOutCardIndex(int index)
    {
        currentOutCardIndex = index - 1;
        Debug.Log("Setting Next OutCard Index to " + index);
    }
    /// <summary>
    /// 有玩家需要刚出的牌并自己重新创建了一个所以我们服务器来判断谁刚出的那个牌并让他删掉
    /// </summary>
    public void DisappearNewOutCardS()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].MyPosIndex == currentOutCardIndex)
            {
                players[i].HandCarde.DisappearNewOutCard();
                break;
            }
        }
    }
    public void TabCurrentOutCard(int count = 4)
    {
        currentOutCardIndex++;
        if (currentOutCardIndex > 4)
        {
            currentOutCardIndex = 1;
        }
        Debug.Log("TabCurrentOutCard, " + currentOutCardIndex);
        //我们在这里开始检测出牌顺序和监听 1 是自己 2 是下家 3 是对家 4 是上家
        int who = -1;
        for (int i = 0; i < players.Length; i++)
        {
            if (currentOutCardIndex == players[i].MyPosIndex)
            {
                if (!players[i].IsHuCard)
                {
                    if (players[i].transform.name == "Me")
                    {
                        //隐藏我激活的按钮
                        players[0].EndListenCardButton();
                    }
                    who = i;
                    break;
                }
                else
                {
                    //f.get(players[i].HandCarde , players[i].getOutCard());
                    Debug.Log("有一位玩家胡牌了 游戏结束了哦");
                    f.SetParameters(players[i].HandCarde, players[i].getOutCard());
                    f.Calculate();
                    f.Set();
                    EndMatch();
                    return;
                    //TabCurrentOutCard(count-1);//这个玩家已经糊牌就切换下一个
                }
            }
        }
        if (who != -1)
        {
            Debug.Log("This Player ==" + players[who].transform.name);
            StartTimer(who);
        }
        /*else
        {
            Debug.Log("三位玩家已经胡牌 游戏结束了哦");
            EndMatch();
        }*/
    }

    public void EndMatch()
    {
        GameSceneLoad.LoadScene(SceneName.EedMatch);
    }

    /// <summary>
    /// 给其他玩家发送听牌信息
    /// </summary>
    /// <param name="card"></param>
    public void StartOutNewCard(Card handCard)
    {
        Debug.Log("StartOutNewCard");
        //隐藏我激活的按钮
        players[0].EndListenCardButton();
        TimerPanel._instance.IsOnTimer = false;
        GameAudio.instance.PlayaudioSourceUI("out_card");
        GameAudio.instance.PlayaudioSourceAuto(handCard.SoundName, Camera.main.transform.position);
        StartCoroutine(EndOutNewCard(handCard));
    }
    /// <summary>
    /// 限制 最少1秒出牌听牌時間 7秒后强制切换牌 
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    IEnumerator EndOutNewCard(Card handCard)
    {
        Debug.Log("EndOutNewCard");
        float time = Time.time;
        if (players[0].MyPosIndex != currentOutCardIndex && players[0].IsHuCard == false)//现在出牌的这个不会听牌
        {
            Debug.LogError("Calling User to ListenOutCard");
            Debug.Log(players[0].name == "Me");
            players[0].ListenOutCard(handCard, null);
        }
        else if (players[0].MyPosIndex == currentOutCardIndex)
        {
            Dictionary<string, string> message = new Dictionary<string, string>();
            message.Add("Peng", "0");
            message.Add("RequestType", "Decision");
            message.Add("Gang", "-1");
            message.Add("Hu", "0");
            conn.EncodeAndSendMessage(message);
        }
        while (conn.NoQueryInQueue) yield return new WaitForSecondsRealtime(0.1f);
        var x = conn.PopQuery;
        while (x["ResponseType"] == "OutCard")
        {
            while (conn.NoQueryInQueue) yield return new WaitForSecondsRealtime(0.1f);
            x = conn.PopQuery;
        }
        Debug.Log("NoAction or Desition: " + x["ResponseType"]);
        for (int i = 1; i < players.Length; i++)
        {
            players[i].ListenOutCard(handCard, x);
        }
        while(players[0].IsOnListenCard && Time.time-time < 6f)
        {
            yield return new WaitForEndOfFrame();
        }
        if (players[0].IsOnListenCard)
        {
            players[0].IsOnListenCard = false;
            players[0].EndListenCardButton();
            Dictionary<string, string> message = new Dictionary<string, string>();
            message.Add("Peng", "0");
            message.Add("RequestType", "Decision");
            message.Add("Gang", "-1");
            message.Add("Hu", "0");
            conn.EncodeAndSendMessage(message, "EndOutNewCard:618," + players[0].MyPosIndex);
        }
        TabCurrentOutCard();
    }

    IEnumerator InitMatch()
    {
        InitCards();
        SetSeat();//设置自己的座位标志
        SetDealer();//设置谁是庄家
        CardDesktopShow();//打开显示所有麻将
        Touzi._instancce.TouziShow(true);
        SetHitPoint();//通过丢骰子设置摸牌的位置
        yield return new WaitForSeconds(3f);
        Touzi._instancce.TouziShow(false);
        int zhuang = InitPlayersData();//初始所有玩家的数据
        players[0].HandCarde.HandCardSort();//给自己手上的牌排序
        StartTimer(zhuang);//开始计时器出牌
    }
}
