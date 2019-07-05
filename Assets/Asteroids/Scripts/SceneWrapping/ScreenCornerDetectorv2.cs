using UnityEngine;
using System.Collections;

public class ScreenCornerDetectorv2 : MonoBehaviour
{
    private Camera myCamera;
    private Transform myTransform;
    private SpriteRenderer mySpriteRenderer;
    [Space(10)]
    private Vector3 viewportPosition;
    private Vector3 myPosition;
    private bool isWrappingX = false;
    private bool isWrappingY = false;
    private bool isVisible = true;


    void Start ()
    {
        myTransform = transform;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myCamera = Camera.main;
    }

    void Update()
    {
        ScreenWrap();
    }

    private void ScreenWrap()
    {
        // Check if the object is Visible (Renderer.isVisible doesn't work at all)
        viewportPosition = myCamera.WorldToViewportPoint(transform.position);
        if(IsObjectVisible(viewportPosition))
        {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }

        // Is it is not visible but is wrapped, then do nothing.
        if (isWrappingX && isWrappingY){ return; }

        // The Object is off screen, wrap it.
        myPosition = transform.position;
        if (!isWrappingX || !isWrappingY)
        {
            if (!isWrappingX && (viewportPosition.x < 0 || viewportPosition.x > 1))
            {
                myPosition.x = -myPosition.x;
                isWrappingX = true;
            }
            if (!isWrappingY && (viewportPosition.y < 0 || viewportPosition.y > 1))
            {
                myPosition.y = -myPosition.y;
                isWrappingY = true;
            }
            transform.position = myPosition;
        }
    }

    private bool IsObjectVisible(Vector3 viewportPosition)
    {
        // This works fine. BUT TAKES INTO ACCOUNT THE SCENES CAMERA WINDOWS TOO. SO BE CAREFULL USING IT IN THE EDITOR.
        //return mySpriteRenderer.isVisible; 
        // This method will work always.
        return viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 && viewportPosition.y < 1;
    }

}
