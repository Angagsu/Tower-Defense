using UnityEngine;

public class InputHandler : MonoBehaviour, IService
{
    private Vector3 direction;

    float horizontalInput;
    float verticalInput;


    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

    }

    public Vector3 GetDirection()
    {
        return direction = new Vector3(horizontalInput, 0, verticalInput);
    }

}
