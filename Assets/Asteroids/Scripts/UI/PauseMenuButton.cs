using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButton : MonoBehaviour
{

    public enum PauseMenuType { CONTINUE = 0, MAINMENU = 1 }

    public PauseMenuType pauseMenuTypeButton = PauseMenuType.CONTINUE;


    public void ButtonPressed()
    {
        switch (pauseMenuTypeButton)
        {
            case PauseMenuType.CONTINUE:
                LevelManager.Instance.inGameUI.SetActive(true);
                LevelManager.Instance.pauseMenuPanel.SetActive(false);
                LevelManager.Instance.UnPauseGame();
                break;
            case PauseMenuType.MAINMENU:
                LevelManager.Instance.inGameUI.SetActive(false);
                LevelManager.Instance.pauseMenuPanel.SetActive(false);
                LevelManager.Instance.ExitGame();
                break;
        }
    }

}
