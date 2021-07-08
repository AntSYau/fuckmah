using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AwaitCardList : MonoBehaviour
{
    private GridLayoutGroup cardGridUP;
    private GridLayoutGroup cardGridDown;
    //防止作弊 只需要设置索引不赋值引用
  
    private List<Image> _indexUpList = new List<Image>();
    private List<Image> _indexDownList = new List<Image>();
    public Image cardImage;

    public List<Image> IndexUpList
    {
        get
        {
            return _indexUpList;
        }

        set
        {
            _indexUpList = value;
        }
    }

    public List<Image> IndexDownList
    {
        get
        {
            return _indexDownList;
        }

        set
        {
            _indexDownList = value;
        }
    }

    private void Awake()
    {
        cardGridUP = transform.Find("Up").GetComponent<GridLayoutGroup>();
        cardGridDown = transform.Find("Down").GetComponent<GridLayoutGroup>();
    }

    public void AddCardInTable(string imageName,int index)
    {
        int number=17;
        //后期还需要判断不同的模式在设置牌组
        /*switch (index)
        {
            case 1:
                number = 17;
                break;
            case 2:
                number = 18;
                break;
            case 3:
                number = 17;
                break;
            case 4:
                number = 18;
                break;
        }*/
    
        Sprite sp = Resources.Load("UIs/Card/" + imageName, typeof(Sprite)) as Sprite;

        for(int i=0;i<number;i++)
        {
            Image im;
            cardImage.sprite = sp;
            im =(Image) Canvas.Instantiate(cardImage, cardGridUP.transform);
            im.transform.localScale = new Vector3(1f, 1f, 1f);
            if (im != null)
            {
                im.transform.localScale = new Vector3(1f, 1f, 1f);
                IndexUpList.Add(im);
            }
            im = (Image)Canvas.Instantiate(cardImage, cardGridDown.transform);
            im.transform.localScale = new Vector3(1f, 1f, 1f);
            if (im != null)
            {
                IndexDownList.Add(im);
            }
        }
        //如果是最后一位玩家就让他列表倒序拿牌显示
        if(transform.parent.name=="Last")
        {
            IndexUpList= ListReverseOrder(IndexUpList);
            IndexDownList= ListReverseOrder( IndexDownList);
        }
    }

    List<Image> ListReverseOrder(List<Image> list)
    {
        List<Image> temp=new List<Image>();
        for (int i = 0; i < list.Count; i++)
        {
            temp.Add(list[list.Count - 1 - i]);
        }
        return temp;
    }
}
