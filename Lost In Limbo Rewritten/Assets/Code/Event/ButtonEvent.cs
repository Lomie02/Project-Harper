using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ButtonEvent : MonoBehaviour
{
    [SerializeField] bool m_IsButtonEnabled = true;
    [SerializeField] bool m_OneTimePress = false;
    [SerializeField] UnityEvent m_OnButtonClicked;
    bool m_HasBeenPressed = false;

    public void ActivateButton()
    {
        if (m_HasBeenPressed || !m_IsButtonEnabled)
            return;
        m_OnButtonClicked.Invoke();

        if (m_OneTimePress)
            m_HasBeenPressed = true;
    }

    public void SetButtonState(bool _state)
    {
        m_IsButtonEnabled = _state;
    }
}
