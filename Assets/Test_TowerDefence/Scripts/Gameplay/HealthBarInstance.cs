using UnityEngine;


public class HealthBarInstance : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    private MaterialPropertyBlock materialPropertyBlock;
    private const string sliderPropertyName = "_Slider";


    private void Awake()
    {
        materialPropertyBlock = new();
        meshRenderer.SetPropertyBlock(materialPropertyBlock);
    }

    public void SetMaterialProperty(float amount)
    {
        if (materialPropertyBlock != null)
        {
            materialPropertyBlock.SetFloat(sliderPropertyName, amount);
            meshRenderer.SetPropertyBlock(materialPropertyBlock);
        }
    }
}
