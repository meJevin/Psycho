using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerEvent : MonoBehaviour
{
    public UnityEvent OnTimerDone;

    public float TimeTotal;

    private bool IsGoing;
    private float TimeLeft;

    void Awake()
    {
        IsGoing = false;
    }

    public void StartTimer()
    {
        TimeLeft = TimeTotal;
        IsGoing = true;
    }
    public void StopTimer()
    {
        IsGoing = false;
    }

    void Update()
    {
        if (IsGoing)
        {
            TimeLeft -= Time.deltaTime;

            if (TimeLeft <= 0)
            {
                StopTimer();

                OnTimerDone.Invoke();
            }
        }
    }
}
