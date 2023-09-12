using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityFileBrowser;

/* 
 * Code by Ed F
 * www.github.com/edf1101
 */

public class imageFileHandler 
{

    // opens a dialog user selects image and the render texture gets returned
    public static RenderTexture importImage()
    {
    
        string path = FileBrowser.OpenFileBrowser(new[] { "png", "jpg","jpeg" })[0];

        Debug.Log(path);

        Texture2D texture;

      
            texture = new Texture2D(1, 1);
        

        byte[] imageData = System.IO.File.ReadAllBytes(path);
        texture.LoadImage(imageData);
        texture.Apply();

        // texRef is your Texture2D
        // You can also reduice your texture 2D that way
        RenderTexture rt = new RenderTexture(texture.width , texture.height , 0);
        RenderTexture.active = rt;
        // Copy your texture ref to the render texture
        Graphics.Blit(texture, rt);

        return rt;

    }

    public static void saveImage(RenderTexture _rt)
    {
        Debug.Log("saving");
        
        string path = FileBrowser.SaveFileBrowser(extensions:new[] { "png", "jpg" });
        Debug.Log(path);
        SaveTexture(_rt, path);
    }

    // Save Texture2D to png
    private static void SaveTexture(RenderTexture rt, string _path)
    {
        byte[] bytes = toTexture2D(rt).EncodeToPNG();
        System.IO.File.WriteAllBytes(_path, bytes);
    }

    // Converts a render texture into A texture 2D
    private static Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }


}
