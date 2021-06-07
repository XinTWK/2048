using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPanel : View
{

    public Slider slider_sound;
    public Slider slider_music;

    //關閉按鈕
    public void OnBtnCloseClick()
    {
        //當前這個介面隱藏
        Hide();
    }

    //音效
    public void OnSoundValueChange(float f)
    {
        //修改音效的大小
        AudioManager._instance.OnSoundVolumeChange(f);
        //保存當前修改
        PlayerPrefs.SetFloat(Const.Sound,f);
    }

    //音樂
    public void OnMusicValueChange(float f)
    {
        // 修改音樂的大小
        AudioManager._instance.OnMusicVolumeChange(f);
        // 保存當前修改
        PlayerPrefs.SetFloat(Const.Music,f);
    }

    public override void Show()
    {
        base.Show();
        //對介面進行初始化
        slider_sound.value = PlayerPrefs.GetFloat(Const.Sound,0);
        slider_music.value = PlayerPrefs.GetFloat(Const.Music,0);


    }

}
