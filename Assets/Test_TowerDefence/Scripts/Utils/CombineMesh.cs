using UnityEngine;


[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]
public class CombineMesh : MonoBehaviour
{

    [SerializeField] private MeshFilter[] meshFilters;


    [ContextMenu("Combine")]
    public void Optimize()
    {
        //MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;

        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        transform.GetComponent<MeshFilter>().sharedMesh = mesh;
        transform.gameObject.SetActive(true);
    }
}
