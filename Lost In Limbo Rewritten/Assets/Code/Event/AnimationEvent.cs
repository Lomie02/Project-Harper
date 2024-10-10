using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// For Animation Events on an animator controllers.
public class AnimationEvent : MonoBehaviour
{
    [SerializeField] string m_EventName;
    [SerializeField] UnityEvent m_OnTrigger;
    public void EventTrigger()
    {
        m_OnTrigger.Invoke();
    }
}
