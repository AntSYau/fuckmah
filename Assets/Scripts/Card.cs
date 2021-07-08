using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Card 
{
    private string _name;//牌的名字
    private int _size;//牌的数字
    private string _cardMark;//牌的类型
    private string _imageName;
    private string _soundName;
    #region 属性方法
    public string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = value;
        }
    }

    public int Size
    {
        get
        {
            return _size;
        }

        set
        {
            _size = value;
        }
    }

    public string CardMark
    {
        get
        {
            return _cardMark;
        }

        set
        {
            _cardMark = value;
        }
    }

    public string ImageName
    {
        get
        {
            return _imageName;
        }

        set
        {
            _imageName = value;
        }
    }

    public string SoundName
    {
        get
        {
            return _soundName;
        }

        set
        {
            _soundName = value;
        }
    }
    #endregion
    public Card()
    {
        
    }
    public Card(string name,int size,string cardMark,string imageName,string soundName)
    {
        this.Name = name;
        this.Size = size;
        this.CardMark = cardMark;
        this.ImageName = imageName;
        this.SoundName = soundName;
    }

}
