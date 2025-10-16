using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingDialgoue : MonoBehaviour {

    [TextArea] public string[] dialogue;
    public string[] speakerName;
    public GameObject[] emotions;

    public TMP_Text dialogueText;
    public TMP_Text speakerNameText;

    public int currentDialogue = 0;

    public GameObject dialogueScreen;

    public GameObject nextButton;
    public GameObject endButton;

    public GameObject creditsScreen;
    public RectTransform credits;

    public float creditsSpeed;

    private bool isCreditsActive;

    float endTime = 33.0f;

    private void Start() {
        NextDialogue();
    }

    private void Update() {
        if(isCreditsActive) {
            credits.position += new Vector3(0.0f, creditsSpeed * Time.deltaTime, 0.0f);
            endTime -= Time.deltaTime;
        }

        if (endTime < 0.0f) {
            SceneManager.LoadScene("TitleScreen");
            SceneManager.UnloadSceneAsync("EndingScene");
        }
    }

    public void NextDialogue() {
        foreach (GameObject emotion in emotions) {
            emotion.SetActive(false);
        }

        nextButton.SetActive(true);

        dialogueText.text = dialogue[currentDialogue];
        speakerNameText.text = speakerName[currentDialogue];
        emotions[currentDialogue].SetActive(true);

        if (currentDialogue < dialogue.Length - 1) currentDialogue++;
        else {
            nextButton.SetActive(false);
            endButton.SetActive(true);
        }
    }

    public void EndDialogue() {
        dialogueScreen.SetActive(false);

        creditsScreen.SetActive(true);

        isCreditsActive = true;
    }
}
