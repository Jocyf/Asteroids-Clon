using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipDestroyer : MonoBehaviour
{
    [SerializeField]
    [Disable]
    private PlayerShipPart[] playerParts;

    [InspectorButton("GetPlayerParts")]
    public bool getParts = false;

    [InspectorButton("DestroyPlayerShip")]
    public bool destroy = false;

    [InspectorButton("RestorePlayerShip")]
    public bool restore = false;


    public void ExplodeShip() { DestroyPlayerShip();  }

    public void SetVelocity(Vector2 _velo, float angularvelo)
    {
        GetComponent<Rigidbody2D>().velocity = _velo;
        GetComponent<Rigidbody2D>().angularVelocity = angularvelo;
    }


    private void GetPlayerParts()
    {
        playerParts = GetComponentsInChildren<PlayerShipPart>();
    }

    private void Awake ()
    {
        if (playerParts == null || playerParts.Length == 0)
        {
            GetPlayerParts();
        }
    }

	
	private void DestroyPlayerShip ()
    {
        for(int i = 0; i < playerParts.Length; i++)
        {
            playerParts[i].DestroyApart();
        }
    }

    private void RestorePlayerShip()
    {
        for (int i = 0; i < playerParts.Length; i++)
        {
            playerParts[i].ResetShip();
        }
    }

}
