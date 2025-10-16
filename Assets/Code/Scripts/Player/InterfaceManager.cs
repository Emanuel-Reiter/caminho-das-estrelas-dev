using TMPro;
using UnityEngine;

public class InterfaceManager : MonoBehaviour  {

    public enum CurrentDialogue { 
        Beginning, 
        Ending,
    };

    public enum HUDs {
        PlayerStatus,
        BossHealthBar,
    };

    [Header("HUDs")]
    [SerializeField] private GameObject playerHUD;
    [SerializeField] private GameObject bossHUD;
    
    [Header("Game UI Windows")]
    [SerializeField] private GameObject pauseUIWindow;
    [SerializeField] private GameObject upgradeUIWindow;

    [Header("Dialogue Windows")]
    [SerializeField] private GameObject beginningDialogue;
    [SerializeField] private GameObject endingDialogue;

    [Header("Interaction Popups")]
    [SerializeField] private GameObject interactionPopup;

    private bool openGameUIWIndow = false;
    private bool canCheckForPopup = true;

    private PlayerInputManager input;
    private PlayerHelper helper;

    private void Start() {
        input = GetComponent<PlayerInputManager>();
        helper = GetComponent<PlayerHelper>();

        ToggleGamePause(true);
    }

    private void Update() {
        // Checks for the player escape input, after that checks if there's
        // any game UI window currently open, if there's any it get closed,
        // if not opens the pause menu.
        if(input.escapeInput) {
            if(openGameUIWIndow) {
                CloseAllGameWindows();
                CloseDialogueWindows();

                ToggleGamePause(false);

                openGameUIWIndow = false;
                EnableHud(HUDs.PlayerStatus);
            }
            else {
                OpenPauseMenu();
            }
        }
    }

    private void CloseAllHUDs() {
        playerHUD.SetActive(false);
        bossHUD.SetActive(false);
    }

    private void CloseAllGameWindows() {
        ToggleGamePause(false);

        pauseUIWindow.SetActive(false);
        upgradeUIWindow.SetActive(false);
    }

    public void CloseDialogueWindows() {
        ToggleGamePause(false);

        beginningDialogue.SetActive(false);
        endingDialogue.SetActive(false);
    }

    public void CloseAllInteractionPopups() {
        interactionPopup.SetActive(false);
    }

    private void CloseAll() {
        CloseAllHUDs();
        CloseAllGameWindows();
        CloseDialogueWindows();
        CloseAllInteractionPopups();
    }

    public void CloseUpgradeMenu() {
        upgradeUIWindow.SetActive(false);
        ToggleGamePause(false);
    }

    private void ToggleGamePause(bool pause) {
        LockCursor(!pause);

        canCheckForPopup = !pause;

        if (pause) Time.timeScale = 0.0f;
        else Time.timeScale = 1.0f;
    }

    private void LockCursor(bool locked) {
        if (locked) Cursor.lockState = CursorLockMode.Locked;
        else Cursor.lockState = CursorLockMode.None;

        Cursor.visible = !locked;
    }

    public void EnableHud(HUDs hud) {
        switch (hud) {
            case HUDs.PlayerStatus: playerHUD.SetActive(true); break;
            case HUDs.BossHealthBar: bossHUD.SetActive(true); break;
            default: return;
        }
    }

    private void OpenPauseMenu() {
        CloseAll();

        ToggleGamePause(true);
        pauseUIWindow.SetActive(true);
        openGameUIWIndow = true;
    }

    public void OpenUpgradeMenu() {
        CloseAll();

        ToggleGamePause(true);
        upgradeUIWindow.SetActive(true);
        openGameUIWIndow = true;
    }

    public void EnableInteractionPopup() {
        /*
        The EnableInteractionMethod will only be called if there's
        an altar aviable to interaction, then when it's called uses
        the canCheckForPopup variable as the visibility parameter for
        the interaction popup, the varible it's only true when there's
        not a game window currently open.
        */
        interactionPopup.SetActive(canCheckForPopup);
    }

    public void StartDialogue(CurrentDialogue dialogue) {
        CloseDialogueWindows();
        openGameUIWIndow = true;

        switch (dialogue) {
            case CurrentDialogue.Beginning: beginningDialogue.SetActive(true); break;
            case CurrentDialogue.Ending: endingDialogue.SetActive(true); break;
            default:
                openGameUIWIndow = false;
                return;
        }
    }
}
