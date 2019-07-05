using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AsteroidsManager : MonoBehaviour
{
    [Header("Skill Progress")]
    public float asteroidMult = 2f;
    public float veloMult = 0.3f;

    [System.Serializable]
    public class AsteroidUnit
    {
        public GameObject[] asteroidPrefabs;
        public int numberAsteroids = 1;
        public float speed = 30f;
        public float torque = 30f;
    }
    [Header("Asteroids Units")]
    public AsteroidUnit[] asteroidsUnits;

    [Space(10)]
    private Transform myTransform;
    private Vector3 myPosition;

    private List<GameObject> asteroidList = new List<GameObject>();
    [SerializeField]
    [Disable]
    private int numAsteroidsCreated = 0;

    [InspectorButton("GenerateAsteroids")]
    public bool create = false;

    [InspectorButton("DeleteAsteroids")]
    public bool destroy = false;

    #region Singleton
    public static AsteroidsManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion


    public bool IsZeroAsteroids() { return numAsteroidsCreated == 0;  }
    public void CreateBigAsteroid(Vector3 _position) { CreateAsteroid(asteroidsUnits[0], _position); }
    public void CreateMediumAsteroid(Vector3 _position) { CreateAsteroid(asteroidsUnits[1], _position);  }
    public void CreateLittleAsteroid(Vector3 _position) { CreateAsteroid(asteroidsUnits[2], _position); }

    public void StartPlaying()
    {
        GenerateAsteroids();
    }

    public bool CheckIfAsteroidsAround(Vector3 position)
    {
        bool isAround = false;

        for (int i = asteroidList.Count - 1; i >= 0; i--)
        {
            if (Vector3.Distance(asteroidList[i].transform.position, position) < 2f)
            {
                isAround = true;
                break;
            }
        }

        return isAround;
    }



    private IEnumerator Start()
    {
        myTransform = transform;

        yield return new WaitForSeconds(0.1f);
        StartPlaying();
    }

    private void GenerateAsteroids()
    {
        numAsteroidsCreated = 0;

        for (int i = 0; i < asteroidsUnits.Length; i++)
        {
            int numberAsteroids = asteroidsUnits[i].numberAsteroids;
            if (numberAsteroids > 0)
            {
                numberAsteroids += (int)(LevelManager.Instance.numLevel * asteroidMult);
                if (numberAsteroids < asteroidsUnits[i].numberAsteroids) { numberAsteroids = asteroidsUnits[i].numberAsteroids; }
            }

            for (int j = 0; j < numberAsteroids; j++)
            {
                Vector2 newPosition = Vector2.zero;
                float distToPlayer = 0;
                while (distToPlayer < 2)
                {
                    newPosition = ScreenBoundsManager.Instance.GetPointInsideScreen();
                    distToPlayer = Vector2.Distance(newPosition, LevelManager.Instance.playerShipSrc.transform.position);
                }

                CreateAsteroid (asteroidsUnits[i], newPosition);
            }
        }
    }

    private void CreateAsteroid(AsteroidUnit asteroidUnit, Vector2 _pos)
    {
        int n = Random.Range(0, asteroidUnit.asteroidPrefabs.Length);
        GameObject obj = Instantiate(asteroidUnit.asteroidPrefabs[n], _pos, Quaternion.identity);
        obj.transform.parent = this.transform;
        PushAsteroid(obj, asteroidUnit);
        numAsteroidsCreated++;
        asteroidList.Add(obj);
    }

    // Make the asteroid move at a random speed (used when an asteroid is created).
    private void PushAsteroid(GameObject _asteroid, AsteroidUnit asteroidUnit)
    {
        Rigidbody2D myRigidbody2D = _asteroid.GetComponent<Rigidbody2D>();
        float xSpeed = Random.Range(asteroidUnit.speed-5, asteroidUnit.speed+5);
        xSpeed *= Random.Range(0, 2) == 0 ? -1 : 1;
        float ySpeed = Random.Range(asteroidUnit.speed - 5, asteroidUnit.speed + 5);
        ySpeed *= Random.Range(0, 2) == 0 ? -1 : 1;
        myRigidbody2D.velocity = new Vector2(xSpeed, ySpeed);
        //Debug.Log("PushAsteroid-> "+_asteroid.name+": Velo: " + myRigidbody2D.velocity+" Multiplying by: "+(LevelManager.Instance.numLevel * veloMult).ToString());
        myRigidbody2D.velocity *= veloMult;     // LevelManager.Instance.numLevel * veloMult; // skill velocity multiplier.
        //Debug.Log("PushAsteroid-> "+_asteroid.name + " Total Velo: " + myRigidbody2D.velocity);

        float newTorque = Random.Range(asteroidUnit.torque*0.7f, asteroidUnit.torque);
        newTorque *= Random.Range(0, 2) == 0 ? -1 : 1;
        myRigidbody2D.AddTorque(newTorque*100);
    }

    public void DestroyAsteroid(GameObject _asteroid)
    {
        if (asteroidList == null) return;
        if (asteroidList.Count == 0) return;
        if (_asteroid == null) return;

        asteroidList.Remove(_asteroid);
        Destroy(_asteroid);
        numAsteroidsCreated--;
    }

    public void DestroyAllAsteroids()
    {
        if (asteroidList == null) return;
        if (asteroidList.Count == 0) return;

        for (int i = asteroidList.Count - 1; i >= 0; i--)
        {
            if (asteroidList[i] != null)
            {
                Destroy(asteroidList[i].gameObject);
            }
        }

        asteroidList.Clear();
        numAsteroidsCreated = 0;
    }

}
