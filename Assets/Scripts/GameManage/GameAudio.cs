using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameAudio : MonoBehaviour
{
    private static GameAudio _instance;

    public static GameAudio instance
    {
        get
        {
            return _instance;
        }
    }


    private Dictionary<string, AudioClip> audios;  //创建一个词典 存放我们的所有声音

    private AudioSource[] audioSources; //包含 背景音效 UI音效 指定音效 
    private AudioSource audioSourceUI;
    private AudioSource audioSourceBG;
    private AudioSource audioSourceRole;

    void Awake()
    {
        _instance = this;
        
        audios = new Dictionary<string, AudioClip>();
        //动态加载 资源文件必须在Resources目录下哦
        AudioClip[] audioArray = Resources.LoadAll<AudioClip>("Audio"); //取得指定目录下所有声音并加入声音数组

        audioSources = GetComponents<AudioSource>();

        audioSourceUI = audioSources[0]; //得到默认的UI音效组件
        audioSourceBG = audioSources[1]; //得到默认的BG音效组件
        audioSourceRole= audioSources[2];//得到默认的Role音效组件

        audioSourceBG.loop = true;

        // 用foreach 遍历声音数组里面的声音  并放入字典 
        foreach (AudioClip a in audioArray)
        {
            audios.Add(a.name, a);            
        }
        PlayaudioSourceBG("bg-music");

    }

    public void PlayaudioSourceUI(string name)
    {
        if (name == "")
        {
            name = "Button";
        }
        if (audios.ContainsKey(name))
        {
            audioSourceUI.clip = audios[name];
            audioSourceUI.Play();
        }
    }
    public void PlayaudioSourceBG(string name)
    {
        if (audios.ContainsKey(name))
        {
            audioSourceBG.clip = audios[name];
            audioSourceBG.Play();
        }
    }
    public void PlayaudioSourceRole(string name)
    {
        if (audios.ContainsKey(name))
        {
            audioSourceRole.clip = audios[name];
            audioSourceRole.Play();
        }
    }
    public void PlayaudioSourceAuto(string name,Vector3 pos)
    {
        if (audios.ContainsKey(name) && audioSourceRole.volume>0)
        {
            AudioSource.PlayClipAtPoint(audios[name], pos);
        }
    }
    public void CloseaudioSourceUI(float f) {
        audioSourceUI.volume = f;
    }
    public void CloseaudioSourceBG(float f)
    {
        audioSourceBG.volume = f ; 
    }
    public void CloseaudioSourceRole(float f)
    {
        audioSourceRole.volume = f;
    }

}

