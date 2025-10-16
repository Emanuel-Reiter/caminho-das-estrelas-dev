using UnityEngine;

public class InGameTimer : MonoBehaviour {

    private float timer = 0.0f;
    private bool enableTimer = false;

    void Update() {
        if(Input.GetKeyDown(KeyCode.T)) enableTimer = !enableTimer;
        if (Input.GetKeyDown(KeyCode.R)) timer = 0.0f;

        if (enableTimer) timer += Time.deltaTime;
    }

    private void OnGUI() {
        int timerFix = (int)timer;

        GUI.skin.label.fontSize = 32;
        GUI.Label(new Rect(128, 256, 512, 48), "Timer: " + timerFix.ToString() + " segundos");
    }
}
