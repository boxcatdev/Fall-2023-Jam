using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DialogueComponent : MonoBehaviour
{
    public bool runAtStart = false;

    [Header("Dialogue Display ")]
    [Tooltip("The gameobject that will be set active and inactive.")]
    [SerializeField] GameObject _dialogueBox;
    [Tooltip("The text field that will display the header text.")]
    [SerializeField] TextMeshProUGUI _headerText;
    [Tooltip("The text field that will display the body text.")]
    [SerializeField] TextMeshProUGUI _bodyText;
    [Space]
    [Tooltip("The text that will be displayed in the dialogue box.")]
    [SerializeField] Dialogue _dialogue;

    private Queue<string> sentences;
    private StarterAssets.StarterAssetsInputs inputs;

    public UnityEvent OnDialogueEnd;

    private void Awake()
    {
        inputs = FindObjectOfType<StarterAssets.StarterAssetsInputs>();
    }
    private void Start()
    {
        sentences = new Queue<string>();
        _dialogueBox.SetActive(false);

        if(runAtStart)
            StartDialogue();
    }
    private void EnableCursor(bool enabled)
    {
        if (enabled)
        {
            Cursor.lockState = CursorLockMode.None;
            inputs.cursorInputForLook = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            inputs.cursorInputForLook = true;
        }
    }
    private void ChangeText()
    {
        if (_headerText.text != _dialogue._speakerName)
            _headerText.text = _dialogue._speakerName;
    }
    public void StartDialogue()
    {
        Debug.Log("Start of conversation...");

        //enable cursor
        EnableCursor(true);

        //unhide dialogue box and update text
        if (_dialogueBox.activeInHierarchy == false)
            _dialogueBox.SetActive(true);

        //update header
        ChangeText();

        //update sentences
        sentences.Clear();

        foreach (var sentence in _dialogue._sentences)
        {
            sentences.Enqueue(sentence);
        }

        //start queueing
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        //display dialogue
        _bodyText.text = sentence;

    }
    public void EndDialogue()
    {
        Debug.Log("End of conversation...");

        //disable cursor
        EnableCursor(false);

        OnDialogueEnd?.Invoke();

        //_dialogueBox.SetActive(false);
    }
}
