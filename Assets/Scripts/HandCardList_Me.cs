using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.Jobs;

public class HandCardList_Me : HandCardList, IPointerClickHandler
{
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
    override public bool AddToHandCard(Card card, string imangName)
    {
        HandCard hc = Canvas.Instantiate(handCard);

        if (hc != null)
        {
            Sprite sp;
            hc.Card = card;
            headCSD.TryGetValue(imangName, out sp);
            hc.HandCardImage.sprite = sp;
            /*if(CurrentFlowerPig== card.CardMark && transform.parent.name=="Me")
            {
                SetGreyCard(hc);
            }*/
            if (card.CardMark == "万")
            {
                wanCardList.Add(hc);
                MarksCardSort(ref wanCardList);
            }
            else if (card.CardMark == "条")
            {
                tiaoCardList.Add(hc);
                MarksCardSort(ref tiaoCardList);
            }
            else if (card.CardMark == "筒")
            {
                tongCardList.Add(hc);
                MarksCardSort(ref tongCardList);
            }
            else if (card.CardMark == "字")
            {
                ziCardList.Add(hc);
                MarksCardSort(ref ziCardList);
            }
            if (newAddPoint != null)
            {
                newAddHC = hc;
                hc.transform.parent = newAddPoint.transform;
                hc.transform.localScale = new Vector3(1f, 1f, 1f);
                newAddPoint.transform.localPosition = new Vector3(GetHeadCardCount() * 35, 0, 0);
            }
            else
            {
                hc.transform.parent = gridListPoint;
                hc.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            return true;
        }
        return false;
    }
    override public List<Card> GetCardS()
    {
        List<Card> allcard = new List<Card>();
        foreach (HandCard hc in wanCardList)
        {
            allcard.Add(hc.Card);
        }
        foreach (HandCard hc in tiaoCardList)
        {
            allcard.Add(hc.Card);
        }
        foreach (HandCard hc in tongCardList)
        {
            allcard.Add(hc.Card);
        }
        foreach (HandCard hc in ziCardList)
        {
            allcard.Add(hc.Card);
        }

        return allcard;
    }
    override public List<HandCard> GetHeadCardS()
    {
        List<HandCard> allHC = new List<HandCard>();
        foreach (HandCard hc in wanCardList)
        {
            allHC.Add(hc);
        }
        foreach (HandCard hc in tiaoCardList)
        {
            allHC.Add(hc);
        }
        foreach (HandCard hc in tongCardList)
        {
            allHC.Add(hc);
        }
        foreach (HandCard hc in ziCardList)
        {
            allHC.Add(hc);
        }
        return allHC;
    }
    override public List<Card> GetCardTpyeList()
    {
        List<Card> allcard = new List<Card>();
        foreach (Card hc in CardTypeList)
        {
            allcard.Add(hc);
        }
        return allcard;
    }
    /*override public int GetCurrentFlowerPigCount()
    {
        List<Card> card = GetCardS();
        var query = card.Where(c => c.CardMark == CurrentFlowerPig);
        return query.ToList().Count;
    }*/
    override public int GetHeadCardCount()
    {
        return tongCardList.Count + wanCardList.Count + tiaoCardList.Count + ziCardList.Count;
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
        HandCard hc = null;
        Sprite sp;
        outCSD.TryGetValue(card.ImageName, out sp);
        for (int i = 0; i < count; i++)//获取到这张牌的hc属性
        {
            if (card.CardMark == "万")
            {
                hc = CardToHandCard(wanCardList, card);
                wanCardList.Remove(hc);
            }
            else if (card.CardMark == "条")
            {
                hc = CardToHandCard(tiaoCardList, card);
                tiaoCardList.Remove(hc);
            }
            else if (card.CardMark == "筒")
            {
                hc = CardToHandCard(tongCardList, card);
                tongCardList.Remove(hc);
            }
            else if (card.CardMark == "字")
            {
                hc = CardToHandCard(ziCardList, card);
                ziCardList.Remove(hc);
            }
            if (count == 1)
            {
                Transform ts;
                cardTPD.TryGetValue(card.Name, out ts);
                hc.HandCardImage.sprite = sp;
                hc.transform.parent = ts;
                hc.InitLocation();
                CardTypeList.Add(hc.Card);
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
            }
        }
        if (count != 1)
        {
            cardTypePoint.RemoveAt(1);
        }
        HandCardSort();
    }

    /*override public string AutoChooseFlowerPig()
    {
        if(wanCardList.Count<=tiaoCardList.Count && wanCardList.Count <= tongCardList.Count)
        {
            return "万";
        }
        else if(tiaoCardList.Count <= wanCardList.Count && tiaoCardList.Count <= tongCardList.Count)
        {
            return "条";
        }
        else if (tongCardList.Count <= wanCardList.Count && tongCardList.Count <= tiaoCardList.Count)
        {
            return "筒";
        }

        return "";
    }*/
    override public void AutoOutCard()
    {
        Card card = MahjonManage._instance.Algorithm.GoCard(GetCardS(), ""); //TODO: connect server
        HandCard hc = null;
        if (card.CardMark == "万")
        {
            hc = CardToHandCard(wanCardList, card);
        }
        else if (card.CardMark == "条")
        {
            hc = CardToHandCard(tiaoCardList, card);
        }
        else if (card.CardMark == "筒")
        {
            hc = CardToHandCard(tongCardList, card);
        }
        else if (card.CardMark == "字")
        {
            hc = CardToHandCard(ziCardList, card);
        }
        RemoveHandCard(hc);
    }
    /// <summary>
    /// 给自己的牌排序
    /// </summary>
    /// <param name="player"></param>
    override public void HandCardSort()
    {
        Dictionary<string, List<HandCard>> dicList = new Dictionary<string, List<HandCard>>();
        /*if (CurrentFlowerPig == "万")
        {
            dicList.Add("条", tiaoCardList);
            dicList.Add("筒", tongCardList);
            dicList.Add("万", wanCardList);
        }
        else if (CurrentFlowerPig == "条")
        {
            dicList.Add("万", wanCardList);
            dicList.Add("筒", tongCardList);
            dicList.Add("条", tiaoCardList);
        }
        else
        {
            dicList.Add("万", wanCardList);
            dicList.Add("条", tiaoCardList);
            dicList.Add("筒", tongCardList);
        }*/
        dicList.Add("万", wanCardList);
        dicList.Add("条", tiaoCardList);
        dicList.Add("筒", tongCardList);
        dicList.Add("字", ziCardList);

        MarksHandCardSort(dicList);
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
                /*if (list.Key==CurrentFlowerPig && transform.parent.name=="Me")
                {
                    SetGreyCard(list.Value[i]);
                }*/
                list.Value[i].transform.SetSiblingIndex(number);
            }
        }
    }

    override public void RemoveHandCard(HandCard hc)
    {
        IsOutCard = false;
        if (wanCardList.Remove(hc))
        {
            Debug.Log(transform.parent.name + " 打出去的牌是：" + hc.Card.Name);
        }
        else if (tiaoCardList.Remove(hc))
        {
            Debug.Log(transform.parent.name + " 打出去的牌是：" + hc.Card.Name);
        }
        else if (tongCardList.Remove(hc))
        {
            Debug.Log(transform.parent.name + " 打出去的牌是：" + hc.Card.Name);
        }
        else if (ziCardList.Remove(hc))
        {
            Debug.Log(transform.parent.name + " 打出去的牌是：" + hc.Card.Name);
        }
        //把手上的牌移动到出牌列表中并更改显示图片
        AddToOutCardList(hc.Card);
        StartCoroutine(GetMessageAndOutCard(hc));
    }

    IEnumerator GetMessageAndOutCard(HandCard hc)
    {
        yield return new WaitForEndOfFrame();
        var x = new Dictionary<string, string>();
        x.Add("RequestType", "OutCard");
        x.Add("OutCard", MahjonManage.EncodeCard(hc.Card));
        conn.EncodeAndSendMessage(x,"GetMessageAndOutCard");
        /*var task = new System.Threading.Tasks.Task(() => { conn.GetMessageAndDecode(); });
        task.Start();
        while (!task.IsCompleted)
        {
            yield return new WaitForEndOfFrame();
        }*/
        Debug.Log("HCL_Me:399 Listening");
        while (conn.NoQueryInQueue)
        {
            yield return new WaitForEndOfFrame();
        }
        Destroy(hc.gameObject);
        HandCardSort();
        MahjonManage._instance.StartOutNewCard(newOutHC.Card);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
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
