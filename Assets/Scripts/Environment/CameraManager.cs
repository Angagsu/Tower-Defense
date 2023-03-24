using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform towerBuildUI;
    [SerializeField] private float TopEdgeOfScreen;
    [SerializeField] private float DownEdgeOfScreen;
    [SerializeField] private float UIPosZ;

    private Coroutine camMoveUpCor, camMoveDownCor;
    private float minX = 40f, maxX = 40f;
    private float minY = 35, maxY = 60f;
    private float minZ = 0f, maxZ = 20f;

    private void LateUpdate()
    {
        EdgeOfCameraScrolling();
        CheckUIPosition();

        if (GameController.IsGameOver)
        {
            this.enabled = true;
            return;
        }

        UIPosZ = towerBuildUI.position.z;
    }
     
    private void CheckUIPosition()
    {
        if (UIPosZ >= TopEdgeOfScreen)
        {
            if (camMoveUpCor != null)
            {
                StopCoroutine(camMoveUpCor);
            }
            camMoveUpCor = StartCoroutine(CameraMoveUp());
        }

        if (UIPosZ <= DownEdgeOfScreen)
        {
            if (camMoveDownCor != null)
            {
                StopCoroutine(camMoveDownCor);
            }
            camMoveDownCor = StartCoroutine(CameraMoveDown());
        }
    }
    private void EdgeOfCameraScrolling()
    {
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        transform.position = pos;
    }

    private IEnumerator CameraMoveUp()
    {
        while (transform.position.z < TopEdgeOfScreen - 25 && transform.position.z < 19)
        {
            transform.Translate(Vector3.forward.normalized, Space.World);
            yield return null;
            
        }
    }

    private IEnumerator CameraMoveDown()
    {
        while (transform.position.z > DownEdgeOfScreen - 7 && transform.position.z > 1)
        {
            transform.Translate(-Vector3.forward.normalized, Space.World);
            yield return null;
        }
    }
}
