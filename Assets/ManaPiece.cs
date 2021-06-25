using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPiece : MonoBehaviour
{
    public float moveSpeed = 20;
    private Vector2[] vec2Dir = new Vector2[8];
    private int name;
    void Start()
    {
        vec2Dir[0] = new Vector2(1, 0);
        vec2Dir[1] = new Vector2(1, 1);
        vec2Dir[2] = new Vector2(0, 1);
        vec2Dir[3] = new Vector2(-1, 1);
        vec2Dir[4] = new Vector2(-1, 0);
        vec2Dir[5] = new Vector2(-1, -1);
        vec2Dir[6] = new Vector2(0, -1);
        vec2Dir[7] = new Vector2(1, -1);
        name = int.Parse(gameObject.name);
        name = name % 2;
        
    }

    void Update()
    {
        if(name == 0)
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
