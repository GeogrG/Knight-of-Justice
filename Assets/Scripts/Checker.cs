using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boxes")
        {
            FindObjectOfType<Instantiate>().isCoroutineReady = true;
            Debug.Log("The collision has been seen");
        }
    }
}
