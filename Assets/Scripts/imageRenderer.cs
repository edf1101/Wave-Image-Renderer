using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Code by Ed F
 * www.github.com/edf1101
 */

public class imageRenderer 
{

    // Textures for intensity and output
    private RenderTexture intensityOutput;
    private RenderTexture colorOutput;

    // set the input image
    private Texture inputImage;
    public void setImage(Texture _img)
    {
        inputImage = _img;
    }


    // variables for image preparation 
    private int downscaleMult;
    private int blurRadius;
    private bool isGreyscale;
    // and their setter
    public void setPrepVariables(int _downscale, int _blurRad,bool _isGreyscale)
    {
        downscaleMult = _downscale;
        blurRadius = _blurRad;
        isGreyscale = _isGreyscale;
    }

    // preps the image for converting
    public void prepareImage()
    {
        // create preparation helper
        imagePrepHelper IPH = new imagePrepHelper(isGreyscale, downscaleMult);
        IPH.prepImage(inputImage);

        intensityOutput = IPH.getIntensityOutput();
        colorOutput = IPH.getColourOutput();

        // create blur helper
        imageBlurHelper IBH = new imageBlurHelper();
        // blur intensity 
        intensityOutput = IBH.blurTexture(intensityOutput, blurRadius);
        if (!isGreyscale) // if its not greyscale then blur colors
            colorOutput = IBH.blurTexture(colorOutput, blurRadius);

    }

    // getters for the textures 
    public Texture getIntensityOutput()
    {
        return intensityOutput;
    }

    public Texture getColorOutput()
    {
        return colorOutput;
    }
}
