using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuLoader : MonoBehaviour {

    public Image logo;
	public float waitTime = 3;


	IEnumerator Start ()
    {
        yield return new WaitForSeconds(1f);
        logo.DOFade(1f, 1f);
        yield return new WaitForSeconds(waitTime);
        logo.DOFade(0f, 1f).OnComplete(LoadMainMenuScene);
    }

    private void LoadMainMenuScene()
    {
        SceneManager.LoadScene(1);
    }

}
