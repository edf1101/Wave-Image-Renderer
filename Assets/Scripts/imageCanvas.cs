using UnityEngine;
using UnityEngine.UI;
using TMPro;
/* 
 * Code by Ed F
 * www.github.com/edf1101
 */

public class imageCanvas : MonoBehaviour
{
    [Header("Image Settings")]
    [SerializeField] private Texture inputImage;
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



    [Header("UI Settings refernces")]
    [SerializeField] private Toggle greyscaleToggle;
    [SerializeField] private Slider downscalingSlider;
    [SerializeField] private Slider blurSlider;
    [SerializeField] private Slider lineAngleSlider;
    [SerializeField] private Slider lineSpacingSlider;
    [SerializeField] private Slider circleSpacingSlider;
    [SerializeField] private Slider canvasUpscaleSlider;
    [SerializeField] private Slider waveAmplitudeSlider;
    [SerializeField] private TMP_InputField radiusLow;
    [SerializeField] private TMP_InputField radiusHigh;
    [SerializeField] private TMP_InputField wlLow;
    [SerializeField] private TMP_InputField wlHigh;

    private RenderTexture finalImg;

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
        
        //convertImage(); // convert the image!

    }


    // sets the image onto the display (mainly handles aspect ratios etc)
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
        //update parameters according to UI settings
        updateSettings();

        // setup variables neede in myRenderer
        myRenderer.setImage(inputImage);
        myRenderer.setPrepVariables(downscaleMult, blurRadius, isGreyScale);
        myRenderer.setLineRenderingVariables(angle, lineIntervals, circleInterval, canvasUpscale, waveAmplitude, radiusRange, wavelengthRange);


        // prepare the image ie downscale + blur
        myRenderer.prepareImage();

        // render it 
        myRenderer.renderImage();

        finalImg = (RenderTexture)myRenderer.getLineOutput();

        // set the image
        setImage(finalImg);
 
    }

 
   

    private void updateSettings()
    {
        isGreyScale = greyscaleToggle.isOn;
        downscaleMult = (int)downscalingSlider.value;
        blurRadius = (int)blurSlider.value;
        angle = lineAngleSlider.value;
        lineIntervals = lineSpacingSlider.value;
        circleInterval = circleSpacingSlider.value;
        canvasUpscale = (int)canvasUpscaleSlider.value;
        waveAmplitude = waveAmplitudeSlider.value;
        radiusRange = new Vector2(float.Parse(radiusLow.text), float.Parse(radiusHigh.text));
        wavelengthRange = new Vector2(float.Parse(wlLow.text), float.Parse(wlHigh.text));
    }

    public void makeImage() // basically calls convert image but this is public and does a check
    {
        if (inputImage != null)
        {
            convertImage();
        }

    }

    // opens image
    public void openImage()
    {
        inputImage = (Texture)imageFileHandler.importImage();
        makeImage();
    }

    // saves image
    public void saveImage()
    {
        imageFileHandler.saveImage(finalImg);
    }

    // closes app
    public void closeApp()
    {
        Application.Quit();
    }

}
