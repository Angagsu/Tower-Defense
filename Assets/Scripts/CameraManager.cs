
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private bool canMoveCamera = true;

    [SerializeField] private float panSpeed = 40f;
    [SerializeField] private float panBorderThickness = 10f;
    private float scrollSpeed = 10f;
    private float minX = 20f, maxX = 60f;
    private float minY = 50f, maxY = 80f;
    private float minZ = -20f, maxZ = 10f;
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            canMoveCamera = !canMoveCamera; 
        }

        if (!canMoveCamera)
        {
            if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panBorderThickness)
            {
                transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
            }

            if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorderThickness)
            {
                transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
            }

            if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorderThickness)
            {
                transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
            }

            if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorderThickness)
            {
                transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
            }
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel") * 1000;
        
        Vector3 pos = transform.position;
        pos.y -= scroll * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        transform.position = pos;
    }
}
