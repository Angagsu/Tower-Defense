using UnityEngine;


public class CameraDragController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float dragSpeed = 0.5f;
    [SerializeField] private Vector2 minBounds = new Vector2(-10, -10);
    [SerializeField] private Vector2 maxBounds = new Vector2(10, 10);


    private Vector2 lastTouchPosition;
    private bool isDragging;

    [SerializeField] private PlayerInputHandler playerInputHandler;

    private void Start()
    {

        playerInputHandler.CameraDragingStarted += OnStartDrag;
        playerInputHandler.CameraDraging += OnDrag;
        playerInputHandler.CameraDragingEnded += OnEndDrag;
    }

    private void OnDisable()
    {
        playerInputHandler.CameraDragingStarted -= OnStartDrag;
        playerInputHandler.CameraDraging -= OnDrag;
        playerInputHandler.CameraDragingEnded -= OnEndDrag;
    }

    private void OnStartDrag(Vector2 touchPosition)
    {
        lastTouchPosition = touchPosition;
        isDragging = true;
    }

    private void OnDrag(Vector2 touchPosition)
    {
        if (!isDragging) return;

        // Calculate delta
        Vector2 delta = touchPosition - lastTouchPosition;

        // Move the camera
        Vector3 move = new Vector3(-delta.x * dragSpeed * Time.deltaTime, mainCamera.transform.position.y, -delta.y * dragSpeed * Time.deltaTime);
        mainCamera.transform.Translate(move, Space.World);

        // Clamp movement
        Vector3 clampedPosition = mainCamera.transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBounds.x, maxBounds.x);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minBounds.y, maxBounds.y);
        clampedPosition.y = 40;
        mainCamera.transform.position = clampedPosition;

        // Update the last touch position
        lastTouchPosition = touchPosition;
    }

    private void OnEndDrag()
    {
        isDragging = false;
    }
}
