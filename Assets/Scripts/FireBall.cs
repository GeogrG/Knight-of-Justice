using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed;
    [SerializeField] float extraSpeed = 1;
    void Update()
    { 
        transform.position = new Vector2(transform.position.x + Time.deltaTime * (-speed) * 23.1f * extraSpeed, transform.position.y);
}
}
