using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;
public class PlayerPanel_AI : PlayerPanel
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
        Debug.Log("ListenOutCard, " + MyPosIndex);
        isPeng = false;
        isHu = false;
        isGang = -1;
        StartCoroutine(ListenTouchCardContd(card));
    }

    IEnumerator ListenTouchCardContd(Card card) {
        Debug.Log("PP_AI:38 Listening. Who: "+MyPosIndex);
        while (conn.NoQueryInQueue)
        {
            yield return new WaitForEndOfFrame();
        }
        status = conn.PopQuery;
        while (status["ResponseType"] == "OutCard") {
            Debug.Log("PP_AI:38 Listening");
            while (conn.NoQueryInQueue)
            {
                yield return new WaitForEndOfFrame();
            }
            status = conn.PopQuery;
        }
        Debug.Log("Received " + status["ResponseType"]);
        if (status["ResponseType"] == "NoAction") HandCarde.AutoOutCard();
        else if (status["Operation"] == "Peng")
        {
            tempOutCard = MahjonManage.ParseCard(status["Card"]);
            isPeng = true;
            PengCard();
        }
        else if (status["Operation"] == "Gang")
        {
            tempOutCard = MahjonManage.ParseCard(status["Card"]);
            isGang = int.Parse(status["Gang"]);//0是明杠，1是暗杠，2是加杠
            GangCard();
        }
        else if (status["Operation"] == "Hu")
        {
            HuCard();
        }
    }
    /// <summary>
    /// 听别人打出的牌
    /// </summary>
    /// <param name="card"></param>
    public override void ListenOutCard(Card card, Dictionary<string, string> status)
    {
        Debug.Log("DecisionListenCard, " + MyPosIndex);
        if (status["ResponseType"] == "NoAction") return;
        if (int.Parse(status["User"]) != _myPosIndex) return;
        switch (status["Operation"])
        {
            case "Peng":
                tempOutCard = MahjonManage.ParseCard(status["Card"]);
                isPeng = true;
                PengCard();
                break;
            case "Hu":
                isHu = true;
                HuCard();
                break;
            default:
                tempOutCard = MahjonManage.ParseCard(status["Card"]);
                isGang = int.Parse(status["Gang"]);
                GangCard();
                break;
        }
    }
}
