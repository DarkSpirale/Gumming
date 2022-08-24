using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Gumming"))
        {
            GameManager.instance.AddGummingSaved();
            Destroy(collision.gameObject);
        }
    }
}
