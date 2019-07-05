using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class PlayerShip : MonoBehaviour
{
    public GameObject explosionPrefab;
    public GameObject ShipDestroyedPrefab;
    public float respawnTime = 4f;
    public float blinkRatio = 0.5f;

    private Rigidbody2D myRigidbody2D;
    private Collider2D myCollider2D;
    private SpriteRenderer mySpriteRenderer;

    private bool isAlive;
    private bool isOnHyperspace;


    public void SetPlayerAlive(bool _isAlive){ isAlive = _isAlive; }
    public bool IsPlayerAlive() { return isAlive; }
    public void SetOnHyperspace(bool _isOnHyperspace) { isOnHyperspace = _isOnHyperspace; }
    public bool IsOnHyperspace() { return isOnHyperspace; }
    public void DestroyShip() { DestroyShipInternal(); }

    public void MakeShipInvulnerable(bool _inv)
    {
        SetInvulnerableShip(true);
        isAlive = !_inv;
    }

    public bool IsPlayerMoving() { return myRigidbody2D.velocity.magnitude > 0.5f; }


    private void Start()
    {        
        isAlive = true;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myCollider2D = GetComponent<Collider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        //StartCoroutine("SetInvulnerableShipTimed");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {        
        if (collision.gameObject.tag.Contains("Asteroid") || collision.gameObject.tag.Contains("Enemy"))
        {
            //Debug.Log("PlayerShip Colliding with: " + collision.gameObject.tag.ToString());
            DestroyShipInternal();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            //Debug.Log("PlayerShip Triggering with: " + collision.gameObject.tag.ToString());
            DestroyShipInternal();
            Destroy(collision.gameObject);  // Destroy bullet
        }
    }

    private void DestroyShipInternal()
    {
        isAlive = false;

        mySpriteRenderer.enabled = false;
        myCollider2D.enabled = false;
        //Debug.Log("PlayerShip has been disabled");
        SendMessage("DisableThrusterBlink");

        CreateShipDestroyed();
        
        LevelManager.Instance.Die();
        if (LevelManager.Instance.numlives > 0)
        {
            StartCoroutine("RestoreShip");
        }
          
    }

    private GameObject shipDestroyed;
    private void CreateShipDestroyed()
    {
        
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        shipDestroyed = Instantiate(ShipDestroyedPrefab, transform.position, transform.rotation);
        shipDestroyed.SendMessage("ExplodeShip");
        shipDestroyed.GetComponent<PlayerShipDestroyer>().SetVelocity(myRigidbody2D.velocity, myRigidbody2D.angularVelocity);
        //bullet.GetComponent<Rigidbody2D>().velocity = transform.up * projectileSpeed;
    }

    private IEnumerator RestoreShip()
    {
        yield return new WaitForSeconds(respawnTime);

        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        myRigidbody2D.velocity = Vector2.zero;
        myRigidbody2D.angularVelocity = 0f;

        Destroy(shipDestroyed);

        while(AsteroidsManager.Instance.CheckIfAsteroidsAround(Vector3.zero) || UFOManager.Instance.IsUFOOnScreen())
        {
            yield return new WaitForSeconds(0.1f);
        }
        
        mySpriteRenderer.enabled = true;
        myCollider2D.enabled = true;
        isAlive = true;
        //Debug.Log("PlayerShip has been restored");

        //StartCoroutine("SetInvulnerableShipTimed");
    }

    #region shipInvulnerable
    private void SetInvulnerableShip(bool _inv)
    {
        myCollider2D.enabled = !_inv;
        //isAlive = !_inv;
        if (_inv)
        {
            StartBlinking();
        }
        else
        {
            StopBlinking();
        }
    }

    private IEnumerator SetInvulnerableShipTimed()
    {
        SetInvulnerableShip(true);
        yield return new WaitForSeconds(4f);
        SetInvulnerableShip(false);
    }

    private void StartBlinking()
    {
        mySpriteRenderer.DOFade(0f, blinkRatio).SetLoops(-1, LoopType.Yoyo).SetId("blinking");
    }

    public void StopBlinking()
    {
        if (DOTween.IsTweening("blinking")) DOTween.Kill("blinking");
    }
    #endregion
}
