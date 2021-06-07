using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;

    private AudioSource bg,sound;

    private void Awake()
    {

        DontDestroyOnLoad(this.gameObject);
 
        _instance = this;

        bg = transform.Find("bg").GetComponent<AudioSource>();
        sound = transform.Find("sound").GetComponent<AudioSource>();

        // 獲取到保存聲音大小
        bg.volume = PlayerPrefs.GetFloat(Const.Music, 0.5f);
        sound.volume = PlayerPrefs.GetFloat(Const.Sound, 0.5f);

    }



    public void PlayMusic(AudioClip audioClip)
    {
        bg.clip = audioClip;
        bg.loop = true;
        bg.Play();
    }

    public void PlaySound(AudioClip audioClip)
    {
        sound.PlayOneShot(audioClip);
    }

    public void OnMusicVolumeChange(float value)
    {
        bg.volume = value;
    }

    public void OnSoundVolumeChange(float value)
    {
        sound.volume = value;
    }
}
