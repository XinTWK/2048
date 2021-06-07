using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    //顯示
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    //隱藏
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
