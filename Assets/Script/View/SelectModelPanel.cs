using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectModelPanel : View
{
    public void OnSelectModelClick(int count)
    {
        //選擇模式
        PlayerPrefs.SetInt(Const.GameModel, count);
        //跳轉場景 到 遊戲場景
        SceneManager.LoadSceneAsync(1);
    }

    ////顯示
    //public void Show()
    //{
    //    gameObject.SetActive(true);
    //}

    ////隱藏
    //public void Hide()
    //{
    //    gameObject.SetActive(false);
    //}

}
