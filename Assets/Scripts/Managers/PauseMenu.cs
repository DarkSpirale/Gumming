using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject menu;

    private float timeScaleBeforePause;

    private bool isPaused;
    private bool pauseNextFrame;
    private bool oneFrameWaited;
    private bool wasMenuShown;

    private void Update()
    {
        if (oneFrameWaited)
        {
            oneFrameWaited = false;
            Pause();

            if (wasMenuShown)
            {
                wasMenuShown = false;
                ShowMenu();
            }
        }

        if (pauseNextFrame)
        {
            oneFrameWaited = true;
            pauseNextFrame = false;
        }

    }

    #region Pause & Resume Functions
    private void Pause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);

        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;
    }

    private void Resume()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        HideMenu();

        Time.timeScale = timeScaleBeforePause;
    }

    public void ResumeForOneFrame()
    {
        pauseNextFrame = true;
        wasMenuShown = IsMenuShown();
        Resume();
    }
    #endregion

    #region Menu Functions
    private void ShowMenu() => menu.SetActive(true);

    private void HideMenu() => menu.SetActive(false);

    private bool IsMenuShown()
    {
        return menu.activeSelf;
    }
    #endregion

    #region Input Functions
    public void InputForPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void InputForPauseWithMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isPaused)
            {
                if (IsMenuShown())
                    Resume();
                else
                    ShowMenu();
            }
            else
            {
                Pause();
                ShowMenu();
            }
        }
    }

    public void InputForResume(InputAction.CallbackContext context)
    {
        if (isPaused && context.started)
        {
            bool pointerOverUI = RaycastUtilities.PointerIsOverUI(Input.mousePosition);

            // We want to maintain the pause if the click is on UI or if an action is set to a Gumming
            if (AbilityManager.instance.CurrentGumming == null && !pointerOverUI) 
                Resume();
        }
    }

    public void InputForResumeOneFrame(InputAction.CallbackContext context)
    {
        if(isPaused && context.started)
        {
            ResumeForOneFrame();
        }
    }
    #endregion
}
