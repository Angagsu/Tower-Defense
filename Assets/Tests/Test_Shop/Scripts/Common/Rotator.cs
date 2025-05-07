using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float rotationSpeed;

    private float currentRotation = 0;

    private void Update()
    {
        currentRotation -= Time.deltaTime * rotationSpeed;
        transform.rotation = Quaternion.Euler(0, currentRotation, 0);
    }

    public void ResetRotation() => currentRotation = 0;
}
