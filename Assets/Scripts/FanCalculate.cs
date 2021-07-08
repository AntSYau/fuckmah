using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;

public class FanCalculate
{
    private HandCardList handCard;
    private Card getCard;
    private List<Card> CList = new List<Card>();
    private List<Card> CardList = new List<Card>();
    private List<Card> cardTypeList = new List<Card>();
    private List<Transform> fanList = new List<Transform>();
    private bool[] fanDetail = new bool[50];
    private string[] FanList = new string[]{
        "QueMen", "Dasixi", "Dasanyuan", "Jiulianbaodeng",
        "Sigang", "Lianqidui", "Qingyaojiu", "Xiaosixi",
        "Xiaosanyuan", "Ziyise", "FanCalDebug"
    };

    public FanCalculate()
    {

    }

    public void Set()
    {
        PlayerPrefs.DeleteKey("fan");
        PlayerPrefs.SetString("fan", toString());
        Debug.Log(PlayerPrefs.GetString("fan"));
    }

    void FanCalDebug()
    {
        fanDetail[49] = true;
    }

    string toString()
    {
        string ret = "";
        foreach (bool c in fanDetail)
        {
            string tmp = "0";
            if (c) tmp = "1";
            ret = ret + tmp;
        }
        return ret;
    }

    public void SetParameters(HandCardList List, Card tmp)
    {
        handCard = List;
        getCard = tmp;
        CardList = handCard.GetCardS();
        cardTypeList = handCard.GetCardTpyeList();
        foreach (Card c in CardList)
        {
            CList.Add(c);
            Debug.Log(c.Name);
        }
        foreach (Card c in cardTypeList)
        {
            CList.Add(c);
            Debug.Log(c.Name);
        }
        CList.Add(getCard);
        Debug.Log(getCard.Name);
    }

    public void Calculate()
    {
        //foreach(string str in FanList)
        //{
        //    Debug.Log(str);
        //    Type type = typeof(FanCalculate);
        //    Debug.Log(type);
        //    System.Object obj = System.Activator.CreateInstance(type);
        //    object[] parameters = null;
        //    MethodInfo method = GetType().GetMethod(str);
        //    method.Invoke(obj, parameters);
        //}
        QueMen();
    }

    public void Store()
    {
        PlayerPrefs.DeleteKey("fan");
        PlayerPrefs.SetString("fan", toString());
    }

    public void QueMen()
    {
        bool ti = false;
        bool w = false;
        bool to = false;
        foreach (Card c in CList)
        {
            if (c.CardMark == "条" && ti == false)
            {
                ti = true;
            }
            if (c.CardMark == "万" && w == false)
            {
                w = true;
            }
            if (c.CardMark == "筒" && to == false)
            {
                to = true;
            }
        }
        if (ti == false && to == true && w == true)
            fanDetail[0] = true;
        if (ti == true && to == false && w == true)
            fanDetail[0] = true;
        if (ti == true && to == true && w == false)
            fanDetail[0] = true;
    }

    // 88番 大四喜
    public int Dasixi()
    {
        int dong_1 = 0;
        int xi_1 = 0;
        int nan_1 = 0;
        int bei_1 = 0;
        if (getCard.CardMark == "字")
        {
            if (getCard.Name == "东风")
            {
                dong_1++;
            }
            if (getCard.Name == "西风")
            {
                xi_1++;
            }
            if (getCard.Name == "南风")
            {
                nan_1++;
            }
            if (getCard.Name == "北风")
            {
                bei_1++;
            }
        }
        foreach (Card c in CardList)
        {
            if (c.CardMark == "字")
            {
                if (c.Name == "东风")
                {
                    dong_1++;
                }
                if (c.Name == "西风")
                {
                    xi_1++;
                }
                if (c.Name == "南风")
                {
                    nan_1++;
                }
                if (c.Name == "北风")
                {
                    bei_1++;
                }
            }
        }

        int dong = 0;
        int xi = 0;
        int nan = 0;
        int bei = 0;

        foreach (Card c in cardTypeList)
        {
            if (c.CardMark == "字")
            {
                if (c.Name == "东风")
                {
                    dong++;
                }
                if (c.Name == "西风")
                {
                    xi++;
                }
                if (c.Name == "南风")
                {
                    nan++;
                }
                if (c.Name == "北风")
                {
                    bei++;
                }
            }
        }
        if (dong <= 3 && dong_1 <= 2)
        {
            return 0;
        }
        if (bei <= 3 && bei_1 <= 2)
        {
            return 0;
        }
        if (nan <= 3 && nan_1 <= 2)
        {
            return 0;
        }
        if (xi <= 3 && xi_1 <= 2)
        {
            return 0;
        }
        return 88;
    }

