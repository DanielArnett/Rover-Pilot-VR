using UnityEngine;
using System;

public class LoadRawTexture : MonoBehaviour
{
    Texture2D tex;
    byte[] pixelMap;
    const int numOfCols = 16;
    const int numOfRows = numOfCols / 2;
    const int numOfPixels = numOfCols * numOfRows;

    // Flag showing when to update the frame
    bool updateFrame = false;

    MjpegProcessor _mjpeg;
    //System.Diagnostics.Stopwatch watch;
    int frameCount = 0;
    public void Start()
    {
        _mjpeg = new MjpegProcessor();
        _mjpeg.FrameReady += mjpeg_FrameReady;
        _mjpeg.Error += mjpeg_Error;
        Uri mjpeg_address = new Uri("http://192.168.1.2:8080/?action=stream");
        _mjpeg.ParseStream(mjpeg_address);
        // Create a 16x16 texture with PVRTC RGBA4 format
        // and will it with raw PVRTC bytes.
        tex = new Texture2D(800, 600, TextureFormat.PVRTC_RGBA4, false);
    }
    private void mjpeg_FrameReady(object sender, FrameReadyEventArgs e)
    {
        updateFrame = true;
    }
    void mjpeg_Error(object sender, ErrorEventArgs e)
    {
        Debug.Log("Error received while reading the MJPEG.");
    }

    // Update is called once per frame
    void Update()
    {
        if (updateFrame)
        {
            tex.LoadImage(_mjpeg.CurrentFrame);
            tex.Apply();
            // Assign texture to renderer's material.
            GetComponent<Renderer>().material.mainTexture = tex;
            updateFrame = false;
        }
        
        /*
        // Every Second update the frame.
        if (watch.Elapsed.Seconds > (lastTimeStamp + 3))
        {
            // Save the first row of pvrtcBytes in 'row'
            byte[] row = new byte[numOfCols];
            Array.Copy(pixelMap, row, numOfCols);
            /*for (int i = 0; i < row.Length; i++)
            {
                Debug.Log("Row[" + i + "] = " + row[i]);
                Debug.Log("pixelMap[" + i + "] = " + pixelMap[i]);
            }
            // Copy the rest of pvrtcBytes in 'swap'
            byte[] swap = new byte[pixelMap.Length];
            Array.Copy(pixelMap, numOfCols, swap, 0, pixelMap.Length - numOfCols);
            for (int i = 0; i < row.Length; i++)
            {
                Debug.Log("Row[" + i + "] = " + row[i]);
            }
                // Copy row into the end of swap
                Array.Copy(row, 0, swap, swap.Length - numOfCols, row.Length);
            pixelMap = swap;

            // Load data into the texture and upload it to the GPU.
            tex.LoadRawTextureData(pixelMap);
            tex.Apply();
            // Assign texture to renderer's material.
            GetComponent<Renderer>().material.mainTexture = tex;
            Debug.Log("Tick");
            lastTimeStamp = watch.Elapsed.Seconds;
        }*/
    }
}
