using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour
{
    public GameObject hitSlasher;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Enemy") || collision.tag.Equals("Player"))
        {
            Instantiate(hitSlasher, new Vector3(transform.position.x, transform.position.y, -0.4f), Quaternion.identity);
        }
    }
}
