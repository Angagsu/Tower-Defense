using UnityEngine;


public class CameraDragController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float dragSpeed = 0.5f;
    [SerializeField] private float smoothness;
    [SerializeField] private Vector2 minBounds = new Vector2(-10, -10);
    [SerializeField] private Vector2 maxBounds = new Vector2(10, 10);
    [SerializeField] private float cameraDictance;

    private Vector2 lastTouchPosition;
    private Vector2 touchPosition;
    private Vector2 delta;
    private bool isDragging;
    private bool isStartDraging;

    private PlayerInputHandler playerInputHandler;



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
