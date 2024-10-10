using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] float m_Duration = 5;
    [SerializeField] bool m_IsTimerEnabled = false;
    [Space]

    [SerializeField] UnityEvent m_OnTimerEnd;
    float m_TimerClock;

    void Start()
    {
        m_TimerClock = m_Duration;
    }

    void FixedUpdate()
    {
        if (m_IsTimerEnabled)
        {
            m_TimerClock -= Time.fixedDeltaTime;

            if (m_TimerClock <= 0)
            {
                m_OnTimerEnd.Invoke();
                m_IsTimerEnabled = false;
            }
        }
    }

    public void StartTimer()
    {
        m_IsTimerEnabled = true;
    }
}
