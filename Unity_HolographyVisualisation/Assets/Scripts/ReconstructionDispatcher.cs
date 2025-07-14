using UnityEngine;

public class ReconstructionDispatcher : MonoBehaviour
{
    public ComputeShader fftShader;
    public RenderTexture inputTexture; // Grayscale hologram texture

    private RenderTexture _temp1;
    private RenderTexture _temp2;
    private RenderTexture _outputTexture;

    private int _kernelFFTH;
    private int _kernelFFTV;
    private int _kernelPropagate;
    private int _kernelIFFTH;
    private int _kernelIFFTV;

    public int textureSize = 512;

    void Start()
    {
        InitTextures();
        InitKernels();
        DispatchChain();
    }

    void InitTextures()
    {
        _temp1 = CreateRenderTexture();
        _temp2 = CreateRenderTexture();
        _outputTexture = CreateRenderTexture();
    }

    RenderTexture CreateRenderTexture()
    {
        RenderTexture rt = new RenderTexture(textureSize, textureSize, 0, RenderTextureFormat.ARGBFloat);
        rt.enableRandomWrite = true;
        rt.Create();
        return rt;
    }

    void InitKernels()
    {
        _kernelFFTH = fftShader.FindKernel("FFT_Horizontal");
        _kernelFFTV = fftShader.FindKernel("FFT_Vertical");
        _kernelPropagate = fftShader.FindKernel("ApplyPropagationKernel");
        _kernelIFFTH = fftShader.FindKernel("IFFT_Horizontal");
        _kernelIFFTV = fftShader.FindKernel("IFFT_Vertical");
    }

    void DispatchChain()
    {
        // 1. Horizontal FFT
        fftShader.SetTexture(_kernelFFTH, "Input", inputTexture);
        fftShader.SetTexture(_kernelFFTH, "Output", _temp1);
        fftShader.Dispatch(_kernelFFTH, textureSize / 8, textureSize / 8, 1);

        // 2. Vertical FFT
        fftShader.SetTexture(_kernelFFTV, "Input", _temp1);
        fftShader.SetTexture(_kernelFFTV, "Output", _temp2);
        fftShader.Dispatch(_kernelFFTV, textureSize / 8, textureSize / 8, 1);

        // 3. Apply propagation kernel
        fftShader.SetTexture(_kernelPropagate, "Input", _temp2);
        fftShader.SetTexture(_kernelPropagate, "Output", _temp1);
        fftShader.Dispatch(_kernelPropagate, textureSize / 8, textureSize / 8, 1);

        // 4. Inverse FFT (horizontal)
        fftShader.SetTexture(_kernelIFFTH, "Input", _temp1);
        fftShader.SetTexture(_kernelIFFTH, "Output", _temp2);
        fftShader.Dispatch(_kernelIFFTH, textureSize / 8, textureSize / 8, 1);

        // 5. Inverse FFT (vertical)
        fftShader.SetTexture(_kernelIFFTV, "Input", _temp2);
        fftShader.SetTexture(_kernelIFFTV, "Output", _outputTexture);
        fftShader.Dispatch(_kernelIFFTV, textureSize / 8, textureSize / 8, 1);

        // outputTexture now holds the reconstructed field
        Shader.SetGlobalTexture("_Reconstructed", _outputTexture);
    }
}
