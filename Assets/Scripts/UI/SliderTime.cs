using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTime : MonoBehaviour
{
    public Slider slider;
    public float time;
    public Text txtTime;
    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = time;
        slider.value = time;
        UpdateTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (SystemVariable.gameController._state == stateOfGame.Play.ToString())
        {
            time -= Time.deltaTime;
            slider.value = time;
            UpdateTime();
        }
    }

    public void UpdateTime()
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        txtTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
