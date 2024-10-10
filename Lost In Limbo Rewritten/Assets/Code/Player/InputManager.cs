using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] Camera m_PlayersCamera;
    PlayerController m_PlayersController;
    [SerializeField] Text m_InteractText;
    bool m_AllowedInput = true;

    private void Start()
    {
        m_InteractText.gameObject.SetActive(false);
        m_PlayersController = GetComponent<PlayerController>();
    }
    void Update()
    {
        if (!m_AllowedInput)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractInputReact();
        }

        ScanForInteraction();
    }

    void ScanForInteraction()
    {
        RaycastHit m_RayData;
        if (Physics.Raycast(m_PlayersCamera.transform.position, m_PlayersCamera.transform.forward, out m_RayData, 2))
        {
            if (m_RayData.collider.GetComponent<DoorModule>() != null)
            {
                DoorModule DoorMod = m_RayData.collider.GetComponent<DoorModule>();
                m_InteractText.gameObject.SetActive(true);

                if (DoorMod.IsDoorLocked())
                    m_InteractText.text = "Locked.";
                else
                {
                    if (DoorMod.GetDoorState())
                        m_InteractText.text = "PRESS [E] TO CLOSE.";
                    else
                        m_InteractText.text = "PRESS [E] TO OPEN.";
                }
            }
            else if (m_RayData.collider.GetComponent<Memorie>() != null)
            {
                m_InteractText.gameObject.SetActive(true);
                m_InteractText.text = "PRESS [E] TO COLLECT MEMORY.";
            }
            else if (m_RayData.collider.tag == "Medicine")
            {
                m_InteractText.gameObject.SetActive(true);
                if (m_PlayersController.IsHealthKitsFull())
                    m_InteractText.text = "INVENTORY FULL.";
                else
                    m_InteractText.text = "PRESS [E] TO TAKE MEDICINE.";

            }
            else if (m_RayData.collider.GetComponent<FuseSwitch>() != null)
            {
                m_InteractText.text = "PRESS [E] TO FLIP SWITCH.";
                m_InteractText.gameObject.SetActive(true);
            }
            else if (m_RayData.collider.GetComponent<ButtonEvent>() != null)
            {
                m_InteractText.text = "PRESS [E] TO INTERACT";
                m_InteractText.gameObject.SetActive(true);
            }
            else
            {
                m_InteractText.gameObject.SetActive(false);
            }
        }
        else
        {
            m_InteractText.gameObject.SetActive(false);
        }
    }

    void InteractInputReact()
    {
        RaycastHit m_RayData;
        if (Physics.Raycast(m_PlayersCamera.transform.position, m_PlayersCamera.transform.forward, out m_RayData, 2))
        {
            if (m_RayData.collider.GetComponent<DoorModule>() != null)
            {
                m_RayData.collider.GetComponent<DoorModule>().CycleDoorStates();
            }
            else if (m_RayData.collider.GetComponent<Memorie>() != null)
            {
                m_RayData.collider.GetComponent<Memorie>().CollectMemorie();
            }
            else if (m_RayData.collider.GetComponent<FuseSwitch>() != null)
            {
                m_RayData.collider.GetComponent<FuseSwitch>().FlickSwitch();
            }
            else if (m_RayData.collider.GetComponent<ButtonEvent>() != null)
            {
                m_RayData.collider.GetComponent<ButtonEvent>().ActivateButton();
            }
            else if (m_RayData.collider.tag == "Medicine")
            {
                if (!m_PlayersController.IsHealthKitsFull())
                {
                    m_RayData.collider.gameObject.SetActive(false);
                    m_PlayersController.AddHealthKit();
                }

            }
        }
    }
}
