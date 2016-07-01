// Sets the device of the WebCamTexture to the first one available and starts playing it
using UnityEngine;
using System.Collections;

public class WebCamToTexture : MonoBehaviour
{
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        WebCamTexture webcamTexture = new WebCamTexture();

        if (devices.Length > 0)
        {
            Debug.Log("Hello:");
            webcamTexture.deviceName = devices[0].name;
            webcamTexture.Play();
        }
    }
}