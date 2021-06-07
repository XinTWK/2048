using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : View
{
    // 重新開始按鈕的點擊事件
    public void OnRestartClick()
    {
        // 調用GamePanel 裡面的重新開始
        GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>().ReStartGame();
        // 隱藏當前頁面
        Hide();
    }

    // 退出按鈕的點擊事件
    public void OnExitClick()
    {
        // 退出到主選單場景
        SceneManager.LoadSceneAsync(0);
    }

}
