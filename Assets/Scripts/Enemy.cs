using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.GetComponent<MovingBall>()!=null)
        {
            Destroy(gameObject);
            return;
        }else if (collision.collider.GetComponent<Enemy>() != null)
        { return; }
        if(collision.contacts[0].normal.y < -0.5)
        { Destroy(gameObject); }
    }
}
