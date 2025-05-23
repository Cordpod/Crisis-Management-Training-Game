using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingDialogueUI : MonoBehaviour
{
    public static TrainingDialogueUI instance;
    public TMP_Text dialogueText;
    public List<TMP_Text> factorLetters;

    private DialogueEntry currentDialogue;
    private int trainingDialogueIndex = -1;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool skipTyping = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject); // This persists during training, but training objects will be destroyed on transition.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Remove or comment out the following line to keep the canvas enabled at startup.
        // gameObject.SetActive(false);
    }

    public void DisplayDialogue(DialogueEntry dialogue)
    {
        DisplayTrainingDialogue(dialogue);
    }

    public void DisplayTrainingDialogue(DialogueEntry dialogue)
    {
        // Ensure the canvas is enabled when dialogue is displayed.
        gameObject.SetActive(true);
        currentDialogue = dialogue;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(dialogue.lines[0].text));

        int factorIndex = GetFactorIndex(dialogue.id);
        if (factorIndex != -1)
        {
            trainingDialogueIndex = factorIndex;
            UpdateLetterHighlight();
        }
    }

    IEnumerator TypeText(string fullText)
    {
        dialogueText.text = "";
        isTyping = true;
        skipTyping = false;
        foreach (char c in fullText)
        {
            if (skipTyping)
            {
                dialogueText.text = fullText;
                break;
            }
            dialogueText.text += c;
            yield return new WaitForSeconds(0.03f);
        }
        isTyping = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) ||
            (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            if (isTyping)
            {
                skipTyping = true;
                return;
            }
            if (currentDialogue != null)
            {
                // If the nextId is empty, we�re at the final dialogue.
                if (string.IsNullOrEmpty(currentDialogue.lines[0].nextId))
                {
                    CloseTrainingDialogue();
                }
                else
                {
                    ContinueTrainingDialogue(currentDialogue.lines[0].nextId);
                }
            }
        }
    }

    private void UpdateLetterHighlight()
    {
        foreach (var letter in factorLetters)
        {
            letter.color = Color.black;
        }
        if (trainingDialogueIndex >= 0 && trainingDialogueIndex < factorLetters.Count)
        {
            factorLetters[trainingDialogueIndex].color = Color.red;
        }
    }

    private int GetFactorIndex(string dialogueId)
    {
        switch (dialogueId)
        {
            case "TrainingScene9":
                return 0;
            case "TrainingScene11":
                return 1;
            case "TrainingScene13":
                return 2;
            case "TrainingScene15":
                return 3;
            case "TrainingScene17":
                return 4;
            default:
                return -1;
        }
    }

    public void ContinueTrainingDialogue(string nextId)
    {
        StartCoroutine(ContinueTrainingDialogueCoroutine(nextId));
    }


    private IEnumerator ContinueTrainingDialogueCoroutine(string nextId)
    {
        // Retrieve the next dialogue entry from TrainingDialogueManager.
        DialogueEntry nextDialogue = TrainingDialogueManager.instance.GetDialogueById(nextId);
        if (nextDialogue != null)
        {
            DialogueLine line = nextDialogue.lines[0];

            if (!string.IsNullOrEmpty(line.trigger))
            {
                yield return StartCoroutine(ExecuteTrainingTriggerSequence(line.trigger, line.sound, line.nextId));
         

            }


            DisplayTrainingDialogue(nextDialogue);
            
        }
        else
        {
            CloseTrainingDialogue();
        }
    }

    IEnumerator ExecuteTrainingTriggerSequence(string triggerId, string soundName, string resumeId)
    {
        //yield return ScreenFader.instance.FadeOut();

        //if (!string.IsNullOrEmpty(soundName))
        //    yield return SoundManager.instance.PlaySoundAndWait(soundName);

        Debug.Log("Sound completed, going to if else for trigger");
        // Trigger cutscene
        if (triggerId.StartsWith( "scene_change"))
        {
            yield return ScreenFader.instance.FadeOut();
            Debug.Log("detected trigger id correctly");
            // getting the scene to change to 
            string[] parts = triggerId.Split('_');
            string bgName = parts[parts.Length - 1]; // e.g., "MRTOutside"
            BackgroundController.instance.ChangeTo(bgName);
            yield return new WaitForSeconds(0.05f);
            yield return ScreenFader.instance.FadeIn();

        } else if (triggerId == "move_sprite") {
            yield return CutsceneController.instance.MoveSprite();
        }

        //yield return new WaitForSeconds(0.1f);
        //yield return ScreenFader.instance.FadeIn();

        //ContinueTrainingDialogue(resumeId);
    }


    public void CloseTrainingDialogue()
    {
        StartCoroutine(TransitionToMainScene());
    }

    IEnumerator TransitionToMainScene()
    {
        // Wait briefly so the final dialogue can be read.
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainScene");
    }
}