using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBehaviour : MonoBehaviour
{
    [Header("Main Referent Objects")]
    [SerializeField] private GameObject fireBall;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private GameObject horse;
    [SerializeField] private float _speedMultiplyer;

    //Inner variables of the NPC
    private BackgroundMover _backgroundMover;
    private Animator anim;
    private BoxCollider2D box;
    private float _sliderValue;
    private float _speed;
    private float _mageDistance;
    private float _horsePosition;
    private float x;
    private bool doWeNeedInfo = true;
    private bool isSpawned = true;

    void Start()
    {
        _backgroundMover = FindObjectOfType<BackgroundMover>();
        _sliderValue = _backgroundMover.moveSpeed;
        _horsePosition = horse.transform.position.x;
        _speed = _sliderValue * 23.1f;
        anim = GetComponent<Animator>();
        _mageDistance = _speed * (1+ _speedMultiplyer);
        //Debug.Log("your position is" + transform.position.x);
        x = Time.time;
        box = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (Time.time >= x + 1 && doWeNeedInfo)
        {
            Debug.Log("your position is" + transform.position.x);
            doWeNeedInfo = false;
        }
        if (_horsePosition >= this.transform.position.x - _mageDistance && isSpawned)
        {
            anim.SetTrigger("IsFiring");
            anim.SetFloat("AnimSpeed", 1 + _sliderValue * 0.6f);
            isSpawned = false;
        }
    }

    private void ShootTheFireBall()
    {
        GameObject fire = Instantiate(fireBall, spawnPos.transform.position, transform.rotation);
        fire.GetComponent<ObstacleMover>().speed = FindObjectOfType<BackgroundMover>().moveSpeed;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void RemoveCollider()
    {
        box.enabled = false;
    }
}
