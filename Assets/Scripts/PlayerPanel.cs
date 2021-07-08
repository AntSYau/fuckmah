using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


abstract public class PlayerPanel : MonoBehaviour
{
    protected Text _posText;
    protected HandCardList _handCarde;
    protected AwaitCardList _awaitCard;
    protected ListenCardList _listenCard;
    //protected ChooseFlowerPig _chooseFP;

    protected Image _infoHeadImage;
    protected Image _infoDealerImage;
    //protected Image _infoFlowerPigImage;
    protected Text _infoNameText;
    protected Text _infoGoldText;


    protected Card tempOutCard;
    protected Card tc;

    protected bool _isOnListenCard = false;
    protected bool _isHuCard = false;
    //protected bool _isOnChooseFlowerPig=false;
    protected int _myPosIndex;
    protected bool IsSelfTouch = false;

    public bool isPeng = false;
    public bool isHu = false;
    public int isGang = -1;//0是明杠，1是暗杠，2是加杠

    protected Connection conn;

    protected Dictionary<string, string> status;

    #region 属性方法

    public Text PosText
    {
        get
        {
            return _posText;
        }

        set
        {
            _posText = value;
        }
    }


    public Image InfoHeadImage
    {
        get
        {
            return _infoHeadImage;
        }

        set
        {
            _infoHeadImage = value;
        }
    }

    public HandCardList HandCarde
    {
        get
        {
            return _handCarde;
        }

        set
        {
            _handCarde = value;
        }
    }

    public AwaitCardList AwaitCard
    {
        get
        {
            return _awaitCard;
        }

        set
        {
            _awaitCard = value;
        }
    }

    public int MyPosIndex
    {
        get
        {
            return _myPosIndex;
        }

        set
        {
            _myPosIndex = value;
        }
    }

    public Image InfoDealerImage
    {
        get
        {
            return _infoDealerImage;
        }

        set
        {
            _infoDealerImage = value;
        }
    }

    public Text InfoNameText
    {
        get
        {
            return _infoNameText;
        }

        set
        {
            _infoNameText = value;
        }
    }

    public Text InfoGoldText
    {
        get
        {
            return _infoGoldText;
        }

        set
        {
            _infoGoldText = value;
        }
    }

    public ListenCardList ListenCard
    {
        get
        {
            return _listenCard;
        }

        set
        {
            _listenCard = value;
        }
    }

    public bool IsOnListenCard
    {
        get
        {
            return _isOnListenCard;
        }

        set
        {
            _isOnListenCard = value;
        }
    }



    /*public ChooseFlowerPig ChooseFP
    {
        get
        {
            return _chooseFP;
        }

        set
        {
            _chooseFP = value;
        }
    }

    public Image InfoFlowerPigImage
    {
        get
        {
            return _infoFlowerPigImage;
        }

        set
        {
            _infoFlowerPigImage = value;
        }
    }

    public bool IsOnChooseFlowerPig
    {
        get
        {
            return _isOnChooseFlowerPig;
        }

        set
        {
            _isOnChooseFlowerPig = value;
        }
    }*/

    public bool IsHuCard
    {
        get
        {
            return _isHuCard;
        }

        set
        {
            _isHuCard = value;
        }
    }

    #endregion

    private void Awake()
    {
        PosText = transform.Find("Pos").GetComponent<Text>();
        HandCarde = transform.Find("HandCardList").GetComponent<HandCardList>();
        AwaitCard = transform.Find("AwaitCardList").GetComponent<AwaitCardList>();
        if (transform.name == "Me")
        {
            //ChooseFP = transform.Find("ChooseFlowerPigPanel").GetComponent<ChooseFlowerPig>();
            ListenCard = transform.Find("ListenCardList").GetComponent<ListenCardList>();
        }
        InfoHeadImage = transform.Find("InfoPanel/Head").GetComponent<Image>();
        InfoDealerImage = transform.Find("InfoPanel/Dealer").GetComponent<Image>();
        //InfoFlowerPigImage = transform.Find("InfoPanel/FlowerPig").GetComponent<Image>();
        InfoNameText = transform.Find("InfoPanel/Name").GetComponent<Text>();
        InfoGoldText = transform.Find("InfoPanel/Gold/Text").GetComponent<Text>();
    }

