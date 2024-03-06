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

        public bool VariableValue { get; private set; }
        public bool sending=false;
        private float height;
        private OpenAIApi openai = new OpenAIApi();
        private List<ChatMessage> messages = new List<ChatMessage>();
        private string prompt = "You are AI coach, an automated service to teach customer to drive the car. " +
            "You have to teach customer and achieve three targets." +
            "Your response is only about driving class and response with chinese." +
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
        public void Start()
        {
 
            // button.onClick.AddListener(SendReply);
        }
        public void Update()
        {
            if (sending == true)
            {
                SendReply();
                sending = false;
            }
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
            
            //button.enabled = true;
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

            //button.enabled = true;
            inputField.enabled = true;
        }
    }
}
