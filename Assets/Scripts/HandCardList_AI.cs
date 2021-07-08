using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Diagnostics;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;
using Unity.Jobs;

public class HandCardList_AI : HandCardList, IPointerClickHandler
{

    void AddSpriteToArray()
    {
        string headCardPath = "";
        string outCardPath = "";
        switch (transform.parent.name)
        {
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

    override public bool AddToHandCard(Card card, string imangName)
    {
        Debug.Log("AddToHandCard, " + transform.parent.name);
        HandCard hc = Canvas.Instantiate(handCard);

        if (hc != null)
        {
            Sprite sp;
            hc.Card = card;
            Debug.Log("!!!!!!!!!" + card.Name);
            if(transform.parent.name == "Next" || transform.parent.name == "Last") 
                headCSD.TryGetValue("hand_left", out sp);
            else
                headCSD.TryGetValue("hand_top", out sp);
            hc.HandCardImage.sprite = sp;
            wanCardList.Add(hc);
            hc.transform.parent = gridListPoint;
            hc.transform.localScale = new Vector3(1f, 1f, 1f);
            return true;
        }
        return false;
    }
    override public List<Card> GetCardS()
    {
        return new List<Card>();
    }
    override public List<HandCard> GetHeadCardS()
    {
        return new List<HandCard>();
    }
    override public List<Card> GetCardTpyeList()
    {
        return new List<Card>();
    }

    override public int GetHeadCardCount()
    {
        return wanCardList.Count;
    }
    override public void AddToOutCardList(Card card)
    {
        HandCard hc = Canvas.Instantiate(handCard);
        if (hc != null)
        {
            Sprite sp;
            newOutHC = hc;
            hc.Card = card;
            outCSD.TryGetValue(card.ImageName, out sp);
            hc.HandCardImage.sprite = sp;
            hc.transform.parent = outGridListPoint;
            hc.InitLocation();
            OutCardList.Add(hc);
            MahjonManage._instance.SetBiaojiPoint(hc.transform);
        }
    }
    /// <summary>
    /// 把桌面或手上的牌移动到牌型表展示
    /// </summary>
    /// <param name="card"></param>
    /// <param name="count"></param>
    override public void MoveToCardTypeList(Card card, int count)
    {
        //HandCard hc = null;
        Sprite sp;
        outCSD.TryGetValue(card.ImageName, out sp);
        for (int i = 0; i < count; i++)//获取到这张牌的hc属性
        {
            /*Card card1 = MahjonManage.ParseCard("11"); //TODO: connect server
            HandCard hc = new HandCard();
            hc.Card = card1;*/
            Card card1 = MahjonManage.ParseCard("11"); 
            HandCard hc1 = CardToHandCard(wanCardList,card1);
            HandCard hc = Canvas.Instantiate(handCard);
            wanCardList.RemoveAt(0);
            if (count == 1)
            {
                Transform ts;
                cardTPD.TryGetValue(card.Name, out ts);
                hc.HandCardImage.sprite = sp;
                hc.transform.parent = ts;
                hc.InitLocation();
                CardTypeList.Add(hc.Card);
                Destroy(hc1.gameObject);
            }
            else if (count == 3 || count == 4)
            {
                hc.HandCardImage.sprite = sp;
                hc.transform.parent = cardTypePoint[1];
                hc.InitLocation();
                if (!cardTPD.ContainsKey(card.Name))
                {
                    cardTPD.Add(card.Name, cardTypePoint[1]);
                }
                CardTypeList.Add(hc.Card);
                Destroy(hc1.gameObject);
            }
        }
        if (count != 1)
        {
            cardTypePoint.RemoveAt(1);
        }
        HandCardSort();
    }

    private HandCard CardToHandCard(List<HandCard> list, Card card)
    {
        foreach (HandCard hc in list)
        {
            if (hc.Card.Size == card.Size)
            {
                return hc;
            }
        }
        return null;
    }

    override public void AutoOutCard()
    {
        Debug.Log("AutoOutCard, " + transform.parent.name);
        StartCoroutine(AutoOutCardCoroutine());
    }


    IEnumerator AutoOutCardCoroutine()
    {
        yield return new WaitForEndOfFrame();
        var x = new Dictionary<string, string>();
        string _card = "";
        Debug.Log("HCL_AI:150 Listening");
        while (conn.NoQueryInQueue)
        {
            yield return new WaitForEndOfFrame();
        }
        x = conn.PopQuery;
        Debug.Log("Received " + x["ResponseType"]);
        //while (x["ResponseType"] != "OutCard") 
        while (x["ResponseType"] == "NoAction")
        {
            Debug.Log("Throwing " + x["ResponseType"]);
            Debug.Log("HCL_AI:159 Listening");
            while (conn.NoQueryInQueue)
            {
                yield return new WaitForEndOfFrame();
            }
            x = conn.PopQuery;
            Debug.Log("Received " + x["ResponseType"]);
        }
        _card = x["OutCard"];
        Card card = MahjonManage.ParseCard(_card); //TODO: connect server
        HandCard hc = new HandCard();
        hc.Card = card;
        RemoveHandCard(hc);
    }
    /// <summary>
    /// 给自己的牌排序
    /// </summary>
    /// <param name="player"></param>
    override public void HandCardSort()
    {
        Dictionary<string, List<HandCard>> dicList = new Dictionary<string, List<HandCard>>();
        dicList.Add("万", wanCardList);
        MarksHandCardSort(dicList);
    }

    override public void MarksHandCardSort(Dictionary<string, List<HandCard>> dicList)
    {
        int number = 0;
        foreach (var list in dicList)
        {
            for (int i = 0; i < list.Value.Count; i++)
            {
                number++;
                if (list.Value[i].transform.parent.name != "GridList")
                {
                    list.Value[i].transform.parent = gridListPoint;
                }
                list.Value[i].transform.SetSiblingIndex(number);
            }
        }
    }

    void MarksCardSort(ref List<HandCard> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            HandCard min = list[i];
            int minIndex = i;//最小的值所在索引
            for (int j = i + 1; j < list.Count; j++)
            {
                if (list[j].Card.Size < min.Card.Size)
                {
                    min = list[j];
                    minIndex = j;
                }
            }
            if (minIndex != i)
            {
                HandCard temp = list[i];
                list[i] = list[minIndex];
                list[minIndex] = temp;
            }
        }
    }
        
    override public void RemoveHandCard(HandCard hc)
    {
        Debug.Log("RemoveHandCard, " + transform.parent.name);
        IsOutCard = false;
        Debug.Log(transform.parent.name + " 打出去的牌是：" + hc.Card.Name);
        //把手上的牌移动到出牌列表中并更改显示图片
        AddToOutCardList(hc.Card);
        MahjonManage._instance.StartOutNewCard(newOutHC.Card);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("PointerClick, " + transform.parent.name);
        if (transform.parent.name != "Me") return;
        GameAudio.instance.PlayaudioSourceRole("select");
        GameObject selectObj = eventData.pointerEnter.gameObject.transform.parent.gameObject;
        string objName = selectObj.transform.parent.name;

        if (objName != "GridList" && objName != "NewAdd") return;

        if (selectObj == null) return;

        if (currentObj == null)
        {
            selectObj.transform.localPosition += movePos;
            currentObj = selectObj;
        }
        else if (currentObj.Equals(selectObj) == false)
        {
            currentObj.transform.localPosition -= movePos;
            selectObj.transform.localPosition += movePos;
            currentObj = selectObj;
        }
        else if (IsOutCard)//这种情况就是现在点击的和上次点击相同的那么我们就可以直接出牌出去
        {
            HandCard hc = currentObj.GetComponent<HandCard>();
            if (hc != null)
            {
                currentObj = null;
                RemoveHandCard(hc);
            }
        }
    }
}
