using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPiece : MonoBehaviour
{
    SpriteRenderer sprite;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void BreakWall(Vector2 dir)
    {
        gameObject.SetActive(true);

        GetComponent<Rigidbody2D>().AddForce(-dir * 20.0f, ForceMode2D.Impulse);
        StartCoroutine(CR_Erase(1.5f));
    }

    IEnumerator CR_Erase(float _time)
    {
        yield return new WaitForSeconds(_time);
        float progress = 1f;

        while (sprite.color.a > 0.1f)
        {
            sprite.color = new Color(1, 1, 1, progress);
            progress -= Time.deltaTime;
            yield return null;
        }
        GetComponentInParent<Destructible>().DestroyObj();
    }
}