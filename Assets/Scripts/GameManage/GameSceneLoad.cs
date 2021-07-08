using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    Start = 0,
    Desktop = 1,
    EedMatch = 2,
    UI_login=3
}
public class GameSceneLoad : MonoBehaviour
{

    public static void LoadScene(SceneName name)
    {
        SceneManager.LoadScene(name.ToString());
    }
}
