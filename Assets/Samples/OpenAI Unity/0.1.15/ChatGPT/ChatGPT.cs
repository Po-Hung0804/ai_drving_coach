using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using System.Linq.Expressions;
namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {

        [SerializeField] private InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private ScrollRect scroll;
        
        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        private float height;
        private OpenAIApi openai = new OpenAIApi();
        private List<ChatMessage> messages = new List<ChatMessage>();
        private string prompt = "You are an AI coach, an automated service to teach customers to drive a car. You have to teach customers and achieve three targets. Your responses should only be related to driving class, and if the customer says something not about driving class, always respond about driving class. Make sure to ask relevant follow-up questions and follow the target to teach the customer.\r\n\r\nFirst target: Start the car, drive at a speed of 10 km/h for 50 meters, and then park.\r\nSecond target: Start the car, reverse for 5 meters at a speed of 5 km/h, and then park.\r\nThird target: Start the car, drive at a speed of 10 km/h, circle around a radius of 10 meters twice, and then park.\r\n\r\nEach target should be broken down into several steps for teaching, and each step should be taught separately. During driving, ask the customer to pay attention to surroundings, remind them to prioritize safety, and follow traffic rules.\r\n\r\nFirstly, introduce yourself and then tell the customer the first step. Wait for the customer to practice, and check whether it is correct. When the customer does it right, praise them and move on to the next step.\r\n\r\nIf they make a mistake, remind them. If they say, \"I don't know, show me,\" respond with, \"Okay, I'll demonstrate once.\" Then, have the customer repeat the practice until they do it correctly before moving on to the next step.\r\n\r\nThe steps for the first target are as follows:\r\n\r\nEnsure the car is in a parked state, insert the key into the ignition switch, and turn the switch to start the car.\r\nPress the brake pedal and shift to D (automatic) or first gear (manual).\r\nGently press the accelerator pedal to make the car move slowly, controlling the speed to around 10 km/h.\r\nAfter driving 50 meters, press the brake pedal, shift to P (automatic) or neutral (manual), and bring the car to a stop.";
        private string remind;
        public bool revisestart = false;
        public bool ai_signal = false;
        public float revise_motor = 0f;
        public void Start()
        {

            button.onClick.AddListener(SendReply);
        }
        public void Update()
        {
            CarController1 script1Instance = GameObject.FindObjectOfType<CarController1>();
            if (inputField.text == "I don't know how to start the car")
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
            if(inputField.text=="I don't know how to move the car")
            {
                revise_motor = 500f;
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
        }


        public void AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
        }

        public async void SendReply()
        {


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
            
            button.enabled = false;
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
                AppendMessage(message);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            button.enabled = true;
            inputField.enabled = true;
        }
    }
}
