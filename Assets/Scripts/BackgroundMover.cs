using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMover : MonoBehaviour {

    Renderer rd;
    public float moveSpeed;

    void Start()
    {
       rd = GetComponent<Renderer>();
    }

    void Update()
    {
        Moving();
    }

    private void Moving()
    {
        var offSet = Time.deltaTime * moveSpeed;
        rd.material.mainTextureOffset += new Vector2(-offSet, 0f);
    }
}