    // 88番 大三元
    public int Dasanyuan()
    {

        int zhong_1 = 0;
        int fa_1 = 0;
        int bai_1 = 0;
        if (getCard.CardMark == "字")
        {
            if (getCard.Name == "红中")
            {
                zhong_1++;
            }
            if (getCard.Name == "发财")
            {
                fa_1++;
            }
            if (getCard.Name == "白板")
            {
                bai_1++;
            }
        }
        foreach (Card c in CardList)
        {
            if (c.CardMark == "字")
            {
                if (c.Name == "红中")
                {
                    zhong_1++;
                }
                if (c.Name == "发财")
                {
                    fa_1++;
                }
                if (c.Name == "白板")
                {
                    bai_1++;
                }
            }
        }

        int zhong = 0;
        int fa = 0;
        int bai = 0;
        foreach (Card c in cardTypeList)
        {
            if (c.CardMark == "字")
            {
                if (c.Name == "红中")
                {
                    zhong++;
                }
                if (c.Name == "发财")
                {
                    fa++;
                }
                if (c.Name == "白板")
                {
                    bai++;
                }
            }
        }
        if (bai <= 3 && bai_1 <= 2)
        {
            return 0;
        }
        if (fa <= 3 && fa_1 <= 2)
        {
            return 0;
        }
        if (zhong <= 3 && zhong_1 <= 2)
        {
            return 0;
        }
        return 88;
    }

    private int hashMark(string s)
    {
        if (s == "条")
        {
            return 0;
        }
        if (s == "筒")
        {
            return 1;
        }
        if (s == "万")
        {
            return 2;
        }
        if (s == "字")
        {
            return 3;
        }
        return -1;
    }

    // 88番 九莲宝灯
    public int Jiulianbaodeng()
    {
        int[,] vis = new int[10, 3];
        for (int i = 1; i <= 9; i++)
        {
            for (int j = 0; j <= 2; j++)
            {
                vis[i, j] = 0;
            }
        }
        foreach (Card c in CList)
        {
            int j = hashMark(c.CardMark);
            if (j != -1 && j <= 2)
            {
                vis[c.Size, j]++;
            }
        }
        int[] arr = new int[10] { 0, 3, 1, 1, 1, 1, 1, 1, 1, 3 };
        int mk = -1;
        for (int j = 0; j <= 2; j++)
        {
            bool pd = true;
            for (int i = 1; i <= 9; i++)
            {
                if (arr[i] > vis[i, j])
                {
                    pd = false;
                }
            }
            if (pd)
            {
                mk = j;
            }
        }
        if (mk == -1)
        {
            return 0;
        }
        if (hashMark(getCard.CardMark) != mk)
        {
            return 0;

        }
        else if (arr[getCard.Size] >= vis[getCard.Size, mk])
        {
            return 0;
        }
        return 88;
    }

    // 88番 四杠
    public int Sigang()
    {
        int[] count = new int[34];
        foreach (Card c in cardTypeList)
        {
            switch (c.CardMark)
            {
                case "万":
                    count[c.Size - 1]++;
                    break;
                case "条":
                    count[c.Size + 9 - 1]++;
                    break;
                case "筒":
                    count[c.Size + 9 * 2 - 1]++;
                    break;
                case "字":
                    count[c.Size % 7 + 9 * 3 - 1]++;
                    break;
            }
        }
        int cnt = 0;
        for (int i = 0; i <= 33; i++)
        {
            if (count[i] >= 4)
            {
                cnt++;
            }
        }
        if (cnt <= 3)
        {
            return 0;
        }
        return 88;
    }

    // 88番 连七对
    public int Lianqidui()
    {
        int[,] vis = new int[10, 3];
        for (int i = 0; i <= 9; i++)
        {
            for (int j = 0; j <= 2; j++)
            {
                vis[i, j] = 0;
            }
        }
        foreach (Card c in CList)
        {
            int j = hashMark(c.CardMark);
            if (j != -1 && j <= 2)
            {
                vis[c.Size, j]++;
            }
        }
        int mk = -1;

        for (int j = 0; j <= 2; j++)
        {
            int cnt = 0;
            for (int i = 9; i >= 0; i--)
            {
                if (vis[i, j] == 0)
                {
                    if (cnt >= 7)
                    {
                        mk = j;
                    }
                    cnt = 0;
                }
            }
        }
        if (mk == -1)
        {
            return 0;
        }
        return 88;
    }

