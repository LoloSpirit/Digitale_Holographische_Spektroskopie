using UnityEngine;

public class InterferenceDispatcher : MonoBehaviour
{
    public ComputeShader computeShader;
    public int textureSize = 512;
    public float pixelSize = 0.01f;
    public Vector2 offset = new Vector2(-2.56f, -2.56f); // center origin

    public Vector3[] sourcePositions;
    public Vector3 referenceSource = new Vector3(0, 0, -10);
    public Vector3 referenceDirection = Vector3.forward;

    public float frequency = 1.0f;
    public float phase = 0.0f;
    public float amplitude = 1.0f;
    public float speed = 1.0f;

    public RenderTexture result;
    ComputeBuffer sourceBuffer;

    void Start()
    {
        result = new RenderTexture(textureSize, textureSize, 0, RenderTextureFormat.ARGBFloat)
            {
                enableRandomWrite = true
            };
        result.Create();

        sourceBuffer = new ComputeBuffer(sourcePositions.Length, sizeof(float) * 3);
        sourceBuffer.SetData(sourcePositions);
    }

    void Update()
    {
        int kernel = computeShader.FindKernel("CSMain");

        computeShader.SetTexture(kernel, "Result", result);
        computeShader.SetBuffer(kernel, "positions", sourceBuffer);

        computeShader.SetInt("count", sourcePositions.Length);
        computeShader.SetFloat("frequency", frequency);
        computeShader.SetFloat("phase", phase);
        computeShader.SetFloat("amplitude", amplitude);
        computeShader.SetFloat("speed", speed);
        computeShader.SetFloat("time", Time.time);
        computeShader.SetFloat("pixelSize", pixelSize);
        computeShader.SetVector("offset", offset);
        computeShader.SetVector("referenceSource", referenceSource);
        computeShader.SetVector("referenceDirection", referenceDirection);

        computeShader.Dispatch(kernel, textureSize / 8, textureSize / 8, 1);

        // Optional: use the texture in a material
        Shader.SetGlobalTexture("_InterferenceTex", result);
    }

    void OnDestroy()
    {
        result.Release();
        sourceBuffer.Release();
    }
}