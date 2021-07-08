using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimerPanel : MonoBehaviour
{

    public static TimerPanel _instance;

    private Image _timePointImage;
    private Text _timeText;
 
    private float time;
    private float timer;
    private bool _isOnTimer;
    #region 属性和方法
    public Image TimePointImage
    {
        get
        {
            return _timePointImage;
        }

        set
        {
            _timePointImage = value;
        }
    }
    public Text TimeText
    {
        get
        {
            return _timeText;
        }

        set
        {
            _timeText = value;
        }
    }



    public bool IsOnTimer
    {
        get
        {
            return _isOnTimer;
        }

        set
        {
            _isOnTimer = value;
        }
    }
    #endregion

    private void Awake()
    {
        _instance = this;
        TimePointImage = transform.Find("TimePoint").GetComponent<Image>();
        TimeText = transform.Find("Time").GetComponent<Text>();
    }
    private void Start()
    {
      
    }
    private void Update()
    {
        if (!IsOnTimer) return;
       
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            TimeText.text = ((int)timer).ToString();
            if (timer < 0)
            {
                MahjonManage._instance.OverTimer();
            }
        }
    }
    
    public void UpdateTimer(int player)
    {
        //以后我们需要服务器获得Time的值
        time = 19;
        timer = time;
        IsOnTimer = true;
        TimeText.text = ((int)timer).ToString();
        _timePointImage.sprite = Resources.Load<Sprite>("UIs/Time/TimePoint" + player);
    }

}
