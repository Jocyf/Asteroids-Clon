using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Main Player Data")]
    public int numlives = 3;
    public int score = 0;
    public int numLevel = 1;
    [Disable]
    public PlayerShip playerShipSrc;

    [Header("InGame UI")]
    public Text scoreText;
    public Text levelText;
    public Image[] liveImages;
    public Text gameOverText;
    [Space(10)]
    public GameObject gameContainer;
    public GameObject inGameUI;
    public GameObject pauseMenuPanel;



    #region Singleton
    public static LevelManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion


    public void StartPlay()
    {
        ResetPlayer();
        AsteroidsManager.Instance.StartPlaying();
        UFOManager.Instance.StartPlaying();
    }

    public void AddScore(int _score)
    {
        score += _score;
        UpdateScoreInScreen();

        if (score % 10000 == 0) { AddExtraLive(); }
    }

    public void AddExtraLive()
    {
        numlives++;
        UpdateLivesInScreen();
        // Todo: Falta el sonido de vida extra (missing extra life sound fx)
    }

    public void Die()
    {
        numlives--;
        if (numlives <= 0)
        {
            Debug.Log("Game Over");
            GameOver();
        }
        else
        {
            Debug.Log("Player Died. Remaining Lives: "+numlives);
        }

        UpdateLivesInScreen();
    }

    public bool IsLevelFinished() { return AsteroidsManager.Instance.IsZeroAsteroids() && !UFOManager.Instance.IsUFOOnScreen(); }

    public void LevelFinished()
    {
        if (numlives <= 0)
        {
            Debug.Log("Game Over");
            GameOver();
            return;      // Check we dont die crashing with the last enemy. This is also GameOver
        }

        Debug.Log("Player Finished Level");
        UFOManager.Instance.ResetUFO();  

        StartCoroutine("UpdateLevelTimed"); // Start Next Level.
    }

    // Se llama desde el menu de pausa para volver al menu principal
    // It's called from pause Popup in game to return to main menu.
    public void ExitGame()
    {
        UnPauseGame();
        playerShipSrc.MakeShipInvulnerable(true);
        StartCoroutine("_LoadMainMenuTimed", 0f);
    }


    private void GameOver()
    {
        UFOManager.Instance.ResetUFO();
        gameOverText.enabled = true;
        StartCoroutine("_LoadMainMenuTimed", 5f);
    }


    private IEnumerator _LoadMainMenuTimed(float _time)
    {
        /**/
        yield return new WaitForSeconds(_time);
        GameManager.Instance.LoadMainMenu();
    }

    // Start Playing new level
    private IEnumerator UpdateLevelTimed()
    {
        yield return new WaitForSeconds(4.0f);
        numLevel++;
        UpdateLevelInScreen();
        AsteroidsManager.Instance.StartPlaying();
        UFOManager.Instance.StartPlaying();
    }


    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
    }

    void Start ()
    {
        playerShipSrc = FindObjectOfType<PlayerShip>();
        ResetPlayer();
    }

    private void ResetPlayer()
    {
        numlives = 3;
        score = 0;
        numLevel = 1;
        UpdateScoreInScreen();
        UpdateLivesInScreen();
        UpdateLevelInScreen();
        gameOverText.enabled = false;
    }

    private void ResetUIPanels()
    {
        gameContainer.SetActive(false);
        inGameUI.SetActive(false);
        pauseMenuPanel.SetActive(false);
    }

    private void UpdateScoreInScreen()
    {
        scoreText.text = score.ToString();
    }

    private void UpdateLivesInScreen()
    {
        for(int i = 0; i < 3; i++)
        {
            liveImages[i].enabled = i < numlives;
        }
    }

    private void UpdateLevelInScreen()
    {
        levelText.text = string.Format("{0:00}", numLevel);
    }
}
