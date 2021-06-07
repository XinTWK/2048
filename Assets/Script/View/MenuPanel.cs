using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    public SelectModelPanel selectModelPanel;
    public SetPanel setPanel;

    public AudioClip bgClip;

    private void Start()
    {
        AudioManager._instance.PlayMusic(bgClip);
    }


    //點擊開始遊戲
    public void OnStartGameClick()
    {
        selectModelPanel.Show();
    }

    //點擊設定
    public void OnSetClick()
    {
        setPanel.Show();
    }

    //點擊退出遊戲
    public void OnExistClick()
    {
        Application.Quit();
    }
}
