using UnityEngine;

public class SkinPlacement : MonoBehaviour
{
    private const string RenderLayer = "SkinRender";

    [SerializeField] private Rotator rotator;

    private GameObject currentModel;

    public void InstantiateModel(GameObject model)
    {
        if (currentModel != null)
        {
            Destroy(currentModel.gameObject);
        }

        rotator.ResetRotation();

        currentModel = Instantiate(model, transform);

        Transform[] children = currentModel.GetComponentsInChildren<Transform>();

        foreach (var item in children)
        {
            item.gameObject.layer = LayerMask.NameToLayer(RenderLayer);
        }
    }
}
