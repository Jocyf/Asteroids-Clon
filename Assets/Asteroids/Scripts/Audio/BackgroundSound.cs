using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
    public float ratio = 1f;
    public float ratioMult = 0.1f;
    public float changeRatioTime = 10f;

    private AudioSource myAS;
    private float internalRatio = 0;

    void Start ()
    {
        myAS = GetComponent<AudioSource>();
        //InvokeRepeating("ChangeRatio", 1f, 0.1f);
    }

    /*private void Update()
    {
        ChangeRatio();
    }*/


    private void Update()
    {
        internalRatio -= Time.deltaTime;
        if (internalRatio <= 0)
        {
            ratio += ratioMult;
            ratio = Mathf.Clamp(ratio, 0.9f, 1.75f);
            internalRatio = changeRatioTime;
            myAS.pitch = ratio;
        }
    }

}
