using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPiece : MonoBehaviour
{
    Vector2[] point = new Vector2[4];

    [SerializeField] [Range(0, 1)] private float t = 0;
    [SerializeField] public float spd = 5;
    [SerializeField] public float posA = 0.55f;
    [SerializeField] public float posB = 0.45f;
    [SerializeField] public Transform _posA = null;
    public GameObject start;

    Vector3 originPos;
    bool finsh = false;
    private int _name;
    void Start()
    {
        originPos = transform.position;
        string temp = gameObject.name;
        int.TryParse(temp, out _name);
        _name = _name % 2;

        if (start != null)
        {
            point[0] = start.transform.position; // P0
            point[1] = PointSetting(start.transform.position); // P1
            point[2] = PointSetting(originPos); // P2
            point[3] = originPos; // P3
            transform.position = start.transform.position;
        }
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, originPos) <= 1)
        {
            finsh = true;
        }
        if (finsh)
        {
            //GetComponent<TrailRenderer>().enabled = false;
            if (_name == 0)
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

    void FixedUpdate()
    {
        if (start != null)
        {
            if (t > 1) return;
            if (finsh) return;
            t += Time.deltaTime * spd;
            DrawTrajectory();
        }
    }

    Vector2 PointSetting(Vector2 origin)
    {
        float x, y;
        if (_posA == null)
        {
            x = posA * Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad)
                + origin.x;
            y = posB * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad)
            + origin.y;
            return new Vector2(x, y);
        }
        else
        {
            return new Vector2(_posA.position.x, _posA.position.y);
        }

        
    }

    void DrawTrajectory()
    {
        transform.position = new Vector2(
            FourPointBezier(point[0].x, point[1].x, point[2].x, point[3].x),
            FourPointBezier(point[0].y, point[1].y, point[2].y, point[3].y)
        );
    }




    private float FourPointBezier(float a, float b, float c, float d)
    {
        return Mathf.Pow((1 - t), 3) * a
            + Mathf.Pow((1 - t), 2) * 3 * t * b
            + Mathf.Pow(t, 2) * 3 * (1 - t) * c
            + Mathf.Pow(t, 3) * d;
    }


}
