using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class CharCode : MonoBehaviour
{
    public static CharCode charCode;

    public int playerCode;
    public int enemyCode;

    private void Awake()
    {
        if (charCode == null)
        {
            DontDestroyOnLoad(gameObject);
            charCode = this;
        }
        else if (charCode != this)
        {
            Destroy(gameObject);
        }
    }
}
