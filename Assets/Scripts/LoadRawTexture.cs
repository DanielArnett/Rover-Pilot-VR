using UnityEngine;
using System;

public class LoadRawTexture : MonoBehaviour
{
    Texture2D tex;
    byte[] pixelMap;
    const int numOfCols = 16;
    const int numOfRows = numOfCols / 2;
    const int numOfPixels = numOfCols * numOfRows;


    MjpegProcessor _mjpeg;
    //System.Diagnostics.Stopwatch watch;
    int frameCount = 0;
    public void Start()
    {
        _mjpeg = new MjpegProcessor();
        _mjpeg.FrameReady += mjpeg_FrameReady;
        _mjpeg.Error += mjpeg_Error;
        Uri mjpeg_address = new Uri("http://extcam-16.se.axis.com/mjpg/video.mjpg");
        _mjpeg.ParseStream(mjpeg_address);
        //watch = new System.Diagnostics.Stopwatch();
        //watch.Start();
        // Create a 16x16 texture with PVRTC RGBA4 format
        // and will it with raw PVRTC bytes.
        tex = new Texture2D(numOfRows, numOfCols, TextureFormat.PVRTC_RGBA4, false);
        // Raw PVRTC4 data for a 16x16 texture. This format is four bits
        // per pixel, so data should be 16*16/2=128 bytes in size.
        // Texture that is encoded here is mostly green with some angular
        // blue and red lines.
        pixelMap = new byte[] {
            0x30,0x32,0x32,0x32,0xe7,0x30,0xaa,0x7f,0x32,0x32,0x32,0x32,0xf9,0x40,0xbc,0x7f,
            0x03,0x03,0x03,0x03,0xf6,0x30,0x02,0x05,0x03,0x03,0x03,0x03,0xf4,0x30,0x03,0x06,
            0x32,0x32,0x32,0x32,0xf7,0x40,0xaa,0x7f,0x32,0xf2,0x02,0xa8,0xe7,0x30,0xff,0xff,
            0x03,0x03,0x03,0xff,0xe6,0x40,0x00,0x0f,0x00,0xff,0x00,0xaa,0xe9,0x40,0x9f,0xff,
            0x5b,0x03,0x03,0x03,0xca,0x6a,0x0f,0x30,0x03,0x03,0x03,0xff,0xca,0x68,0x0f,0x30,
            0xaa,0x94,0x90,0x40,0xba,0x5b,0xaf,0x68,0x40,0x00,0x00,0xff,0xca,0x58,0x0f,0x20,
            0x00,0x00,0x00,0xff,0xe6,0x40,0x01,0x2c,0x00,0xff,0x00,0xaa,0xdb,0x41,0xff,0xff,
            0x00,0x00,0x00,0xff,0xe8,0x40,0x01,0x1c,0x00,0xff,0x00,0xaa,0xbb,0x40,0xff,0xff,
        };
        Debug.Log(pixelMap.Length);
        
        /*pixelMap = new byte[numOfPixels];
        for (int i = 0; i < pixelMap.Length; i++)
        {
            pixelMap[i] = 0x00;
        }
        pixelMap[2] = 255;
        pixelMap[3] = 255;*/
        // Load data into the texture and upload it to the GPU.
        tex.LoadRawTextureData(pixelMap);
        tex.Apply();
        // Assign texture to renderer's material.
        GetComponent<Renderer>().material.mainTexture = tex;
        Debug.Log("Tex set.");
    }
    private void mjpeg_FrameReady(object sender, FrameReadyEventArgs e)
    {
        System.Console.WriteLine("Frame Received.");
        e.Bitmap.Save("receivedJPEG" + frameCount + ".bmp");
        //byte[] imageRec = _mjpeg.CurrentFrame;
    }

    void mjpeg_Error(object sender, ErrorEventArgs e)
    {
        Debug.Log("Error received while reading the MJPEG.");
    }
    // Update is called once per frame
    void Update()
    {/*
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
