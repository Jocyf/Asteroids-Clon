using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class BlinkUITextFX : MonoBehaviour
{
    public float ratio = 1f;
    private Text myText;


    public void StopBlinking()
    {
        if(DOTween.IsTweening("blinking")) DOTween.Kill("blinking");
    }

    private void Start()
    {
        myText = GetComponent<Text>();
        StartBlinking();
    }

    private void StartBlinking()
    {
        myText.DOFade(0f, ratio).SetLoops(-1, LoopType.Yoyo).SetId("blinking");
    }
}
