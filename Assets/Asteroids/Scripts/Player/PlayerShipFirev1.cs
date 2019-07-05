using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShipFirev1 : MonoBehaviour
{
    [Space(5)]
    public bool canFire = true;
    public float fireRatio = 0.15f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 5;
    //public int maxProjectilesNumber = 4;
    

    private Rigidbody2D myRigidbody2D;
    private Collider2D myCollider2D;
    private SpriteRenderer mySpriteRenderer;
    private PlayerShip playerShipSrc;

    private float fireRatioInternal = 0;

    
    
    public bool IsPlayerAlive() { return playerShipSrc.IsPlayerAlive(); }
    public bool IsOnHyperspace() { return playerShipSrc.IsOnHyperspace(); }
    public void DisableFire() { canFire = false; }
    //public void DecreaseProjectileCount() { currentProjectilesNumber--; }

    private void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myCollider2D = GetComponent<Collider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        playerShipSrc = GetComponent<PlayerShip>();
    }

    private void Update()
    {
        if (!playerShipSrc.IsPlayerAlive()) return;
        if (playerShipSrc.IsOnHyperspace()) return;

        if (canFire)
        {
            Fire();     // Fire pressing the fire with a ratio or pressing quily (you fire faster)
        }
    }

    private void Fire()
    {
        if (fireRatioInternal > 0) fireRatioInternal -= Time.deltaTime;
        if (Input.GetButtonUp("Fire1")) fireRatioInternal = 0;
        if (Input.GetButtonDown("Fire1") || (Input.GetButton("Fire1") && fireRatioInternal <= 0))
        {
            fireRatioInternal = fireRatio;
            ShootBullet();
        }
    }

    private void ShootBullet()
    {
        if (!canFire) return;   // Sentencia de seguridad
        //if (GameObject.FindGameObjectsWithTag("PlayerProjectile").Length < maxProjectilesNumber)
        //if (currentProjectilesNumber < maxProjectilesNumber)
        //{
        GameObject bullet = Instantiate(projectilePrefab, transform.position+transform.up * 0.2f, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = transform.up * projectileSpeed;
        //p.SendMessage("SetPlayerShipSource", this);
        //currentProjectilesNumber++;
        //gameObject.GetComponents<AudioSource>()[0].Play();
        //}
    }

}
