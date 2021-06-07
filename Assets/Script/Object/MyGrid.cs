using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid : MonoBehaviour
{
    public Number number; //當前這個格子的數字

    //判斷是否有數字
    public bool IsHaveNumber()
    {
        return number != null;
    }

    //獲取這個格子的數字
    public Number GetNumber()
    {
        return number;
    }

    //設置數字
    public void SetNumber (Number number)
    {
        this.number = number;
    }

}
