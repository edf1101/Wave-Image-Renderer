using UnityEngine;

/* 
 * Code by Ed F
 * www.github.com/edf1101
 */


// This class creates the lines etc on the final texture

public class lineRendererHelper 
{
    private RenderTexture outputImage; // image we return as the final image


    private Vector2 inpImgSize; // size of the input image
    private Vector2 outImgSize; // size of output image

    // variables regarding line generation
    private float angle;
    private float lineIntervals;
    private float circleInterval;
    private int canvasUpscale;
    private float waveAmplitude;
    private Vector2 radiusRange;
    private Vector2 WLRange;
    // setter for above variables
    public lineRendererHelper(float _ang, float _lineIntervals, float _circIntervals, int _canvUpscale, float _wavAmp, Vector2 _radRange, Vector2 _WLRange)
    {
        angle = _ang;
        lineIntervals = _lineIntervals;
        circleInterval = _circIntervals;
        canvasUpscale = _canvUpscale;
        waveAmplitude = _wavAmp;
        radiusRange = _radRange;
        WLRange = _WLRange;
    }
    
    private static ComputeShader myShader; // the compute shader we'll use
    public static void setShader(ComputeShader _shader) // Setter for compute shader
    {
        myShader = _shader;
    }


    // Textures fed in that show the intensity and colour of pixels
    private RenderTexture intensityTex;
    private RenderTexture colorTex;
    // setter for textures
    public void setInputTextures(RenderTexture _intensity, RenderTexture _color)
    {
        intensityTex = _intensity;
        colorTex = _color;
    }

    public RenderTexture renderImage()
    { 

        // Set up image sizes
        inpImgSize = new Vector2(intensityTex.width, intensityTex.height);
        outImgSize = inpImgSize * canvasUpscale;

        // create output image
        outputImage = new RenderTexture((int)outImgSize.x, (int)outImgSize.y, 24);
        outputImage.enableRandomWrite = true;
        outputImage.Create();

        myShader.SetTexture(0, "outputImage", outputImage);

        // kernel 0 is set background
        myShader.Dispatch(0,Mathf.CeilToInt(outImgSize.x/16f), Mathf.CeilToInt(outImgSize.y/ 16f) , 1);

        // kernel 1 is the main line drawing code
        
        myShader.SetTexture(1, "outputImage", outputImage);

        // give it the input textures
        myShader.SetTexture(1, "inputIntensity", intensityTex);
        myShader.SetTexture(1, "inputColor", colorTex);


        // set variables needed
        myShader.SetInt("canvasMult", canvasUpscale);
        myShader.SetVector("outImageSize", outImgSize);
        myShader.SetVector("inImageSize", inpImgSize);
        myShader.SetFloat("tanOfAngle", Mathf.Tan(Mathf.Deg2Rad * angle));
        myShader.SetFloat("circleIntervals", circleInterval);
        myShader.SetVector("radiusRange", radiusRange);
        myShader.SetVector("WLRange", WLRange);
        myShader.SetFloat("waveAmplitude", waveAmplitude);

        if(lineIntervals==0) // error checking if lineIntervals=0 then it would crash
        {
            Debug.Log("Line Intervals cant be 0");
            return outputImage; // would be just background
        }

        if (circleInterval == 0) // error checking if circleInterval=0 then it would crash
        {
            Debug.Log("circle Intervals cant be 0");
            return outputImage; // would be just background
        }

        /* We have interval spacings. isYGap states whether these spacings are on x or 
         * y axis. its required as if its based on the wrong one and angle is at an extreme
         * (far from 45º) then the spacing is very weird. feel free to try.
         */

        bool isYGap = (angle > 45) ? false : true;

        // it goes up until it reaches the top
        float upTo = inpImgSize.y ;

        // use tempIntervals so we dont permanantly modify lineInterval data from user
        float temporaryIntervals = lineIntervals; //default is ygap
        if (!isYGap)
        {
            
            // calculate the inteval along y axis needed to get the correct interval
            // along the x axis instead
            float intervalsAlongXAxis = getYIntercept(0) - getYIntercept(lineIntervals);
            temporaryIntervals = intervalsAlongXAxis; // use this instead
        
        }

        // calculate how many lines are in the frame, aka how many times to run compute shader
        int lineCount = Mathf.CeilToInt((upTo-getMinY())/temporaryIntervals);

        
        // few things to send to shader now theyre calculated
        myShader.SetFloat("minY", getMinY());
        myShader.SetFloat("lineIntervals", temporaryIntervals);

        myShader.Dispatch(1, Mathf.CeilToInt(lineCount/16f), 1 , 1); // dispatch it


        return outputImage;
    }


    // functions relating to line calculations

    // the lowest y coordinate essentially what y intersect when line goes through bottom right corner
    public float getMinY() 
    {
        return getYIntercept(inpImgSize.x);
        //return -Mathf.Tan(Mathf.Deg2Rad * angle) * inpImgSize.x;
    }

    // gets the y intercept when the line goes through any given x intercept on the bottom of canvas
    float getYIntercept(float xIntercept) 
    {
        return -Mathf.Tan(Mathf.Deg2Rad * angle) * xIntercept;
    }

}
