using UnityEngine;

public class GPUInctansingEnablerSkinnedMesh : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    private MaterialPropertyBlock materialPropertyBlock;


    private void Awake()
    {
        materialPropertyBlock = new();
        materialPropertyBlock.SetFloat("_Global_Light_Intensity", 0.7f);
        skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);
    }
}
