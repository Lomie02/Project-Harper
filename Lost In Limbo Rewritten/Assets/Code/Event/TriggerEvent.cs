using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] UnityEvent m_OnActivate;
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_OnActivate.Invoke();
            gameObject.SetActive(false);
        }
    }
}
