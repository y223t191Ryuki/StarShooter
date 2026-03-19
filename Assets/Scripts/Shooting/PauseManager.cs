using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject pausePanel;

    private bool isPaused = false;

    [SerializeField] private GameObject firstSelectedButton;

    void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    // InputSystem から呼ばれる
    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (isPaused)
            Resume();
        else
            Pause();
    }

    void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ResetTime(){
        Time.timeScale = 1f;
    }
}