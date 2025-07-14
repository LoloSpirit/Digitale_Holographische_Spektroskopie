using UnityEngine;

[ExecuteInEditMode] public class ReconstructionVisualizer : MonoBehaviour
{
    public HologramController hologramController;
    public LineRenderer mainRenderer;
    public LineRenderer objectRenderer;
    public LineRenderer virtualObjectRenderer;
    public Transform screen;
    public Transform referenceSource;
    public Transform objectSource;
    
    public bool show = true;
    
    public void Toggle() => show = !show;

    private void Update()
    {
        var enabled = hologramController != null && hologramController.count == 1 && show;
        mainRenderer.enabled = enabled;
        objectRenderer.enabled = enabled;
        virtualObjectRenderer.enabled = enabled;
            
        mainRenderer.SetPosition(0, referenceSource.position);
        var pos = new Vector3(referenceSource.position.x, referenceSource.position.y, screen.position.z);
        mainRenderer.SetPosition(1, pos + Vector3.forward * (pos.z-objectSource.position.z));
        
        objectRenderer.SetPosition(0, objectSource.position);
        objectRenderer.SetPosition(1, pos);
        
        var virtualObjectPos = new Vector3(objectSource.position.x, objectSource.position.y, 2 * pos.z-objectSource.position.z);
        virtualObjectRenderer.SetPosition(0, virtualObjectPos);
        virtualObjectRenderer.SetPosition(1, pos);
        virtualObjectRenderer.SetPosition(2, objectSource.position + 2 * (pos-objectSource.position));
    }
}
