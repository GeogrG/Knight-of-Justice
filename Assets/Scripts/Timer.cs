using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Tooltip("Max game time in seconds!")]
    [SerializeField] float seconds;
    [SerializeField] GameObject finishScreen;
    public float sliderValue;

    float a = 0;
    public bool isLevelStarted = false;

    public void CalculateTime()
    {
        seconds *= (1 - sliderValue);
        seconds += 15f;
    }
    // Update is called once per frame
    void Update()
    {
        if (isLevelStarted)
        {
            a += Time.deltaTime;
            GetComponent<Slider>().value = a / seconds;

            bool timerFinished = (a >= seconds);
            if (timerFinished)
            {
                isLevelStarted = false;
                finishScreen.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
}
