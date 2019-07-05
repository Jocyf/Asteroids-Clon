using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int frameRate = 30;

    #region Singleton
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion


    public void LoadMainMenu(){ SceneManager.LoadScene(1); }
    public void LoadClassicGameScene(){ SceneManager.LoadScene(2); }


    void Start()
    {
        Application.targetFrameRate = frameRate;
    }

}
