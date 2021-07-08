using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;
public class StartMatch : MonoBehaviour
{

    private Button startButton;

    private void Awake()
    {
        startButton = GetComponent<Button>();

    }
    private void Start()
    {
        startButton.onClick.AddListener(delegate ()
        {
            StartCoroutine( ContinueStart());
        });
    }

    IEnumerator ContinueStart()
    {
        yield return new WaitForEndOfFrame();
        Dictionary<string, string> signal = new Dictionary<string, string>();
        signal.Add("username", "qiushi");
        signal.Add("password", "123456");
        GameObject.Find("ConnectionObject").GetComponent<Connection>().EncodeAndSendMessage(signal);
        GameObject.Find("ConnectionObject").GetComponent<Connection>().EncodeAndSendMessage(signal);
        yield return new WaitForEndOfFrame();
        MahjonManage._instance.StartMatch();
        gameObject.SetActive(false);
    }
}
