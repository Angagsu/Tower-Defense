using UnityEngine;

public class MovementController : MonoBehaviour, IService
{
    private InputHandler inputHandler;


    [Inject]
    public void Costruct(InputHandler inputHandler)
    {
        this.inputHandler = inputHandler;
    }

    public void Move(Transform transform, float velocity)
    {
        transform.position += inputHandler.GetDirection() * velocity * Time.deltaTime;
    }
}
