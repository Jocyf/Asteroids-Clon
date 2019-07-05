using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOFire : MonoBehaviour {

    [Space(5)]
    public bool canFire = true;
    public float fireRatio = 0.15f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 5;
    //public int maxProjectilesNumber = 4;

    private Transform myTransform;
    private Rigidbody2D myRigidbody2D;
    private Collider2D myCollider2D;
    private SpriteRenderer mySpriteRenderer;

    private PlayerShip playerShipSrc;
    //private Transform playerTransform;
    private float fireRatioInternal = 0;



    public void DisableFire() { canFire = false; }


    private void Start()
    {
        myTransform = this.transform;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myCollider2D = GetComponent<Collider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        playerShipSrc = LevelManager.Instance.playerShipSrc;
        //playerTransform = playerShipSrc.transform;
    }

    private void Update()
    {
        //if (!playerShipSrc.IsPlayerAlive()) return;
        //if (playerShipSrc.IsOnHyperspace()) return;

        canFire = ScreenBoundsManager.Instance.IsVisible(myTransform, 0.25f);

        if (canFire)
        {
            Fire();     // Fire pressing the fire with a ratio or pressing quily (you fire faster)
        }
    }

    private void Fire()
    {
        if (fireRatioInternal > 0) fireRatioInternal -= Time.deltaTime;
        if (fireRatioInternal <= 0)
        {
            fireRatioInternal = fireRatio;
            ShootBullet();
        }
    }

    private void ShootBullet()
    {
        if (!canFire) return;   // Sentencia de seguridad
        if (!playerShipSrc.IsPlayerAlive()) return;

        Vector3 target = playerShipSrc.transform.position;
        if (playerShipSrc.IsPlayerMoving())
        {
            target = ScreenBoundsManager.Instance.GetPointInsideScreen();
        }

        Vector2 dir = target - myTransform.position;
        dir.Normalize();

        GameObject bullet = Instantiate(projectilePrefab, transform.position + transform.up * 0.2f, Quaternion.identity); // transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = dir * projectileSpeed;
    }
}
