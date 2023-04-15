using UnityEngine;

public class HighlightOnHover : MonoBehaviour
{
    public Material highlightMaterial;

    private Material originalMaterial;
    private Renderer objectRenderer;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterial = objectRenderer.material;
    }

    private void OnMouseEnter()
    {
        objectRenderer.material = highlightMaterial;
    }

    private void OnMouseExit()
    {
        objectRenderer.material = originalMaterial;
    }
}
