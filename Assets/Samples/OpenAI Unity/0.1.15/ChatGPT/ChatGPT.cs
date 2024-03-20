using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using System.Linq.Expressions;
using System.Collections;
using Amazon.Polly;
using Amazon.Runtime;
using Amazon;
using Amazon.Polly.Model;
using System.IO;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Numerics;


namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {

        [SerializeField] private InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private ScrollRect scroll;
        
        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;
        [SerializeField] private AudioSource audioSource;
        public bool VariableValue { get; private set; }
        public bool sending=false;
        private float height;
        private OpenAIApi openai = new OpenAIApi("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");
        private List<ChatMessage> messages = new List<ChatMessage>();
        private string prompt = "You are AI coach, an automated service to teach customer to drive the car. " +
            "You have to teach customer and achieve three targets." +
            "Your response is only about driving class and response with english." +
            "If customer says somthing which is not about driving class always response about driving class." +
            "Make sure to ask the user relevant follow-up questions." +
            "And follow the target to teach customer."+
            "The first target is start the car"+
            "second target is driving the car strightly"+
            "third target is turn left"+
            "fourth target is turn right"+
            "the last target is drive the destination";
        private string remind;
        public bool revisestart = false;
        public bool ai_signal = false;
        public float revise_motor = 0f;
        public bool send = false;
        public string text;
        public void Start()
        {
            
            
        }
        public void Update()
        {
            CarController1 script1Instance = GameObject.FindObjectOfType<CarController1>();
            if (sending == true)
            {
                
                if (inputField.text == "I don't know how to start the car."||inputField.text=="I don't know how to start the car"|| inputField.text == "I don't know how to start a car." || inputField.text == "I don't know how to start a car")
                {
                    revisestart = true;

                    if (script1Instance != null)
                    {

                        script1Instance.ReceiveVariableValue(revisestart);

                    }
                    else
                    {
                        Debug.LogWarning("Script1 not found in the scene.");
                    }
                }
                if (inputField.text == "I don't know how to move the car."||inputField.text=="I don't know how to move the car"|| inputField.text=="I don't know how to move a car." || inputField.text == "I don't know how to move a car")
                {
                    revise_motor = 50f;
                    ai_signal = true;
                    if (script1Instance != null)
                    {
                        script1Instance.Recive_ai_signal(ai_signal);
                        script1Instance.Receivevertical(revise_motor);

                    }
                    else
                    {
                        Debug.LogWarning("Script1 not found in the scene.");
                    }
                }
                SendReply();
                sending = false;
            }
            
            
        }
        public void ReceiveVariableValue1(bool value)
        {

            VariableValue = value;
            sending = VariableValue;
        }

        public void AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new UnityEngine.Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
        }
        private void WriteIntoFile(Stream stream)
        {
            using (var fileStream = new FileStream($"{Application.persistentDataPath}/audio.mp3", FileMode.Create))
            {
                byte[] buffer = new byte[8 * 1024];
                int bytesRead;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);

                }
            }
        }
        public async void SendReply()
        {
            var credentials = new BasicAWSCredentials("AKIAQ3EGUSX2MWBWPZ5U", "qog2t2ABw3DQ0uz/SSSQP3vfFGnRI/x9Ct7xhClv");
            var client = new AmazonPollyClient(credentials, RegionEndpoint.EUCentral1);

            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = inputField.text
            };
            
            AppendMessage(newMessage);

            if (messages.Count == 0)
            {

                    newMessage.Content = prompt + "\n" + inputField.text;
                
  
            }
            messages.Add(newMessage);
            
            button.enabled = true;
            inputField.text = "";
            inputField.enabled = false;
            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo",
                Messages = messages,
            }) ;

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                
                messages.Add(message);
                text = message.Content.ToString();
                AppendMessage(message);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }
            
            var request = new SynthesizeSpeechRequest()
            {
                Text = text,
                Engine = Engine.Neural,
                VoiceId = VoiceId.Aria,
                OutputFormat = OutputFormat.Mp3
            };

            var response = await client.SynthesizeSpeechAsync(request);
            WriteIntoFile(response.AudioStream);

            using (var www = UnityWebRequestMultimedia.GetAudioClip($"{Application.persistentDataPath}/audio.mp3", AudioType.MPEG))
            {
                var op = www.SendWebRequest();
                while (!op.isDone) await Task.Yield();

                var clip = DownloadHandlerAudioClip.GetContent(www);

                audioSource.clip = clip;
                audioSource.Play();
            }

            button.enabled = true;
            inputField.enabled = true;
        }
    }
}
