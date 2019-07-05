using System.Collections;
using UnityEngine;

public class UFOManager : MonoBehaviour
{
    [Header("UFO Prefabs")]
    public GameObject omniBigPrefab;
    public GameObject omniLittlePrefab;

    [Header("UFO Times Progress")]
    public float initialOmniTime = 15.0f;
    public float omniRatio = 5.0f;
    [Range(1, 100)] public int omniProbability = 20;
    [Range(1, 100)] public int omniLittleProbability = 0;

    [Header("Skill Progress")]
    public float initialTimeMult = 0.9f;
    public float ratioMult = 0.9f;
    public float probMult = 1.3f;

    [SerializeField]
    [Disable]
    private GameObject UFOObject;
    private Camera myCamera;

    #region Singleton
    public static UFOManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion


    public bool IsUFOOnScreen() { return UFOObject != null; }

    public void ResetUFO()
    {
        Destroy(UFOObject);
        UFOObject = null;
        StopCoroutine("_UFOFireTimed");
        StopCoroutine("_StartPlayingTimed");
    }

    public void StartPlaying()
    {
        ResetUFO();
        StartCoroutine("_StartPlayingTimed");
    }


    IEnumerator _StartPlayingTimed()
    {
        if (LevelManager.Instance.numLevel > 1)
        {
            //float waitTimeAux = initialOmniTime * (int)(initialTimeMult / LevelManager.Instance.numLevel);
            initialOmniTime *= initialTimeMult;

            omniRatio *= ratioMult;
            if(omniRatio < 10) { omniRatio = 10; }

            omniProbability = Mathf.FloorToInt(omniProbability * probMult);
            if (omniProbability > 75) { omniProbability = 75; }

            omniLittleProbability = LevelManager.Instance.numLevel * 10;
            if (omniLittleProbability > 100) { omniLittleProbability = 100; }
        }

        float waitTime = Random.Range(initialOmniTime * 0.5f, initialOmniTime);
        yield return new WaitForSeconds(waitTime);

        StartCoroutine("_UFOFireTimed");
    }

    private void Start()
    {
        myCamera = Camera.main;

        StartPlaying();
    }

    private IEnumerator _UFOFireTimed()
    {
        while (true)
        {
            yield return new WaitForSeconds(omniRatio);
            int n = Random.Range(1, 100);
            if (n < omniProbability)
            {
                CreateUFO();
            }
        }
    }

    private void CreateUFO()
    {
        if (!LevelManager.Instance.playerShipSrc.IsPlayerAlive()) { return; }

        int n = Random.Range(1, 100);
        if (n < omniLittleProbability)
        {
            CreateUFOLittle();
        }
        else
        {
            CreateUFOBig();
            omniLittleProbability += 10;
            if (omniLittleProbability > 75) { omniLittleProbability = 75; }
        }
    }

    private void CreateUFOBig()
    {
        if (UFOObject != null) return;

        Vector2 newPos = ScreenBoundsManager.Instance.GetPointInsideScreen();
        newPos = ScreenBoundsManager.Instance.MovePointOutsideScreenHorizontal(newPos);

        UFOObject = Instantiate(omniBigPrefab, newPos, Quaternion.identity);
        //UFOObject.transform.parent = this.transform;
    }

    private void CreateUFOLittle()
    {
        if (UFOObject != null) return;

        Vector2 newPos = ScreenBoundsManager.Instance.GetPointInsideScreen();
        newPos = ScreenBoundsManager.Instance.MovePointOutsideScreenHorizontal(newPos);

        UFOObject = Instantiate(omniLittlePrefab, newPos, Quaternion.identity);
        //UFOObject.transform.parent = this.transform;
    }
}
