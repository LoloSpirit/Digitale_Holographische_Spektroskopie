using System.Linq;
using UnityEngine;

[ExecuteInEditMode] public class HologramController : MonoBehaviour
{
    public Transform referenceBeam;
    public int maxCount = 32;
    public int count = 1;
    public Transform[] points = {};
    
    private Material _material;

    public float frequency;
    public float phase;
    public float amplitude;
    public float speed;
    
    private void OnEnable()
    {
        _material = GetComponent<Renderer>().sharedMaterial;
    }
    
    private void Update()
    {
        // Clamp to array size supported in shader
        var clampedCount = Mathf.Min(count, maxCount);

        var positions = points.Select(p =>
        {
            if (p != null)
                return (Vector4)p.position;
            return Vector4.one * -1;
        }).ToArray();
        _material.SetVectorArray(PositionsID, positions);
        _material.SetInteger(CountID, clampedCount);
        _material.SetFloat(FrequencyID, frequency);
        _material.SetVector(ReferenceSourceID, referenceBeam.position);
        _material.SetVector(ReferenceDirectionID, referenceBeam.forward);
        _material.SetFloat(PhaseID, phase);
        _material.SetFloat(AmplitudeID, amplitude);
        _material.SetFloat(SpeedID, speed);
    }
    
    private static readonly int PositionsID = Shader.PropertyToID("positions");
    private static readonly int ReferenceSourceID = Shader.PropertyToID("referenceSource");
    private static readonly int ReferenceDirectionID = Shader.PropertyToID("referenceDirection");
    private static readonly int CountID = Shader.PropertyToID("count");
    private static readonly int FrequencyID = Shader.PropertyToID("frequency");
    private static readonly int PhaseID = Shader.PropertyToID("phase");
    private static readonly int AmplitudeID = Shader.PropertyToID("amplitude");
    private static readonly int SpeedID = Shader.PropertyToID("speed");
    private static readonly int TimeID = Shader.PropertyToID("time");
}
