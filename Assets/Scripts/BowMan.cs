using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowMan : MonoBehaviour
{
    [SerializeField] private GameObject arrowObj;
    [SerializeField] private GameObject horse;
    private BackgroundMover _backgroundMover;
    private bool isSpawned = true;
    private Animator animator;
    public float _sliderValue;
    private float _speed;
    private float _distance;
    private float _archerDistance;
    private float _horsePosition;
    private float x;
    private bool doWeNeedInfo = true;

    //0.8 => duration of arrow animation
    //0.72 => duration of shooting animation
    void Start()
    {
        _backgroundMover = FindObjectOfType<BackgroundMover>();
        _sliderValue = _backgroundMover.moveSpeed;
        _horsePosition = horse.transform.position.x;
        _speed = _sliderValue * 23.1f;
        animator = GetComponent<Animator>();
        _distance =  _speed * 0.8f;
        _archerDistance = _speed * .72f;
        //Debug.Log("your position is" + transform.position.x);
        x = Time.time;
    }


    void Update()
    {
        if(Time.time >= x + 1 && doWeNeedInfo)
        {
            //Debug.Log("your position is" + transform.position.x);
            doWeNeedInfo = false;
        }
        if (_horsePosition >= this.transform.position.x - _archerDistance && isSpawned)
        {
            animator.SetTrigger("IsShooting");
            Invoke("ArrowShoot", .72f);
            isSpawned = false;
        }
    }

    private void ArrowShoot()
    {
        arrowObj.SetActive(true);
        arrowObj.transform.position = new Vector2(arrowObj.transform.position.x + _distance, arrowObj.transform.position.y);
    }
}
