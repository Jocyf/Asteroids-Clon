using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipPart : MonoBehaviour
{
    [Header("Move Apart v1")]
    public float speed = 2;
    public float torque = 2;

    [Header("Move Apart v2")]
    public float force = 1;
    public float radius = 1;

    [InspectorButton("PushPart")]
    public bool push = false;

    [InspectorButton("ResetShip")]
    public bool reset = false;

    private Rigidbody2D myRigidbody2D;
    private Vector3 initialPosition;


    public void DestroyApart()
    {
        PushPartv1();
    }

    private void Awake ()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        initialPosition = transform.localPosition;
    }


    public void ResetShip()
    {
        myRigidbody2D.velocity = Vector2.zero;
        myRigidbody2D.angularVelocity = 0;
        transform.localPosition = initialPosition;
        transform.localRotation = Quaternion.identity;
    }

    public void PushPartv1()
    {
        float newForce = Random.Range(force*0.1f, force);
        myRigidbody2D.AddExplosionForce(newForce, this.transform.parent.position, radius);

        float newTorque = Random.Range(-torque, torque);
        myRigidbody2D.AddTorque(newTorque);
    }

    public void PushPartv2()
    {
        float xSpeed = Random.Range(-speed, speed);
        float ySpeed = Random.Range(-speed, speed);
        myRigidbody2D.velocity = new Vector2(xSpeed, ySpeed) * Time.deltaTime;

        float newTorque = Random.Range(-torque, torque);
        myRigidbody2D.AddTorque(newTorque);
    }
}
