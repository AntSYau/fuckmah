using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FanShow : MonoBehaviour
{
    [SerializeField] Transform Content;
    [SerializeField] GameObject prefeb;
    private bool[] fanDetail = new bool[50];
    // Use this for initialization

    public FanShow()
    {

    }

    void Start()
    {
        Get();
        UpdatePrefabs();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Get()
    {
        string str = PlayerPrefs.GetString("fan");
        ToList(str);
    }

    void ToList(string str)
    {
        if (str.Length > fanDetail.Length) return;
        for (int i = 0; i < str.Length; i++) // I started i = 0, vs. yours i =1
        {
            fanDetail[i] = str.Substring(i, 1)[0] == '1' ? true : false;
        }
    }

    void UpdatePrefabs()
    {
        for (int i = 0; i < fanDetail.Length; i++)
        {
            if (fanDetail[i])
            {
                GameObject cell = Instantiate(prefeb);  //实例化预制体
                cell = init(cell, i);                                //实现按钮数据的初始化方法
                cell.transform.SetParent(Content);      //指定预制体父控件（把Cell放到）表格里
            }
        }
    }

    public GameObject init(GameObject cell, int i)
    {
        Debug.Log(i + ":" + fanDetail[i]);
        if (fanDetail[i] == true)
        {
            cell.GetComponent<Text>().text = "QueMen 2";
            cell.GetComponent<Text>().fontSize = 25;
            Debug.Log("sample");
            return cell;
        }
        cell.GetComponent<Text>().text = "Sample 1";
        cell.GetComponent<Text>().fontSize = 25;
        return cell;
    }
}
