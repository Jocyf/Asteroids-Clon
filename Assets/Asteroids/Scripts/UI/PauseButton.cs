using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{

	public void PressButton ()
    {
        LevelManager.Instance.PauseGame();
        LevelManager.Instance.inGameUI.SetActive(false);
        LevelManager.Instance.pauseMenuPanel.SetActive(true);
    }
	
}
