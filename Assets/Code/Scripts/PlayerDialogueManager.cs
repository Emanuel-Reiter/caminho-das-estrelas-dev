using TMPro;
using UnityEngine;
using static InterfaceManager;

public class PlayerDialogueManager : MonoBehaviour {

    [Header("Dialogue Data")]
    [TextArea] public string[] dialogue;
    public string[] speakerName;
    public GameObject[] speakerExpressions;

    [Header("UI References")]
    public TMP_Text dialogueText;
    public TMP_Text speakerNameText;

    public int currentDialogue = 0;

    public GameObject playerHUD;

    public GameObject nextDialogueButton;
    public GameObject endDialogueButton;

    public GameObject dialogueScreen;

    public bool endedDialogue = false;

    private InterfaceManager playerInterface;

    private void Start() {
        playerInterface = GetComponent<InterfaceManager>();

        StartBiginningDialogue();
    }

    private void NextDialogue() {
        // Disable the speaker expression graphic before setting the 
        // current correct expression for the current dialogue line.
        foreach(GameObject expressions in speakerExpressions) {
            expressions.SetActive(false);
        }

        nextDialogueButton.SetActive(true);

        dialogueText.text = dialogue[currentDialogue];
        speakerNameText.text = speakerName[currentDialogue];
        speakerExpressions[currentDialogue].SetActive(true);

        if (currentDialogue < dialogue.Length -1) currentDialogue++;
        else {
            nextDialogueButton.SetActive(false);
            endDialogueButton.SetActive(true);
        }
    }

    private void EndDialogue() {
        playerInterface.CloseDialogueWindows();
        playerInterface.EnableHud(HUDs.PlayerStatus);
    }

    //Dialogue interface buttons
    public void NextDialogueUIButton() { NextDialogue(); }
    public void EndDialogueUIButton() { EndDialogue(); }

    //Start dialogues
    public void StartBiginningDialogue() { 
        playerInterface.StartDialogue(CurrentDialogue.Beginning);
        NextDialogue();
    }
    public void StartEndingDialogue() { 
        playerInterface.StartDialogue(CurrentDialogue.Ending);
    }


}
