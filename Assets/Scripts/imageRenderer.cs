using UnityEngine;

/* 
 * Code by Ed F
 * www.github.com/edf1101
 */

public class imageRenderer 
{

    private lineRendererHelper LRH; // class for line rendering

    // variables and settings for line rendering 
    private float angle;
    private float lineIntervals;
    private float circleInterval;
    private int canvasUpscale;
    private float waveAmplitude;
    private Vector2 radiusRange;
    private Vector2 WLRange;

    // setter for above variables
    public void setLineRenderingVariables(float _ang, float _lineIntervals,float _circIntervals,int _canvUpscale, float _wavAmp , Vector2 _radRange, Vector2 _WLRange)
    {
        angle = _ang;
        lineIntervals = _lineIntervals;
        circleInterval = _circIntervals;
        canvasUpscale = _canvUpscale;
        waveAmplitude = _wavAmp;
        radiusRange = _radRange;
        WLRange = _WLRange;
    }

    // Textures for intensity and output
    private RenderTexture intensityOutput;
    private RenderTexture colorOutput;
    private RenderTexture lineOutput;

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

    public void renderImage()
    {
        LRH = new lineRendererHelper(angle,lineIntervals,circleInterval,canvasUpscale,waveAmplitude,radiusRange, WLRange);
        LRH.setInputTextures(intensityOutput, colorOutput);

        lineOutput = LRH.renderImage();
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

    public Texture getLineOutput()
    {
        return lineOutput;
    }
}
