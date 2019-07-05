//
// Based on : https://github.com/tutsplus/screen-wrapping-unity/blob/master/src/Assets/Scripts/ScreenWrapBehaviour.cs
//

using UnityEngine;
using System.Collections;

public class AdvancedWrapping : MonoBehaviour
{
    public GameObject ghostPrefab;
    public Transform[] ghosts = new Transform[8]; // GhostShips

    private Camera myCamera;
    private Transform myTransform;
    [Space(10)]
    private Vector3 myPosition;
    private bool isLeftScreen = false;
    private Vector3 bottomLeft;
    private Vector3 topRight;
    private float screenWidth;
    private float screenHeight;

    void Start()
    {
        myTransform = transform;
        myCamera = Camera.main;
        bottomLeft = myCamera.ViewportToWorldPoint(new Vector3(0, 0, myCamera.nearClipPlane));
        topRight = myCamera.ViewportToWorldPoint(new Vector3(1, 1, myCamera.nearClipPlane));
        screenWidth = topRight.x - bottomLeft.x;
        screenHeight = topRight.y - bottomLeft.y;

        CreateGhostShips();
        PositionGhostShips();
    }

    void Update()
    {
        PositionGhostShips();
        SwapShips();
    }

    private void CreateGhostShips()
    {
        GameObject _container = GameObject.Find("GameContainer");
        for (int i = 0; i < ghosts.Length; i++)
        {
            GameObject obj = Instantiate(ghostPrefab, Vector3.zero, Quaternion.identity);
            ghosts[i] = obj.transform;
            //ghosts[i].SendMessage("DisableFire");
            ghosts[i].transform.parent = _container.transform;
            /*DestroyImmediate(ghosts[i].GetComponent<Collider2D>());
            DestroyImmediate(ghosts[i].GetComponent<ScreenCornerDetectorv2>());
            DestroyImmediate(ghosts[i].GetComponent<AdvancedWrapping>());*/
        }
    }

    void PositionGhostShips()
    {
        // All ghost positions will be relative to the ships (this) transform,
        // so let's star with that.
        var ghostPosition = myTransform.position;
        // We're positioning the ghosts clockwise behind the edges of the screen.

        // Let's start with the far right.
        ghostPosition.x = myTransform.position.x + screenWidth;
        ghostPosition.y = myTransform.position.y;
        ghosts[0].position = ghostPosition;
        // Bottom-right
        ghostPosition.x = myTransform.position.x + screenWidth;
        ghostPosition.y = myTransform.position.y - screenHeight;
        ghosts[1].position = ghostPosition;
        // Bottom
        ghostPosition.x = myTransform.position.x;
        ghostPosition.y = myTransform.position.y - screenHeight;
        ghosts[2].position = ghostPosition;
        // Bottom-left
        ghostPosition.x = myTransform.position.x - screenWidth;
        ghostPosition.y = myTransform.position.y - screenHeight;
        ghosts[3].position = ghostPosition;
        // Left
        ghostPosition.x = myTransform.position.x - screenWidth;
        ghostPosition.y = myTransform.position.y;
        ghosts[4].position = ghostPosition;
        // Top-left
        ghostPosition.x = myTransform.position.x - screenWidth;
        ghostPosition.y = myTransform.position.y + screenHeight;
        ghosts[5].position = ghostPosition;
        // Top
        ghostPosition.x = myTransform.position.x;
        ghostPosition.y = myTransform.position.y + screenHeight;
        ghosts[6].position = ghostPosition;
        // Top-right
        ghostPosition.x = myTransform.position.x + screenWidth;
        ghostPosition.y = myTransform.position.y + screenHeight;
        ghosts[7].position = ghostPosition;

        // All ghost ships should have the same rotation as the main ship
        for (int i = 0; i < 8; i++)
        {
            ghosts[i].rotation = myTransform.rotation;
        }
    }

    private void SwapShips()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            if (ghosts[i].position.x < screenWidth*0.5f  && ghosts[i].position.x > -screenWidth*0.5f &&
                ghosts[i].position.y < screenHeight*0.5f && ghosts[i].position.y > -screenHeight*0.5f)
            {
                myTransform.position = ghosts[i].position;
                break;
            }
        }
    }

}
