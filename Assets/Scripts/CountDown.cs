using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public TextMeshProUGUI time;
    public int timeLimit;
    public int timeLeft;
    public float timeStep = 1;
    public bool isFreeze = false;
    public Transform iconHourglass;
    public Transform iconFreeze;

    private void Awake()
    {
        timeLeft = timeLimit;
        iconHourglass.gameObject.SetActive(true);
        iconFreeze.gameObject.SetActive(false);
        UpdateTime();
    }

    private void Update()
    {
        if(LevelManager.Instance.state == State.Playing)
        {
            if (isFreeze) return;
            timeStep -= Time.deltaTime;
            if(timeStep > 0) return;
            timeStep = 1;
            timeLeft--;
            UpdateTime();
            if (timeLeft <= 0)  LevelManager.Instance.CheckWinCondition();
        }
    }

    public void LoadTime(int time)
    {
        timeLimit = time;
        timeLeft = timeLimit;
        UpdateTime();
    }

    public void UpdateTime()
    {
        int minutes = timeLeft / 60;
        int seconds = timeLeft % 60;
        string secondsString = seconds < 10 ? "0" + seconds : seconds.ToString();
        time.text = minutes + ":" + secondsString;
    }

    public IEnumerator FreezeTime(int time)
    {
        isFreeze = true;
        iconFreeze.gameObject.SetActive(true);
        iconHourglass.gameObject.SetActive(false);
        yield return new WaitForSeconds(time);
        iconFreeze.gameObject.SetActive(false);
        iconHourglass.gameObject.SetActive(true);
        isFreeze = false;
    }

}
