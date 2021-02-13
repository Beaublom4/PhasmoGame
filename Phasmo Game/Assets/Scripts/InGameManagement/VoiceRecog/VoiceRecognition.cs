using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceRecognition : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;

    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    private void Awake()
    {
        SetActions();
    }
    private void Start()
    {
        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }
    void SetActions()
    {
        actions.Add("How old are you", AgeQuestion);
        actions.Add("What is your age", AgeQuestion);

        actions.Add("Run", MakeAngry);
    }
    void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        actions[speech.text].Invoke();
    }

    void AgeQuestion()
    {
        print("(Age)");
    }
    void MakeAngry()
    {
        print("Angry");
    }
}
