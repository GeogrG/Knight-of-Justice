using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public float speed;
    private float offset;
    [SerializeField] float extraSpeed = 1f;
    [SerializeField] float whichLine;
    [SerializeField] GameObject particleExplosion;

    private void Start()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + whichLine);
    }
    void Update()
    {
        offset = Time.deltaTime * speed * extraSpeed * 23.1f;
        transform.position = new Vector2(transform.position.x - offset, transform.position.y);
    }

    public void Die()
    {
        Destroy(gameObject);
        if(gameObject.tag == "ShootingItems")
        {
            GameObject particle = Instantiate(particleExplosion, transform.position, transform.rotation);
            Destroy(particle, 0.5f);
        }
    }
}
