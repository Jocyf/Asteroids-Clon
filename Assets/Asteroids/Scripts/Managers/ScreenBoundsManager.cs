using UnityEngine;

public class ScreenBoundsManager : MonoBehaviour
{
    private Camera myCamera;

    private Vector3 bottomLeft;
    private Vector3 topRight;

    #region Singleton
    public static ScreenBoundsManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion


    public Vector3 GetButtonLeftScreenCorner() { return bottomLeft; }
    public Vector3 GetTopRightScreenCorner() { return topRight; }

    public bool IsVisible(GameObject obj, float deadZone = 1.0f)        { return !IsBoundReached(obj.transform.position, deadZone); }
    public bool IsVisible(Transform myTransform, float deadZone = 1.0f) { return !IsBoundReached(myTransform.position, deadZone); }
    public bool IsVisible(Vector3 position, float deadZone = 1.0f)      { return !IsBoundReached(position, deadZone); }

    public bool IsOutScreen(GameObject obj, float deadZone = 1.0f)          { return IsBoundReached(obj.transform.position, deadZone); }
    public bool IsOutScreen(Transform myTransform, float deadZone = 1.0f)   { return IsBoundReached(myTransform.position, deadZone); }
    public bool IsOutScreen(Vector3 position, float deadZone = 1.0f)        { return IsBoundReached(position, deadZone); }

    public Vector2 GetPointInsideScreen()
    {
        Vector2 point = Vector3.zero;
        point.x = Random.Range(bottomLeft.x, topRight.x);
        point.y = Random.Range(bottomLeft.y, topRight.y);

        return point;
    }

    public Vector2 MovePointOutsideScreenHorizontal(Vector2 pos, float deadZone = 1.0f)
    {
        Vector2 newPos = pos;
        newPos.x = Random.Range(0, 2) == 0 ? bottomLeft.x - deadZone : topRight.x + deadZone;

        return newPos;
    }

    private void Start()
    {
        myCamera = Camera.main;
        bottomLeft = myCamera.ViewportToWorldPoint(new Vector3(0, 0, myCamera.nearClipPlane));
        topRight = myCamera.ViewportToWorldPoint(new Vector3(1, 1, myCamera.nearClipPlane));
    }

    private bool IsBoundReached(Vector3 position, float deadZone)
    {
        return position.x <= bottomLeft.x - deadZone || position.x >= topRight.x + deadZone;
    }

   
}
