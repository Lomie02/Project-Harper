using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorModule : MonoBehaviour
{
    [Header("General")]
    [SerializeField] bool m_IsDoorLocked = false;

    [SerializeField] Animator m_DoorAnimator;
    [SerializeField] bool m_DoorState = true;

    [Header("Audio")]
    [SerializeField] AudioClip[] m_AudioClips = new AudioClip[2];
    AudioSource m_AudioSource;

    /*
        0 = Open Sound
        1 = Close Sound
     
     */

    private void Start()
    {
        m_DoorAnimator = GetComponent<Animator>();

        // Set Up Audio Source
        gameObject.AddComponent<AudioSource>();
        m_AudioSource = GetComponent<AudioSource>();

        m_AudioSource.spatialBlend = 1;
        m_AudioSource.rolloffMode = AudioRolloffMode.Linear;
        m_AudioSource.maxDistance = 10f;

        UpdateDoorState();
    }
    public bool GetDoorState()
    {
        return m_DoorState;
    }
    public bool IsDoorLocked()
    {
        return m_IsDoorLocked;
    }
    void UpdateDoorState()
    {
        if (!m_DoorState)
        {
            m_DoorState = true;
            m_AudioSource.clip = m_AudioClips[1];
            m_AudioSource.Play();
            m_DoorAnimator.SetBool("DoorState", m_DoorState);
        }
        else
        {
            m_DoorState = false;
            m_AudioSource.clip = m_AudioClips[0];
            m_AudioSource.Play();
            m_DoorAnimator.SetBool("DoorState", m_DoorState);
        }
    }

    public void LockDoor()
    {
        m_DoorState = false;
        m_DoorAnimator.SetBool("DoorState", m_DoorState);

        m_IsDoorLocked = true;
    }

    public void UnlockDoor()
    {
        m_IsDoorLocked = false;
    }

    public void CycleDoorNormal()
    {
        CycleDoorStates();
    }

    public bool CycleDoorStates()
    {
        if (m_IsDoorLocked)
            return false;

        if (!m_DoorState)
        {
            m_DoorState = true;
            m_AudioSource.clip = m_AudioClips[0];
            m_AudioSource.Play();
            m_DoorAnimator.SetBool("DoorState", m_DoorState);
        }
        else
        {
            m_DoorState = false;
            m_AudioSource.clip = m_AudioClips[1];
            m_AudioSource.Play();
            m_DoorAnimator.SetBool("DoorState", m_DoorState);
        }


        return true;
    }
}
