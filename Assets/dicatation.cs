using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class dicatation : MonoBehaviour
{
    DictationRecognizer dictationRecognizer;
    // Start is called before the first frame update
    void Start()
    {
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationResult += onDictationResult;
        dictationRecognizer.DictationHypothesis += onDictationHypothesis;
        dictationRecognizer.DictationComplete += onDictationComplete;
        dictationRecognizer.DictationError += onDictationError;
    }
    void onDictationResult(string text,ConfidenceLevel confidence)
    {
        Debug.LogFormat("Dictation result: " + text);
    }
    void onDictationHypothesis(string text)
    {
        Debug.LogFormat("Dictation hypothesis:{0}", text);
    }
    void onDictationComplete(DictationCompletionCause cause)
    {
        if (cause != DictationCompletionCause.Complete)
        {
            Debug.LogErrorFormat("Dictation completed unsuccessfuly: {0}.", cause);
        }
    }
    void onDictationError(string error,int hresult)
    {
        Debug.LogErrorFormat("Dictation error:{0}; HRessult={1}.", error, hresult);
    }
   
}
