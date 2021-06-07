using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMe : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
