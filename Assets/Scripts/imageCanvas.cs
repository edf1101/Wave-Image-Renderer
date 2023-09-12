using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using static UnityEngine.UIElements.UxmlAttributeDescription;

/* 
 * Code by Ed F
 * www.github.com/edf1101
 */

public class imageCanvas : MonoBehaviour
{
    [Header("Image Settings")]
    [SerializeField] private Texture inputImage;
    [SerializeField] private bool saving;
    [SerializeField] private string savePath;


    [Header("Image Preparation Settings")]
    [SerializeField] private bool isGreyScale;
    [SerializeField] private int downscaleMult;
    [SerializeField] private int blurRadius;

    [Header("Line Settings")]
    [Range(0, 89.999f)] [SerializeField] private float angle;
    [SerializeField] private float lineIntervals;

    [Header("Rendering settings")]
    [SerializeField] private float circleInterval;
    [SerializeField] private int canvasUpscale;
    [SerializeField] private float waveAmplitude;
    [SerializeField] private Vector2 radiusRange;
    [SerializeField] private Vector2 wavelengthRange;


    [Header("Shader References")]
    [SerializeField] private ComputeShader imagePrepShader;
    [SerializeField] private ComputeShader imageBlurShader;
    [SerializeField] private ComputeShader lineRendererShader;

    public RenderTexture debugTex;

    // Component refernces (found in void start)
    private RawImage rawImgRef;
    private RectTransform rawImgRect;

    private imageRenderer myRenderer; // class to help render images

    private bool started = false;

    private void Start() // called before first frame
    {
        // Get the raw img components required on start
        rawImgRef = GetComponent<RawImage>();
        rawImgRect = GetComponent<RectTransform>();

        // Set the compute shaders statically for the shaders
        imagePrepHelper.setShader(imagePrepShader);
        imageBlurHelper.setShader(imageBlurShader);
        lineRendererHelper.setShader(lineRendererShader);

        // setup myRenderer
        myRenderer = new imageRenderer();

        started = true;
        
        convertImage(); // convert the image!

    }

    
    private void setImage(Texture _img) {

        // calculate aspect ratio for input image (width/height)
        int width = _img.width;
        int height = _img.height;
        float aspRatio = width / (float)height;

        float rawImgHeight = rawImgRect.sizeDelta.y;

        // modify the width of the raw img so its the aspect ratio* height
        rawImgRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,aspRatio*rawImgHeight);

        // set the image
        rawImgRef.texture = _img;

    }

    // converts the image using imageRenderer class
    private void convertImage()
    {
      
        // setup variables neede in myRenderer
        myRenderer.setImage(inputImage);
        myRenderer.setPrepVariables(downscaleMult, blurRadius, isGreyScale);
        myRenderer.setLineRenderingVariables(angle, lineIntervals, circleInterval, canvasUpscale, waveAmplitude, radiusRange, wavelengthRange);


        // prepare the image ie downscale + blur
        myRenderer.prepareImage();

        // render it 
        myRenderer.renderImage();

        debugTex = (RenderTexture)myRenderer.getLineOutput();

        // set the image
        setImage(debugTex);

        if (saving)
            SaveTexture(debugTex);
        
       
    }
    private void SaveTexture(RenderTexture rt)
    {
        byte[] bytes = toTexture2D(rt).EncodeToPNG();
        System.IO.File.WriteAllBytes(savePath, bytes);
    }

    private Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    private void OnValidate() // update the image each time a setting is changed
    {
        if(Application.isPlaying && started)
        {
            convertImage();
        }
    }
}
