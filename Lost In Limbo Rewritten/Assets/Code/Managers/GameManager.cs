using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] UnityEvent m_OnConfigure;
    [SerializeField] UnityEvent m_OnExit;
    [SerializeField] bool m_ScanForInput = true;

    GameObject m_PauseMenu;
    bool m_IsPaused = false;
    bool m_TutorialShowing = false;
    void Start()
    {
        m_PauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");

        m_OnConfigure.Invoke();
        if (m_PauseMenu)
            m_PauseMenu.SetActive(false);
    }

    public void TutorialPopup()
    {
        m_TutorialShowing = true;

        Time.timeScale = 0;

        ShowCursor();
    }

    public void ResetTimeScale()
    {
        Time.timeScale = 1;
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void TutorialHide()
    {
        m_TutorialShowing = false;

        Time.timeScale = 1;

        HideCursor();
    }

    public void Update()
    {
        if (!m_ScanForInput)
            return;

        if (Input.GetKeyDown(KeyCode.Escape) && !m_PauseMenu)
        {
            QuitGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && m_PauseMenu)
        {
            CyclePauseState();
        }
    }

    public void ChangeScene(string _name)
    {
        SceneManager.LoadScene(_name);
    }

    public void PauseGame()
    {
        if (m_TutorialShowing)
            return;

        m_IsPaused = true;
        Time.timeScale = 0;

        if (m_PauseMenu)
            m_PauseMenu.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        if (m_TutorialShowing)
            return;

        m_IsPaused = false;
        Time.timeScale = 1;

        if (m_PauseMenu)
            m_PauseMenu.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void CyclePauseState()
    {
        if (m_IsPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void ScanInputState(bool _state)
    {
        m_ScanForInput = _state;
    }

    public void QuitGame()
    {
        ExitConfig();
        Application.Quit();
    }

    public void ExitConfig()
    {
        m_OnExit.Invoke();
    }
}
