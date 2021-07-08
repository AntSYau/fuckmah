using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touzi : MonoBehaviour
{
    public static Touzi _instancce;
    private UGUIAnimation _touziOne;
    private UGUIAnimation _touziTwo;


    public UGUIAnimation TouziOne
    {
        get
        {
            return _touziOne;
        }

        set
        {
            _touziOne = value;
        }
    }

    public UGUIAnimation TouziTwo
    {
        get
        {
            return _touziTwo;
        }

        set
        {
            _touziTwo = value;
        }
    }

    private void Awake()
    {
        _instancce = this;
        TouziOne = transform.Find("1").GetComponent<UGUIAnimation>();
        TouziTwo = transform.Find("2").GetComponent<UGUIAnimation>();
    }
    
    public void PlayerAllAnimation()
    {
        TouziOne.Play();
        TouziTwo.PlayReverse();
    }
    public void TouziShow(bool value)
    {
        TouziOne.gameObject.SetActive(value);
        TouziTwo.gameObject.SetActive(value);
    }



}
