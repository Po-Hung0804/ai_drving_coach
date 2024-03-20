using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon.Polly;
using Amazon.Runtime;
using Amazon;
using Amazon.Polly.Model;
using System.IO;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class texttospeech : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public string text = "unity is the best application";
    // Start is called before the first frame update
   private async void Start()
    {
        var credentials = new BasicAWSCredentials("AKIAQ3EGUSX2MWBWPZ5U", "qog2t2ABw3DQ0uz/SSSQP3vfFGnRI/x9Ct7xhClv");
        var client = new AmazonPollyClient(credentials,RegionEndpoint.EUCentral1);

        var request = new SynthesizeSpeechRequest()
        {
            Text=text,
            Engine=Engine.Neural,
            VoiceId=VoiceId.Aria,
            OutputFormat=OutputFormat.Mp3
        };

        var response = await client.SynthesizeSpeechAsync(request);
        WriteIntoFile(response.AudioStream);

        using (var www = UnityWebRequestMultimedia.GetAudioClip( $"{Application.persistentDataPath}/audio.mp3", AudioType.MPEG))
        {
            var op = www.SendWebRequest();
            while (!op.isDone) await Task.Yield();

            var clip = DownloadHandlerAudioClip.GetContent(www);

            audioSource.clip = clip;
            audioSource.Play(); 
        }
        
    }

    private void WriteIntoFile(Stream stream)
    {
        using (var fileStream = new FileStream( $"{Application.persistentDataPath}/audio.mp3", FileMode.Create))
        {
            byte[] buffer = new byte[8 * 1024];
            int bytesRead ;
            while ((bytesRead=stream.Read(buffer,0,buffer.Length))>0)
            {
                fileStream.Write(buffer,  0, bytesRead);

            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
