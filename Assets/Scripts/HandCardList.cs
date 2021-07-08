using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

abstract public class HandCardList : MonoBehaviour
{

    protected List<HandCard> wanCardList = new List<HandCard>();
    protected List<HandCard> tiaoCardList = new List<HandCard>();
    protected List<HandCard> tongCardList = new List<HandCard>();
    protected List<HandCard> ziCardList = new List<HandCard>();

    protected List<HandCard> _outCardList = new List<HandCard>();
    protected List<Card> _cardTypeList = new List<Card>();

    protected List<Transform> cardTypePoint = new List<Transform>();
    protected Dictionary<string, Transform> cardTPD = new Dictionary<string, Transform>();
    protected Dictionary<string, Sprite> headCSD = new Dictionary<string, Sprite>();
    protected Dictionary<string, Sprite> outCSD = new Dictionary<string, Sprite>();

    protected GameObject currentObj = null;
    protected HandCard newAddHC;
    protected HandCard newOutHC;
    protected Transform newAddPoint;


    protected Transform gridListPoint;
    protected Transform outGridListPoint;

    protected Vector3 movePos;
    //protected string _currentFlowerPig="";
    protected bool _isOutCard = false;
    protected bool _isNoGetCard = false;

    public HandCard handCard;

    protected Connection conn;

    #region 属性方法
    public bool IsOutCard
    {
        get
        {
            return _isOutCard;
        }

        set
        {
            _isOutCard = value;
        }
    }

    public List<HandCard> OutCardList
    {
        get
        {
            return _outCardList;
        }

        set
        {
            _outCardList = value;
        }
    }

    public List<Card> CardTypeList
    {
        get
        {
            return _cardTypeList;
        }
        set
        {
            _cardTypeList = value;
        }

    }

    public bool IsNoGetCard
    {
        get
        {
            return _isNoGetCard;
        }

        set
        {
            _isNoGetCard = value;
        }
    }
    #endregion

    private void Awake()
    {
        movePos = new Vector3(0, 20, 0);
        gridListPoint = transform.Find("GridList").GetComponent<Transform>();
        outGridListPoint = transform.Find("OutGridList").GetComponent<Transform>();
        Transform[] tfArray = transform.Find("CardTypeList/Grid").GetComponentsInChildren<Transform>();
        //handCard.SetSize();
        foreach (Transform tf in tfArray)
        {
            cardTypePoint.Add(tf);
        }
        if (transform.parent.name == "Me")
        {
            newAddPoint = transform.Find("NewAdd").GetComponent<Transform>();
        }
    }

    private void Start()
    {
        conn = GameObject.Find("ConnectionObject").GetComponent<Connection>();
        AddSpriteToArray();
    }

    void AddSpriteToArray()
    {
        string headCardPath = "";
        string outCardPath = "";
        switch (transform.parent.name)
        {
            case "Me":
                headCardPath = "MeHeadCard";
                outCardPath = "DownOutCard";
                break;
            case "Next":
                headCardPath = "OtherHeadCard";
                outCardPath = "RightOutCard";
                break;
            case "Across":
                headCardPath = "OtherHeadCard";
                outCardPath = "MeHeadCard";//把我手中显示的牌旋转180°就是对面出牌的显示效果
                break;
            case "Last":
                headCardPath = "OtherHeadCard";
                outCardPath = "LeftOutCard";
                break;
        }
        Sprite[] spArray;
        spArray = Resources.LoadAll<Sprite>("UIs/Card/" + headCardPath);

        foreach (Sprite sp in spArray)
        {
            headCSD.Add(sp.name, sp);
        }
        spArray = Resources.LoadAll<Sprite>("UIs/Card/" + outCardPath);
        foreach (Sprite sp in spArray)
        {
            outCSD.Add(sp.name, sp);
        }
    }

    public void DisappearNewOutCard()
    {
        newOutHC.HandCardImage.color = Color.gray;
    }

    public abstract bool AddToHandCard(Card card, string imangName);

    abstract public List<Card> GetCardS();

    abstract public List<HandCard> GetHeadCardS();

    abstract public List<Card> GetCardTpyeList();

    abstract public int GetHeadCardCount();

    abstract public void AddToOutCardList(Card card);

    abstract public void MoveToCardTypeList(Card card, int count);

    abstract public void AutoOutCard();

    abstract public void HandCardSort();

    abstract public void MarksHandCardSort(Dictionary<string, List<HandCard>> dicList);

    abstract public void RemoveHandCard(HandCard hc);
}
