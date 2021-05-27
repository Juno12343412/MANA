using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    bool gameStart = false;

    private void Awake()
    {
        Cursor.visible = false;
        gameStart = true;
    }
    void OnApplicationQuit()
    {
        Cursor.visible = true;
        gameStart = false;
    }
}
