using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneUI : MonoBehaviour
{
    public SceneName sn;

    void Awake()
    {
    
    }

    void Start()
    {
        Button bt = transform.GetComponent<Button>();
        bt.onClick.AddListener(() => GameSceneLoad.LoadScene(sn));
    }
  
}
