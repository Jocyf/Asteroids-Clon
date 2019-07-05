using UnityEngine;

public enum WrapDirection { BothDirections = 0, Horizontal = 1, Vertical = 2 };

public class ScreenCornerDetectorv1 : MonoBehaviour
{
    public float deadZone = 1.0f;
    public WrapDirection wrapDirection = WrapDirection.BothDirections;

    private Transform myTransform;
    //private SpriteRenderer mySpriteRenderer;
    [Space(10)]
    private Vector3 myPosition;
	private bool isLeftScreen = false;
    private Vector3 bottomLeft;
    private Vector3 topRight;

    void Start ()
    {
        myTransform = transform;
        //mySpriteRenderer = GetComponent<SpriteRenderer>();
        bottomLeft = ScreenBoundsManager.Instance.GetButtonLeftScreenCorner(); 
        topRight = ScreenBoundsManager.Instance.GetTopRightScreenCorner(); 
    }

    void Update()
    {
        switch(wrapDirection)
        {
            case WrapDirection.BothDirections:
                ScreenWrapVertical();
                ScreenWrapHorizontal();
                break;
            case WrapDirection.Horizontal:
                ScreenWrapHorizontal();
                break;
            case WrapDirection.Vertical:
                ScreenWrapVertical();
                break;
        }
    }

    private void ScreenWrapHorizontal()
    {
        isLeftScreen = false;
        myPosition = myTransform.position;
        if (myTransform.position.x <= bottomLeft.x - deadZone)
        {
            myPosition.x = topRight.x + deadZone;
            isLeftScreen = true;			
		}
		else if(myTransform.position.x >= topRight.x + deadZone)
        {
            myPosition.x = bottomLeft.x - deadZone;
            isLeftScreen = true;
		}


		if(isLeftScreen)
        {
            myTransform.position = myPosition;
        }				
	}

    private void ScreenWrapVertical()
    {
        isLeftScreen = false;
        myPosition = myTransform.position;
        if (myTransform.position.y <= bottomLeft.y - deadZone)
        {
            myPosition.y = topRight.y + deadZone;
            isLeftScreen = true;
        }
        else if (myTransform.position.y >= topRight.y + deadZone)
        {
            myPosition.y = bottomLeft.y - deadZone;
            isLeftScreen = true;
        }


        if (isLeftScreen)
        {
            myTransform.position = myPosition;
        }
    }
}
