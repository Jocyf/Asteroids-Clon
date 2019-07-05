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
        // Falta el sonido de vida extra
    }

    public void Die()
    {
        numlives--;
        //AsteroidsManager.Instance.ratio = 1f;     // Reiniciamos el ratio.
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
        //string displayString = string.Format("{0:00}", numLevel);
        levelText.text = string.Format("{0:00}", numLevel);
    }


    #region GooglePlay Section

    // Sending data to Leaderboards & achievements.
    /*private void ReportToGooglePlayGames()
    {
        #if UNITY_ANDROID
        StartCoroutine("SubmitScoreGooglePlayAndroid", score);
        #elif UNITY_IPHONE
		StartCoroutine ("SubmitScoreGooglePlayIOS" , score);
        #endif
        //if (currentLevel % 5 == 0)
        //    ReportAchievement();
    }

    private IEnumerator SubmitScoreGooglePlayAndroid(int _score)
    {
        #if UNITY_ANDROID
        Debug.Log("Submitting Score: " + _score);// + " Level: " + currentLevel);
        yield return new WaitForSeconds(1); 

        GooglePlayManager.Instance.SubmitScore(ConnectGooglePlay.LEADERBOARD_FAST_NAME, _score);
        #endif
    }

    private void SubmitScoreGooglePlayIOS(int _score)
    {
        #if UNITY_IPHONE
		Debug.Log("Submitting Score: " + _score + " Level: " + ArkanoidManager.Instance.currentLevel);
        yield return new WaitForSeconds(1); 
        GooglePlayManager.Instance.SubmitScore (ConnectGooglePlay.LEADERBOARD_FAST_ID, _score);
        #endif
    }*/

    /*private void ReportAchievement()
    {

        Debug.Log("Reportando logro a GooglePlay. Level: " + ArkanoidManager.Instance.currentLevel);
        switch (ArkanoidManager.Instance.currentLevel)
        {
            case 10:
                #if UNITY_ANDROID
                GooglePlayManager.Instance.UnlockAchievement(ConnectGooglePlay.ACHIEVEMENT_NOOB_NAME);
                #elif UNITY_IPHONE
			    GameCenterManager.submitAchievement(100.0f, ConnectGooglePlay.ACHIEVEMENT_NOOB_ID);
                #endif
                break;
            case 20:
                #if UNITY_ANDROID
                GooglePlayManager.Instance.UnlockAchievement(ConnectGooglePlay.ACHIEVEMENT_PRO_NAME);
                #elif UNITY_IPHONE
			    GameCenterManager.submitAchievement(100.0f, ConnectGooglePlay.ACHIEVEMENT_PRO_ID);
                #endif
                break;
            case 30:
                #if UNITY_ANDROID
                GooglePlayManager.Instance.UnlockAchievement(ConnectGooglePlay.ACHIEVEMENT_BOSS_NAME);
                #elif UNITY_IPHONE
			    GameCenterManager.submitAchievement(100.0f, ConnectGooglePlay.ACHIEVEMENT_BOSS_ID);
                #endif
                break;
            case 50:
                #if UNITY_ANDROID
                GooglePlayManager.Instance.UnlockAchievement(ConnectGooglePlay.ACHIEVEMENT_MASTER_NAME);
                #elif UNITY_IPHONE
			    GameCenterManager.submitAchievement(100.0f, ConnectGooglePlay.ACHIEVEMENT_MASTER_ID);
                #endif
                break;
            default: break;
        }
    }*/

    #endregion
}
