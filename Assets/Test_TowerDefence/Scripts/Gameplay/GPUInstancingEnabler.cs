using UnityEngine;


public class GPUInstancingEnabler : MonoBehaviour
{
    private MaterialPropertyBlock materialPropertyBlock;

    [SerializeField] private MeshRenderer meshRenderer;
    

    private void Awake()
    {
        materialPropertyBlock = new();
        meshRenderer.SetPropertyBlock(materialPropertyBlock);
    }

    public void SetMaterialProperty(float amount)
    {
        materialPropertyBlock.SetFloat("_Slider", amount);

        meshRenderer.SetPropertyBlock(materialPropertyBlock);
    }
}
