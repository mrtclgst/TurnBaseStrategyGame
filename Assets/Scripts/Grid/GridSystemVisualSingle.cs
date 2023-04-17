using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    public void Show(Material gridMaterial)
    {
        _meshRenderer.enabled = true;
        _meshRenderer.material = gridMaterial;
    }

    public void Hide()
    {
        _meshRenderer.enabled = false;
    }
}