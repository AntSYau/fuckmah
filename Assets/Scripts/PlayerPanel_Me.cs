using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerPanel_Me : PlayerPanel
{
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

    /// <summary>
    /// 听自己摸的牌
    /// </summary>
    /// <param name="card"></param>
    override public void ListenTouchCard(Card card)
    {
        Debug.Log("ListenTouchCard, "+MyPosIndex);
        isPeng = false;
        isHu = false;
        isGang = -1;
        tempOutCard = card;
        IsSelfTouch = true;
        isHu = MahjonManage._instance.Algorithm.IsHu(HandCarde.GetCardS());
        if (isHu) tc = tempOutCard;
        if (MahjonManage._instance.Algorithm.IsGang(HandCarde.GetCardS(), card, 4) == true)//自己摸起来的四张就是暗杠
        {
            isGang = 1;
        }
        else if (MahjonManage._instance.Algorithm.IsGang(HandCarde.CardTypeList, card, 3) == true)//碰了的三张牌自己摸起来一张就是加杠
        {
            isGang = 2;
        }
        Dictionary<string, string> message = new Dictionary<string, string>();
        message.Add("Peng", isPeng?"1":"0");
        message.Add("Gang", isGang.ToString());
        message.Add("Hu", isHu ? "1" : "0");
        message.Add("RequestType", "Decision");
        conn.EncodeAndSendMessage(message,"ListenTouchCard:51,"+MyPosIndex);
        OneselfChooseCard(isPeng, isGang, isHu);
        StartCoroutine(WaitForAPacket());
    }

    IEnumerator WaitForAPacket()
    {
        while (conn.NoQueryInQueue) yield return new WaitForSecondsRealtime(0.1f);
        var _ = conn.PopQuery;
    }
    /// <summary>
    /// 听别人打出的牌
    /// </summary>
    /// <param name="card"></param>
    override public void ListenOutCard(Card card, Dictionary<string, string> status)
    {
        Debug.Log("ListenOutCard, " + MyPosIndex);
        isPeng = false;
        isHu = false;
        isGang = -1;
        IsOnListenCard = true;
        tempOutCard = card;
        IsSelfTouch = false;

        isPeng = MahjonManage._instance.Algorithm.IsPeng(HandCarde.GetCardS(), card);
        if (MahjonManage._instance.Algorithm.IsGang(HandCarde.GetCardS(), card, 3) == true)
        {
            isGang = 0;
        }
        List<Card> headCards = HandCarde.GetCardS();
        headCards.Add(card);
        isHu = MahjonManage._instance.Algorithm.IsHu(headCards);
        if (isHu) tc = tempOutCard;
        if (isPeng == false && isGang == -1 && isHu == false)
        {
            Dictionary<string, string> message = new Dictionary<string, string>();
            message.Add("Peng", "0");
            message.Add("Gang", "-1");
            message.Add("Hu", "0");
            message.Add("RequestType", "Decision");
            conn.EncodeAndSendMessage(message,"ListenOutCard,PPM:84,"+MyPosIndex);
            IsOnListenCard = false;
            return;
        }
        OneselfChooseCard(isPeng, isGang, isHu);
    }
    void OneselfChooseCard(bool isPeng, int isGang, bool isHu)
    {
        Debug.Log("OneselfChooseCard, " + MyPosIndex);
        if (isPeng)//是否可以碰牌
        {
            ListenCard.PengButton.gameObject.SetActive(true);
            ListenCard.GuoButton.gameObject.SetActive(true);
            return;
        }
        else if (isGang >= 0)//是否可以杠牌 -1不能 0是明杠 1加杠 2是暗杠
        {
            ListenCard.GangButton.gameObject.SetActive(true);
            ListenCard.GuoButton.gameObject.SetActive(true);
            return;
        }
        else if (isHu)//是否可以胡牌
        {
            ListenCard.HuButton.gameObject.SetActive(true);
            ListenCard.GuoButton.gameObject.SetActive(true);
            return;
        }
    }
}
