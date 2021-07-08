using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HandCard : MonoBehaviour
{
    private Image _handCardImage;
    private RectTransform _rectTF;
    private Card _card;

    #region 属性方法
    public Image HandCardImage
    {
        get
        {
            return _handCardImage;
        }

        set
        {
            _handCardImage = value;
        }
    }

    public Card Card
    {
        get
        {
            return _card;
        }

        set
        {
            _card = value;
        }
    }

    public RectTransform RectTF
    {
        get
        {
            return _rectTF;
        }

        set
        {
            _rectTF = value;
        }
    }

    #endregion

    private void Awake()
    {
        HandCardImage = transform.Find("Card").GetComponent<Image>();
        RectTF = transform.Find("Card").GetComponent<RectTransform>();
    }

    private void Start()
    {
        InitLocation();
    }

    public void InitLocation()
    {
        RectTF.sizeDelta = transform.parent.GetComponent<GridLayoutGroup>().cellSize;
        transform.localRotation = Quaternion.identity;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

}
