using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class LoudnessDetector : MonoBehaviour
{
    private int sampleWindow = 128;
    private AudioClip microphoneClip;
    void Start()
    {
        MicrophoneToAudioClip();
    }

    void Update()
    {

    }
    public void MicrophoneToAudioClip()
    {
        //Get first microphone in device list
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }

    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]),microphoneClip);
    }
    
    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
{
    int startPosition = clipPosition - sampleWindow;

    if (startPosition < 0)
    {
        return 0;
    }
    float[] waveData = new float[sampleWindow];
    clip.GetData(waveData, startPosition);

    //compute loudness
    float totalLoudness = 0;

    for (int i = 0; i < sampleWindow; i++)
    {
        totalLoudness += Mathf.Abs(waveData[i]);
    }
    return totalLoudness / sampleWindow;
}
}
