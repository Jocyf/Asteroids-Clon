using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;

public class ClassicGameLoader : MonoBehaviour
{
    public bool fade = false;
    public float waitTime = 1f;
    public Text pushText;
    public Text playerText;

    private int levelToLoad;
    private CanvasGroup myCanvas;


    public void LoadClassicGame(int nLevel)
    {
        levelToLoad = nLevel;
        StartCoroutine("_LoadClassicGameTimed");
    }

    private void Start()
    {
        myCanvas = this.transform.parent.GetComponent<CanvasGroup>();
    }

    private IEnumerator _LoadClassicGameTimed ()
    {
        pushText.enabled = false;
        pushText.SendMessage("StopBlinking");
        playerText.enabled = true;
        yield return new WaitForSeconds(waitTime);

        if (fade)
        {
            myCanvas.DOFade(0f, 1f).OnComplete(LoadClassicGameTypeScene); // Fade the Canvas Group
        }
        else
        {
            LoadClassicGameTypeScene();
        }
    }

    private void LoadClassicGameTypeScene()
    {
        GameManager.Instance.LoadClassicGameScene();
    }

}
