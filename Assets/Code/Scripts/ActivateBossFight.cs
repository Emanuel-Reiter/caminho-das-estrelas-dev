using UnityEngine;

public class ActivateBossFight : MonoBehaviour {
    public BossHelper helper;

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            helper.TargerPlayer();
            this.gameObject.SetActive(false);
        }
    }
}
