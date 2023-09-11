using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Code by Ed F
 * www.github.com/edf1101
 */

public class imagePrepHelper
{

    // refernces and setters for compute shader
    private static ComputeShader myShader;
    public static void setShader(ComputeShader _shader)
    {
        myShader = _shader;
    }


    // settings for prep shader
    private bool isGreyscale;
    private int downscaleMult;

    public imagePrepHelper(bool _isGreyscale, int _downscaleMult)
    {
        isGreyscale = _isGreyscale;
        downscaleMult = _downscaleMult;

    }

    // Output textures
    private RenderTexture colorOutput;
    private RenderTexture intensityOutput;

    public RenderTexture getColourOutput()
    {
        return colorOutput;
    }

    public RenderTexture getIntensityOutput()
    {
        return intensityOutput;
    }

    public void prepImage(Texture _inpImage)
    {
        // set shader variables
        myShader.SetInt("downscaleMult", downscaleMult);
        myShader.SetBool("isGreyscale", isGreyscale);
        
        // create image size
        Vector2 imgSize = new Vector2(_inpImage.width, _inpImage.height);

        // create new textures

        // intensity
        RenderTexture intesityTex = new RenderTexture((int)imgSize.x, (int)imgSize.y, 24);
        intesityTex.enableRandomWrite = true;
        intesityTex.Create();

        RenderTexture colorTex = new RenderTexture((int)imgSize.x, (int)imgSize.y, 24);
        colorTex.enableRandomWrite = true;
        colorTex.Create();


        myShader.SetTexture(0, "inputImg", _inpImage); // assign input texture
        myShader.SetTexture(0, "intensityOutput", intesityTex);
        myShader.SetTexture(0, "colorOutput", colorTex);

        myShader.Dispatch(0, Mathf.CeilToInt((imgSize.x/(float)downscaleMult) / 8f), Mathf.CeilToInt((imgSize.y / (float)downscaleMult) / 8f), 1);

        intensityOutput = intesityTex;
        colorOutput = colorTex;


    }

}