    private void Start()
    {
        conn = GameObject.Find("ConnectionObject").GetComponent<Connection>();
    }

    public Card getOutCard()
    {
        return tc;
    }

    /// <summary>
    /// 听自己摸的牌
    /// </summary>
    /// <param name="card"></param>
    abstract public void ListenTouchCard(Card card);

    /// <summary>
    /// 听别人打出的牌
    /// </summary>
    /// <param name="card"></param>
    abstract public void ListenOutCard(Card card, Dictionary<string, string> status);

    public void PengCard()
    {
        IsOnListenCard = false;
        Debug.Log("arfhlaer Peng!!!!!, User "+MyPosIndex);
        GameAudio.instance.PlayaudioSourceAuto("man_peng0", Camera.main.transform.position);
        HandCarde.AddToHandCard(tempOutCard, tempOutCard.ImageName);
        HandCarde.MoveToCardTypeList(tempOutCard, 3);
        HandCarde.IsNoGetCard = true;
        MahjonManage._instance.DisappearNewOutCardS();//让服务器删掉刚出的牌
        MahjonManage._instance.SetCurrentOutCardIndex(MyPosIndex);//让服务器下一次让自己出牌
        // MahjonManage._instance.TabCurrentOutCard();
    }
    public void GangCard()
    {
        TimerPanel._instance.IsOnTimer = false;
        Debug.Log("arfhlaer Gang!!!!!, User " + MyPosIndex);
        GameAudio.instance.PlayaudioSourceAuto("man_gang1", Camera.main.transform.position);
        MahjonManage._instance.SetCurrentOutCardIndex(MyPosIndex);//让服务器下一次让自己出牌
        switch (isGang)//0是明杠，1是暗杠，2是加杠
        {
            case 0:
                IsOnListenCard = false;
                HandCarde.AddToHandCard(tempOutCard, tempOutCard.ImageName);
                HandCarde.MoveToCardTypeList(tempOutCard, 4);
                MahjonManage._instance.DisappearNewOutCardS();//让服务器删掉刚出的牌
                // MahjonManage._instance.TabCurrentOutCard();
                break;
            case 1:
                HandCarde.MoveToCardTypeList(tempOutCard, 4);
                MahjonManage._instance.TabCurrentOutCard();//自摸的杠需要通知服务器切牌
                break;
            case 2:
                HandCarde.MoveToCardTypeList(tempOutCard, 1);
                MahjonManage._instance.TabCurrentOutCard();//自摸的杠需要通知服务器切牌
                break;
        }
    }
    public void HuCard()
    {
        Debug.Log("arfhlaer Hu!!!!!, User " + MyPosIndex);
        IsHuCard = true;
        GameAudio.instance.PlayaudioSourceAuto("man_hu0", Camera.main.transform.position);
        Debug.Log(transform.name + " 胡牌了 不打了");
        MahjonManage._instance.EndMatch();
        MahjonManage._instance.TabCurrentOutCard();//直接需要通知服务器切牌
        IsOnListenCard = false;
        MahjonManage._instance.SetCurrentOutCardIndex(MyPosIndex);//让服务器下一次让自己出牌
    }
    public void GuoCard()
    {
        Debug.Log("arfhlaer Guo!!!!!, User " + MyPosIndex);
        GameAudio.instance.PlayaudioSourceRole("ui_click");
        IsOnListenCard = false;
    }

    public void EndListenCardButton()
    {
        Debug.Log("EndListenCardButton, " + MyPosIndex);
        if (ListenCard != null)
        {
            ListenCard.PengButton.gameObject.SetActive(false);
            ListenCard.GangButton.gameObject.SetActive(false);
            ListenCard.HuButton.gameObject.SetActive(false);
            ListenCard.GuoButton.gameObject.SetActive(false);
        }
    }
}
