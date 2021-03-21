using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = false;
    }
    void OnApplicationQuit()
    {
        Cursor.visible = true;
    }
}
