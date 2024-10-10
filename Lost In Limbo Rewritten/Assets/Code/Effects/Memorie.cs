using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Memorie : MonoBehaviour
{
    [SerializeField] bool m_HideOnCollection = false;
    [SerializeField] bool m_DisableOnCollection = false;
    [SerializeField] UnityEvent m_OnMemorieCollected;

    public void CollectMemorie()
    {
        m_OnMemorieCollected.Invoke();

        if (m_DisableOnCollection)
            enabled = false;
        if (m_HideOnCollection)
            gameObject.SetActive(false);
    }
}
