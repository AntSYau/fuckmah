using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

public class MahjonAlgorithm
{

    //构造方法
    public MahjonAlgorithm()
    {

    }

    /// <summary>
    /// 是否能碰牌
    /// </summary>
    /// <param name="handcard"></param>
    /// <returns></returns>
    public bool IsPeng(List<Card> handcard, Card gocard)
    {
        List<Card> mid = TraverseSpecifiedNumber(handcard, 2);
        if (mid.Count >= 2)
        {
            foreach (Card card in mid)
            {
                if (gocard.Size == card.Size && gocard.CardMark == card.CardMark)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool IsGang(List<Card> handcard, Card gocard, int number)
    {
        List<Card> mid = TraverseSpecifiedNumber(handcard, number);
        if (mid.Count >= number)
        {
            foreach (Card card in mid)
            {
                if (gocard.Size == card.Size && gocard.CardMark == card.CardMark)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 是否胡牌
    /// </summary>
    /// <param name="handcard"></param>
    /// <param name="firstMark"></param>
    /// <returns></returns>
    public bool IsHu(List<Card> handcard)
    {
        int[] count = new int[34];
        foreach (Card cd in handcard)
        {
            switch (cd.CardMark)
            {
                case "万":
                    count[cd.Size - 1]++;
                    break;
                case "条":
                    count[cd.Size + 9 - 1]++;
                    break;
                case "筒":
                    count[cd.Size + 9 * 2 - 1]++;
                    break;
                case "字":
                    count[cd.Size % 7 + 9 * 3 - 1]++;
                    break;
            }
        }
        return (TryHu(count, handcard.Count));
    }

    public int IsFirstMark(List<Card> handcard, string firstMark = "")
    {
        int count = 0;
        foreach (Card card in handcard)
        {
            if (card.CardMark == firstMark)
            {
                count++;
            }
        }
        return (count);
    }
    public Card GoCard(List<Card> handcard, string firstMark = "")
    {
        List<Card> tempCardList = new List<Card>();
        foreach (Card card in handcard)
        {
            if (card.CardMark == firstMark)
            {
                return card;
            }
            else
            {
                tempCardList.Add(card);
            }
        }
        //暂时这里随机咯 以后可以在这里写多种算法加强机器人自动出牌
        Random ra = new Random();
        int next = ra.Next(1, tempCardList.Count);
        return tempCardList[next - 1];
    }
    //遍历手中相同牌的指定数量
    private List<Card> TraverseSpecifiedNumber(List<Card> handcard, int number)
    {
        List<Card> cards = new List<Card>();
        int tempNumber = 0;
        for (int i = 0; i < handcard.Count; i++)
        {
            for (int j = 0; j < handcard.Count; j++)
            {
                if (handcard[i].Size == handcard[j].Size && handcard[i].CardMark == handcard[j].CardMark)
                    tempNumber++;
            }
            if (tempNumber == number)
            {
                cards.Add(handcard[i]);
            }
            tempNumber = 0;
        }
        return cards;
    }

    /// <summary>
    /// 利用递归属性去检测是否能糊牌成功返回真失败假
    /// </summary>
    /// <param name="count"></param>
    /// <param name="cardNumber"></param>
    /// <returns></returns>
    public bool TryHu(int[] count, int cardNumber)
    {
        if (cardNumber == 0) return true;
        if (cardNumber % 3 == 2)
        {
            // 一副牌必须有对牌 没有优先判断
            for (var i = 0; i < 34; i++)
            {
                if (count[i] >= 2)
                {
                    count[i] -= 2;
                    if (TryHu(count, cardNumber - 2))
                    {
                        return true;
                    }
                    else
                    {
                        count[i] += 2;
                    }
                }
            }
        }
        else if (cardNumber % 3 == 0)
        {
            //是否还有三张一样的牌 
            for (var i = 0; i < 34; i++)
            {
                if (count[i] >= 3)
                {
                    count[i] -= 3;
                    if (TryHu(count, cardNumber - 3))
                    {
                        return true;
                    }
                    else
                    {
                        count[i] += 3;
                    }

                }

            }
            //三张一起是否是顺子
            int k;
            for (int i = 0; i < 3; i++)
            {
                k = i * 9;
                for (int j = 0; j < 7; j++)
                {
                    if (count[k + j] > 0 && count[k + j + 1] > 0 && count[k + j + 2] > 0)
                    {
                        count[k + j] -= 1;
                        count[k + j + 1] -= 1;
                        count[k + j + 2] -= 1;
                        if (TryHu(count, cardNumber - 3))
                        {
                            return true;
                        }
                        else
                        {
                            count[k + j] += 1;
                            count[k + j + 1] += 1;
                            count[k + j + 2] += 1;
                        }
                    }
                }
            }
        }
        return false;
    }
}
