using System.Collections;
using UnityEngine;

public class UFOShip : MonoBehaviour
{
    [Header("Main Configuration")]
    public float speed = 7f;
    //public float Bounds = 8.5f;
    public int score = 40;

    public enum UFODirection { Horizontal = 0, Up = 1, Down = 2 };
    [Header("UFO Movement")]
    public UFODirection UFODir = UFODirection.Horizontal;
    public float changeDirTime = 5;
    public float changeDirProb = 50;

    public GameObject explosionPrefab;

    private Transform myTransform;
    private Rigidbody2D myRigidbody2D;

    private SpriteRenderer mySpriteRenderer;
    private BoxCollider2D myCollider2D;

    private float deadZone = 1.0f;
    private bool isInitialized = false;
    private int dir = 1;

    


    IEnumerator Start()
    {
        myTransform = transform;
        myRigidbody2D = GetComponent<Rigidbody2D>();

        dir = (myTransform.position.x < 0) ? 1 : -1;
        myRigidbody2D.velocity = Vector2.right * speed * dir;
        UFODir = UFODirection.Horizontal;
        yield return new WaitForSeconds(1.0f);
        isInitialized = true;

        StartCoroutine("ChangeDirection");
    }

    void Update()
    {
        if (!isInitialized) return;

        if (ScreenBoundsManager.Instance.IsOutScreen(myTransform, deadZone))
        {
            Destroy(this.gameObject);

            if (AsteroidsManager.Instance.IsZeroAsteroids())
            {
                LevelManager.Instance.LevelFinished();
            }
        }
    }


    private IEnumerator ChangeDirection()
    {
        do
        {
            yield return new WaitForSeconds(Random.Range(changeDirTime*0.5f, changeDirTime));
            if (Random.Range(1, 100) <= changeDirProb)
            {
                if (UFODir != UFODirection.Horizontal)
                {
                    myRigidbody2D.velocity = Vector2.right * speed * dir;
                }
                else
                {
                    Vector2 dirVertical = myTransform.position.y > 0 ? Vector2.down : Vector2.up;
                    myRigidbody2D.velocity = (Vector2.right + dirVertical) * speed * dir;
                    yield return new WaitForSeconds(Random.Range(1f, 2f));
                    myRigidbody2D.velocity = Vector2.right * speed * dir;
                }
            }
        }
        while (true);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet")) return;

        Destroy(collision.gameObject);  // Destroy the bullet
        Instantiate(explosionPrefab, myTransform.position, Quaternion.identity);
        LevelManager.Instance.AddScore(score);
        Destroy(this.gameObject);       // Destroy the UFO (this object)

        if (AsteroidsManager.Instance.IsZeroAsteroids())
        {
            LevelManager.Instance.LevelFinished();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Instantiate(explosionPrefab, myTransform.position, Quaternion.identity);
            Destroy(this.gameObject);       // Destroy the UFO (this object)
        }

        if (AsteroidsManager.Instance.IsZeroAsteroids())
        {
            LevelManager.Instance.LevelFinished();
        }
    }

}