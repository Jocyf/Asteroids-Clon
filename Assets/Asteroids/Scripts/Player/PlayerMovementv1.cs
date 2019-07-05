using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovementv1 : MonoBehaviour
{
    [Header("Ship Movement")]
    public float thrustForce = 3f;
    public Transform thrust;
    public float rotateSpeed = 500f;
    public float maxVelocity = 15f;
    

    [Header("Ship Hyperspace")]
    public float hyperspace_duration_ = 2;
    public float hyperspace_explode_chance_ = 20;

    [Header("Ship Thruster")]
    [SerializeField]
    [Disable]
    private bool isBlinking = false;
    public float blinkRatio = 0.05f;
    //public AudioClip thrustersClip;
    //public ParticleSystem thrust_particles_prefab_;
    //public AudioClip projectile_sound_;
    //public GameObject hyperspace_anim_prefab_;
    //public ParticleSystem ship_explosion_prefab_;

    private Rigidbody2D myRigidbody2D;
    private Collider2D myCollider2D;
    private SpriteRenderer mySpriteRenderer;
    private SpriteRenderer thrustRenderer;
    private PlayerShip playerShipSrc;
    private AudioSource thrustersAS;
    private float hyperspaceTimer = 0f;


    public bool IsPlayerAlive() { return playerShipSrc.IsPlayerAlive(); }
    public bool IsOnHyperspace() { return playerShipSrc.IsOnHyperspace(); }

    public void DisableThrusterBlink() { DisableThrustBlinkFX(); } // Called from playerShip when ship is destroyed.


    void Start()
    {        
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myCollider2D = GetComponent<Collider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        thrustRenderer = thrust.GetComponent<SpriteRenderer>();
        thrustersAS = GetComponent<AudioSource>();

        thrustRenderer.enabled = false;
        playerShipSrc = GetComponent<PlayerShip>();
    }


    private float horizontal = 0;
    private bool vertical = false;
    private bool hyper = false;
    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical") > 0 && Input.GetButton("Vertical");
        hyper = Input.GetButtonDown("Jump");
    }


    void FixedUpdate()
    {
        if (!IsPlayerAlive()) { thrustersAS.Stop(); return; }
        if (IsOnHyperspace()) { thrustersAS.Stop(); return; }

        // Ship Rotation.
        transform.Rotate(Vector3.forward * -horizontal * rotateSpeed * Time.deltaTime); // Ship turns
        if (Mathf.Abs(horizontal) >= 0.1f)
        {
            myRigidbody2D.angularVelocity = 0;
        }

        // HyperSpace
        if (hyper)
        {
            Hyperspace();
        }

        // Movement. Ship thrust forward and blink effect
        if (vertical && !IsOnHyperspace())
        {
            myRigidbody2D.AddForce(transform.up * thrustForce/* * Time.deltaTime*/);
            if (!thrustersAS.isPlaying) thrustersAS.Play();
            if (!isBlinking)
            {
                isBlinking = true;
                StartCoroutine("_ThrustBlinkFX");
            }
        }
        else
        {
            if (thrustersAS.isPlaying) thrustersAS.Stop();
            DisableThrustBlinkFX();
        }

        // check velocity:  not to go too fast
        if (myRigidbody2D.velocity.magnitude > maxVelocity)
        {
            myRigidbody2D.velocity = myRigidbody2D.velocity.normalized * maxVelocity;
        }
    }


    private void DisableThrustBlinkFX()
    {
        isBlinking = false;
        StopCoroutine("_ThrustBlinkFX");
        thrustRenderer.enabled = false;
    }

    private IEnumerator _ThrustBlinkFX()
    {
        while (true)
        {
            thrustRenderer.enabled = true;
            yield return new WaitForSeconds(blinkRatio);
            thrustRenderer.enabled = false;
            yield return new WaitForSeconds(blinkRatio);
        }
    }

    void Hyperspace()
    {
        //GameObject hyperspace_anim = (GameObject)Instantiate(hyperspace_anim_prefab_, transform.position, transform.rotation);
        //hyperspace_anim.GetComponent<HyperspaceAnim>().Init(0);
        //gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        float x = Random.Range(0.0f, 1.0f);
        float y = Random.Range(0.0f, 1.0f);
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0.0f));
        pos.z = 0.0f;
        transform.position = pos;
        playerShipSrc.SetOnHyperspace(true);
        myCollider2D.enabled = false;
        mySpriteRenderer.enabled = false;
        DisableThrusterBlink();
        StartCoroutine("_EndHyperSpaceTimed");

        //hyperspace_anim = (GameObject)Instantiate(hyperspace_anim_prefab_, transform.position, transform.rotation);
        //hyperspace_anim.GetComponent<HyperspaceAnim>().Init(1);
    }

    private IEnumerator _EndHyperSpaceTimed()
    {
        yield return new WaitForSeconds(hyperspace_duration_);
        playerShipSrc.SetOnHyperspace(false);
        myCollider2D.enabled = true;
        mySpriteRenderer.enabled = true;

        int prob = Random.Range(0, 100);
        if (prob <= hyperspace_explode_chance_)
        {
            playerShipSrc.DestroyShip();
        }
    }

}
