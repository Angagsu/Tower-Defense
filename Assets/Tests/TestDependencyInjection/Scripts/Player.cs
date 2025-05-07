using UnityEngine;

public class Player : MonoBehaviour
{
    private MovementController movementController;
    private float velocity = 2;

    [Inject]
    public void Construct(MovementController movementController)
    {
        this.movementController = movementController;
    }

    private void Update()
    {
        movementController.Move(transform, velocity);
    }
}
