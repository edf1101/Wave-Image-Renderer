using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 
 * Code by Ed F
 * www.github.com/edf1101
 */

public class imageCanvas : MonoBehaviour
{
    [Header("Image Settings")]
    [SerializeField] private Texture inputImage;


    [Header("Image Preparation Settings")]
    [SerializeField] private bool isGreyScale;
    [SerializeField] private int downscaleMult;
    [SerializeField] private int blurRadius;

    [Header("Shader References")]
    [SerializeField] private ComputeShader imagePrepShader;
    [SerializeField] private ComputeShader imageBlurShader;

    public RenderTexture debugTex;

    // Component refernces (found in void start)
    private RawImage rawImgRef;
    private RectTransform rawImgRect;

    private imageRenderer myRenderer; // class to help render images

    private void Start() // called before first frame
    {
        // Get the raw img components required on start
        rawImgRef = GetComponent<RawImage>();
        rawImgRect = GetComponent<RectTransform>();

        // Set the compute shaders statically for the shaders
        imagePrepHelper.setShader(imagePrepShader);
        imageBlurHelper.setShader(imageBlurShader);

        // setup myRenderer
        myRenderer = new imageRenderer();

        
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
        // setup variables in myRendererer
        myRenderer.setImage(inputImage);
        myRenderer.setPrepVariables(downscaleMult, blurRadius, isGreyScale);

        myRenderer.prepareImage();


        setImage(myRenderer.getIntensityOutput());
    }
}
