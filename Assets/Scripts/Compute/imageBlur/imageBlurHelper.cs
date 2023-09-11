using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Code by Ed F
 * www.github.com/edf1101
 */

public class imageBlurHelper 
{
    // private reference to shader and setter to set it statically
    private static ComputeShader myShader;
    public static void setShader(ComputeShader _shader)
    {
        myShader = _shader;
    }

    private int kernelSize; // how far around each pixel it looks for its blur

    
    // function blurs the texture
    public RenderTexture blurTexture(Texture _inpTexture, int _radius)
    {
        Vector2 imgSize = new Vector2(_inpTexture.width, _inpTexture.height);

        kernelSize = _radius; // set the radius
        myShader.SetInt("KernelSize", kernelSize); // assign to shader

        // set texture size in shader
        myShader.SetVector("imgSize", imgSize);

        // create a blank texture for output and send it to the shader
        RenderTexture outputRT = new RenderTexture((int)imgSize.x, (int)imgSize.y, 24);
        outputRT.enableRandomWrite = true;
        outputRT.Create();
        myShader.SetTexture(0, "outputTexture", outputRT);

        myShader.SetTexture(0, "inpTexture", _inpTexture);

        myShader.Dispatch(0, Mathf.CeilToInt(imgSize.x / 8f), Mathf.CeilToInt(imgSize.y / 8f), 1);


        // vertical pass

        myShader.SetTexture(1, "inpTexture", outputRT);

        // make outputRT blank again and assign it
        outputRT = new RenderTexture((int)imgSize.x, (int)imgSize.y, 24);
        outputRT.enableRandomWrite = true;
        outputRT.Create();
        myShader.SetTexture(1, "outputTexture", outputRT);

        myShader.Dispatch(1, Mathf.CeilToInt(imgSize.x / 8f), Mathf.CeilToInt(imgSize.y / 8f), 1);



        return outputRT;
    }
}
