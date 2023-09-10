using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 
 * Code by Ed F
 * www.github.com/edf1101
 */

public class imageRenderer : MonoBehaviour
{
    [Header("Image Settings")]
    [SerializeField] private Texture inputImage;


    [Header("Shader References")]
    [SerializeField] private ComputeShader imagePrepShader;


    // Component refernces (found in void start)
    private RawImage rawImgRef;
    private RectTransform rawImgRect;

    private void Start() // called before first frame
    {
        // Get the raw img components required on start
        rawImgRef = GetComponent<RawImage>();
        rawImgRect = GetComponent<RectTransform>();

        // Set the compute shaders statically for the shaders
        imagePrepHelper.setShader(imagePrepShader);



        
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


    private void convertImage()
    {
        setImage(inputImage);
    }
}
