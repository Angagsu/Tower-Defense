using UnityEngine;


public class CanvasCameraAdjuster : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float cameraMoveSpeed = 50f;

    private bool isMovable = false;
    private bool isVisibale = false;

    private void OnEnable()
    {
        isMovable = true;
        isVisibale = true;
    }

    private void OnDisable()
    {
        isMovable = false;
        isVisibale = false;
    }

    void Update()
    {
        if (!isMovable && !isVisibale)
        {
            return;
        }

        Vector3[] worldCorners = new Vector3[4];
        rectTransform.GetWorldCorners(worldCorners);

        Vector3 moveDirection = Vector3.zero;

        foreach (Vector3 corner in worldCorners)
        {
            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(corner);


            if (viewportPoint.x < 0)
            {
                moveDirection = new Vector3(-1, 0, 0);
            }
            else if (viewportPoint.x > 1)
            {
                moveDirection = new Vector3(1, 0, 0);
            }
            if (viewportPoint.y < 0)
            {
                moveDirection = new Vector3(0, 0, -1);
            }
            else if (viewportPoint.y > 1)
            {
                moveDirection = new Vector3(0, 0, 1);
            }
        }

        if (moveDirection != Vector3.zero)
        {
            mainCamera.transform.position += moveDirection.normalized * cameraMoveSpeed * Time.deltaTime;
        }
        else
        {
            isMovable = false;
        }
    }
}

