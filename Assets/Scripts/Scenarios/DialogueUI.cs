using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    // Singleton instance
    public static DialogueUI instance;

    public TMP_Text dialogueText;
    public GameObject optionsContainer;
    public Button optionButtonPrefab;

    private DialogueEntry currentDialogue; // For current ref dialogue
    public static bool isDialogueActive = false; // Global flag

    // Holds references to your "C, H, I, M, E" TextMeshPro objects in order
    public List<TMP_Text> factorLetters;

    // Start at -1 so that no letter is highlighted initially
    private int trainingDialogueIndex = -1;

    // Flag to indicate training mode
    public bool isTrainingMode = false;

    public UI_StatsRadarChart radarChart;
    private Stats stats;

    private void Start()
    {
        // Hide dialogue box on start
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        // Singleton pattern to ensure only one DialogueUI exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    public void DisplayDialogue(DialogueEntry dialogue)
    {
        gameObject.SetActive(true); // Show dialogue box
        isDialogueActive = true;
        currentDialogue = dialogue; // Store the current dialogue

        if (dialogueText == null)
        {
            Debug.LogError("dialogueText is NULL! Check if it's assigned.");
            return;
        }

        //// Display the first line of this dialogue entry
        //dialogueText.text = dialogue.lines[0].text;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(dialogue.lines[0].text));

        Debug.Log($"Displaying current dialogue: {dialogueText.text}");

        // In training mode, disable option buttons and update the CHIME highlight based on dialogue id
        if (isTrainingMode)
        {
            optionsContainer.SetActive(false);
            int factorIndex = GetFactorIndex(dialogue.id);
            if (factorIndex != -1)
            {
                trainingDialogueIndex = factorIndex;
                UpdateLetterHighlight();
            }
        }
        else
        {
            // Only create option buttons if they exist
            if (dialogue.lines[0].options.Count > 0)
            {
                // Clear previous buttons
                foreach (Transform child in optionsContainer.transform)
                {
                    Destroy(child.gameObject);
                    Debug.Log($"destroyed: {child.name}");
                }

                optionsContainer.SetActive(true);
                foreach (var option in dialogue.lines[0].options)
                {
                    Button btn = Instantiate(optionButtonPrefab, optionsContainer.transform);
                    TMP_Text btnText = btn.GetComponentInChildren<TMP_Text>();

                    // image_v1.0
                    Image btnImage = btn.GetComponent<Image>();
                    if (!string.IsNullOrEmpty(option.image))
                    {
                        // Remove extension (if any) before loading
                        string imagePath = System.IO.Path.ChangeExtension(option.image, null);
                        //Debug.Log($"Attempting to load sprite from: {imagePath}");

                        Sprite optionSprite = Resources.Load<Sprite>(imagePath);
                        if (optionSprite != null)
                        {
                            btnImage.sprite = optionSprite;
                            if (btnText != null) btnText.gameObject.SetActive(false);
                            //Debug.Log($"Successfully loaded image: {imagePath}");
                        }
                        else
                        {
                            Debug.LogWarning($"❌ Could NOT load image: {imagePath}. Make sure it's in Resources/Options.");
                        }
                    }
                    else if (!string.IsNullOrEmpty(option.text) && btnText != null)
                    {
                        btnText.text = option.text;
                        btnText.gameObject.SetActive(true);
                    }
                    else
                    {
                        Debug.LogError("TMP text not found in button prefab");
                    }
                    // image_v1.0 end

                    // Check if stats is null
                    if (option.stats == null)
                    {
                        Debug.Log($"Option '{option.text}' has NULL stats dictionary!");
                    }
                    else
                    {
                        //Debug.Log($"Option '{option.text}' has {option.stats.Count} stats entries:");
                        //foreach (var stat in option.stats)
                        //{
                        //    Debug.Log($"  -- Stat Key: '{stat.GetParsedKey()}', Value: {stat.value}");
                        //}
                    }

                    //Debug.Log($"option text for button: {option.text}, option nextId for button {option.nextId}");

                    // Assign click behavior
                    btn.onClick.AddListener(() => OnOptionSelected(option.text ?? option.image, option.nextId, option.stats));
                }
            }
            else
            {
                optionsContainer.SetActive(false);
            }
        }
    }

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool skipTyping = false;

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
            yield return new WaitForSeconds(0.03f); // Adjust speed as needed
        }

        isTyping = false;
    }
    private void Update()
    {
        // Handles screen taps/clicks to move on dialogue
        if (Input.GetMouseButtonDown(0) ||
           (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            //Debug.Log("Screen Clicked");

            if (isTyping)
            {
                // If currently typing, finish typing immediately
                skipTyping = true;
                return;
            }

            // Only advance if no choices are active (or if training mode has no choices)
            if (optionsContainer.activeSelf == false)
            {
                if (currentDialogue != null && currentDialogue.lines[0].options.Count == 0)
                {
                    // If NOT in training mode, auto-increment the highlight index as before.
                    if (!isTrainingMode)
                    {
                        trainingDialogueIndex++;
                        UpdateLetterHighlight();
                    }
                    // Continue to the next dialogue line regardless.
                    ContinueDialogue(currentDialogue.lines[0].nextId);
                }
            }
        }
    }

    void OnOptionSelected(string optionText, string nextId, List<StatPair> optionStats)
    {
        //Debug.Log("Player selected: " + optionText);

        if (stats == null)
        {
            stats = new Stats(); // Create if null
        }

        //Debug.Log("Stats Before Update:");
        foreach (var statType in System.Enum.GetValues(typeof(Stats.Type)))
        {
        //    Debug.Log($"{statType}: {stats.GetStatAmount((Stats.Type)statType)}");
        }

        // Update stats based on the stats passed in from the dialogue option
        if (optionStats != null && optionStats.Count > 0)
        {
            //Debug.Log("Current Stats from option:");
            foreach (var stat in optionStats)
            {
                stats.AddToStatAmount(stat.GetParsedKey(), stat.value);
                //Debug.Log($"Parsed Stat: {stat.GetParsedKey()}, Value: {stat.value}");
            }
        }
        else
        {
            //Debug.Log($"Option '{optionText}' has no stats to add.");
        }

        //Debug.Log("Stats After Update:");
        foreach (var statType in System.Enum.GetValues(typeof(Stats.Type)))
        {
        //    Debug.Log($"{statType}: {stats.GetStatAmount((Stats.Type)statType)}");
        }

        if (StatsManager.instance == null)
        {
            Debug.LogError("StatsManager instance is null!");
        }
        else
        {
            // Store stats persistently using PlayerPrefs or a Singleton
            StatsManager.instance.SaveStats(stats);
            // Retrieve the saved stats to verify
            Stats loadedStats = StatsManager.instance.GetStats();
            //Debug.Log("Stats After Saving:");
            foreach (var statType in System.Enum.GetValues(typeof(Stats.Type)))
            {
                //Debug.Log($"{statType}: {loadedStats.GetStatAmount((Stats.Type)statType)}");
            }
        }

        // Continue to next dialogue
        ContinueDialogue(nextId);
    }

    void ContinueDialogue(string nextId)
    {
        Debug.Log($"Calling ContinueDialogue, Moving to Next Dialogue Line Id:{nextId}");
        var nextDialogue = DialogueManager.instance.GetDialogueById(nextId);
        if (nextDialogue != null)
        {
            var line = currentDialogue.lines[0];
            Debug.Log($"this is the line: ({line}) and this is the trigger ({line.trigger})");
            if (!string.IsNullOrEmpty(line.trigger))
            {
                Debug.Log("Trigger param has been detected");
                StartCoroutine(ExecuteTriggerSequence(line.trigger, line.sound, line.nextId));
                return;
            }

            Debug.Log("This is outside the trigger param");
            DisplayDialogue(nextDialogue);
        }
        else
        {
            Debug.Log("Dialogue Ended");
            gameObject.SetActive(false); // Hide dialogue box when finished
            isDialogueActive = false;
        }
    }

    public void CloseDialogue()
    {
        Debug.Log("Dialogue closed.");
        isDialogueActive = false; // Allow player to move again
        gameObject.SetActive(false); // Hide dialogue box
    }

    /// <summary>
    /// Resets all letters to black, then highlights the current index red if valid.
    /// </summary>
    /// 
    IEnumerator ExecuteTriggerSequence(string triggerId, string soundName, string resumeId)
    {
        Debug.Log("inside ExecuteTriggerSequence");
        yield return ScreenFader.instance.FadeOut();

        if (!string.IsNullOrEmpty(soundName))
            yield return SoundManager.instance.PlaySoundAndWait(soundName);
        // "Options/Fire/[fire] fire extinguisher_ you try to extinguish it yourself.png"

        // trigger logic
        if (triggerId == "train_depart")
            yield return CutsceneController.instance.MoveTrain();
        else if (triggerId == "test_scene_change")
            BackgroundController.instance.ChangeTo("MRTOutside");

        yield return new WaitForSeconds(0.5f); // Optional pause

        yield return ScreenFader.instance.FadeIn();

        ContinueDialogue(resumeId);
    }

    private void UpdateLetterHighlight()
    {
        // Reset all letters to black
        foreach (var letter in factorLetters)
        {
            letter.color = Color.black;
        }
        // Highlight the current letter if it's in range
        if (trainingDialogueIndex >= 0 && trainingDialogueIndex < factorLetters.Count)
        {
            factorLetters[trainingDialogueIndex].color = Color.red;
        }
    }

    /// <summary>
    /// Returns the factor index based on the dialogue id.
    /// For example:
    /// "TrainingScene9"  => 0 (Cognitive load => C)
    /// "TrainingScene11" => 1 (Heuristics and biases => H)
    /// "TrainingScene13" => 2 (Information clarity => I)
    /// "TrainingScene15" => 3 (Mental models => M)
    /// "TrainingScene17" => 4 (External aid => E)
    /// Returns -1 if the dialogue id does not match any factor.
    /// </summary>
    private int GetFactorIndex(string dialogueId)
    {
        switch (dialogueId)
        {
            case "TrainingScene9":
                return 0; // Cognitive load -> C
            case "TrainingScene11":
                return 1; // Heuristics and biases -> H
            case "TrainingScene13":
                return 2; // Information clarity -> I
            case "TrainingScene15":
                return 3; // Mental models -> M
            case "TrainingScene17":
                return 4; // External aid -> E
            default:
                return -1;
        }
    }
}
