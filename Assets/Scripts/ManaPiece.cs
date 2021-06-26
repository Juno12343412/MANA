using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPiece : MonoBehaviour
{
    public float moveSpeed = 20;
    private int _name;
    void Start()
    {
        string temp = gameObject.name;
        int.TryParse(temp, out _name);
        _name = _name % 2;
    }

    void Update()
    {
        if(_name == 0)
        {
            transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime);
            transform.Rotate(0, 0, 100 * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime);
            transform.Rotate(0, 0, -100 * Time.deltaTime);
        }
    }
}