    // 88番 十三幺

    //64番 清幺九
    public int Qingyaojiu()
    {
        foreach (Card c in CList)
        {
            if (hashMark(c.CardMark) == -1 || hashMark(c.CardMark) >= 2)
            {
                return 0;
            }
            if (c.Size != 1 && c.Size != 9)
            {
                return 0;
            }
        }
        return 66;
    }

    // 64番 小四喜
    public int Xiaosixi()
    {
        int dong_1 = 0;
        int xi_1 = 0;
        int nan_1 = 0;
        int bei_1 = 0;
        if (getCard.CardMark == "字")
        {
            if (getCard.Name == "东风")
            {
                dong_1++;
            }
            if (getCard.Name == "西风")
            {
                xi_1++;
            }
            if (getCard.Name == "南风")
            {
                nan_1++;
            }
            if (getCard.Name == "北风")
            {
                bei_1++;
            }
        }
        foreach (Card c in CardList)
        {
            if (c.CardMark == "字")
            {
                if (c.Name == "东风")
                {
                    dong_1++;
                }
                if (c.Name == "西风")
                {
                    xi_1++;
                }
                if (c.Name == "南风")
                {
                    nan_1++;
                }
                if (c.Name == "北风")
                {
                    bei_1++;
                }
            }
        }
        int dong = 0;
        int xi = 0;
        int nan = 0;
        int bei = 0;
        foreach (Card c in cardTypeList)
        {
            if (c.CardMark == "字")
            {
                if (c.Name == "东风")
                {
                    dong++;
                }
                if (c.Name == "西风")
                {
                    xi++;
                }
                if (c.Name == "南风")
                {
                    nan++;
                }
                if (c.Name == "北风")
                {
                    bei++;
                }
            }
        }
        int cnt = 0;
        int cnt1 = 0;
        if (dong >= 4 || dong_1 >= 3)
        {
            cnt++;
        }
        else if (dong_1 >= 2)
        {
            cnt1++;
        }
        if (bei >= 4 || bei_1 >= 3)
        {
            cnt++;
        }
        else if (bei_1 >= 2)
        {
            cnt1++;
        }
        if (nan >= 4 || nan_1 >= 3)
        {
            cnt++;
        }
        else if (nan_1 >= 2)
        {
            cnt1++;
        }
        if (xi >= 4 && xi_1 >= 3)
        {
            cnt++;
        }
        else if (xi_1 >= 2)
        {
            cnt1++;
        }
        if (cnt >= 3 && cnt1 >= 1)
        {
            return 64;
        }
        return 0;
    }

    // 64番 小三元
    public int Xiaosanyuan()
    {

        int zhong_1 = 0;
        int fa_1 = 0;
        int bai_1 = 0;
        if (getCard.CardMark == "字")
        {
            if (getCard.Name == "红中")
            {
                zhong_1++;
            }
            if (getCard.Name == "发财")
            {
                fa_1++;
            }
            if (getCard.Name == "白板")
            {
                bai_1++;
            }
        }
        foreach (Card c in CardList)
        {
            if (c.CardMark == "字")
            {
                if (c.Name == "红中")
                {
                    zhong_1++;
                }
                if (c.Name == "发财")
                {
                    fa_1++;
                }
                if (c.Name == "白板")
                {
                    bai_1++;
                }
            }
        }

        int zhong = 0;
        int fa = 0;
        int bai = 0;
        foreach (Card c in cardTypeList)
        {
            if (c.CardMark == "字")
            {
                if (c.Name == "红中")
                {
                    zhong++;
                }
                if (c.Name == "发财")
                {
                    fa++;
                }
                if (c.Name == "白板")
                {
                    bai++;
                }
            }
        }
        int cnt = 0;
        int cnt1 = 0;
        if (bai >= 4 || bai_1 >= 3)
        {
            cnt++;
        }
        else if (bai_1 >= 2)
        {
            cnt1++;
        }
        if (fa >= 4 || fa_1 >= 3)
        {
            cnt++;
        }
        else if (fa_1 >= 2)
        {
            cnt1++;
        }
        if (zhong >= 4 || zhong_1 >= 3)
        {
            cnt++;
        }
        else if (zhong_1 >= 2)
        {
            cnt1++;
        }
        if (cnt >= 3 && cnt1 >= 1)
        {
            return 64;
        }
        return 0;
    }


    //64番 字一色
    public int Ziyise()
    {
        foreach (Card c in CList)
        {
            if (hashMark(c.CardMark) != 3)
            {
                return 0;
            }
        }
        return 64;
    }
}
