using UnityEngine;

public class CameraControllerLevelsMap : MonoBehaviour
{
    [SerializeField] protected Camera mainCamera;
    [Space(10)]
    [SerializeField] protected float dragSpeed = 0.5f;
    [SerializeField] protected float smoothness;
    [SerializeField] protected float cameraDictance;
    [Space(10)]
    [SerializeField] protected Vector2 minBounds = new Vector2(-10, -10);
    [SerializeField] protected Vector2 maxBounds = new Vector2(10, 10);

    protected Vector2 lastTouchPosition;
    protected Vector2 touchPosition;
    protected Vector2 delta;
    protected bool isDragging;
    protected bool isStartDraging;
    
    protected PlayerInputHandler playerInputHandler;



    [Inject]
    public void Costruct(PlayerInputHandler playerInputHandler)
    {
        this.playerInputHandler = playerInputHandler;
    }

    private void Start()
    {
        playerInputHandler.CameraDragingStarted += OnStartDrag;
        playerInputHandler.CameraDraging += OnDrag;
        playerInputHandler.CameraDragingEnded += OnEndDrag;
    }

    private void OnStartDrag(Vector2 touchPosition)
    {
        lastTouchPosition = touchPosition;
        isStartDraging = true;
    }

    private void OnDrag(Vector2 touchPosition)
    {
        isDragging = true;
        if (!isDragging && isStartDraging) return;

        this.touchPosition = touchPosition;
        delta = touchPosition - lastTouchPosition;

        Vector3 moveDirection = new Vector3(-delta.x * dragSpeed * Time.deltaTime, mainCamera.transform.position.y, -delta.y * dragSpeed * Time.deltaTime);
        Vector3 cameraPosition = mainCamera.transform.position;

        cameraPosition.x = Mathf.SmoothStep(cameraPosition.x, moveDirection.x, smoothness);
        cameraPosition.y = Mathf.SmoothStep(cameraPosition.y, moveDirection.y, smoothness);
        cameraPosition.z = Mathf.SmoothStep(cameraPosition.z, moveDirection.z, smoothness);

        mainCamera.transform.Translate(cameraPosition, Space.World);
        cameraPosition = mainCamera.transform.position;

        cameraPosition.x = Mathf.Clamp(cameraPosition.x, minBounds.x, maxBounds.x);
        cameraPosition.z = Mathf.Clamp(cameraPosition.z, minBounds.y, maxBounds.y);
        cameraPosition.y = cameraDictance;
        mainCamera.transform.position = cameraPosition;

        lastTouchPosition = touchPosition;
    }

    private void OnEndDrag()
    {
        isStartDraging = false;
        isDragging = false;
    }

    private void OnDisable()
    {
        playerInputHandler.CameraDragingStarted -= OnStartDrag;
        playerInputHandler.CameraDraging -= OnDrag;
        playerInputHandler.CameraDragingEnded -= OnEndDrag;
    }
}
