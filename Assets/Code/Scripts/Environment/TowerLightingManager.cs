using System.Collections;
using UnityEngine;

public class TowerLightingManager : MonoBehaviour {

    [SerializeField] private GameObject towerReflectionProbes;

    private void Start () {
        RenderSettings.ambientIntensity = 1.0f;
        ToggleTowerLighting(false);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) ToggleTowerLighting(true);
        else return;
    }

    private void OnTriggerExit(Collider other) { 
        if(other.gameObject.CompareTag("Player")) ToggleTowerLighting(false);
        else return;
    }

    private void ToggleTowerLighting(bool toggle) {
        float targetIntensity = toggle ? 0.5f : 1.0f;
        StartCoroutine(LightingChangeCoroutine(targetIntensity));
      
        towerReflectionProbes.SetActive(toggle);
    }

    private IEnumerator LightingChangeCoroutine(float targetIntensity) {
        float initialIntensity = RenderSettings.ambientIntensity;
        
        float elapsedTIme = 0.0f;
        float lerpTime = 0.5f;

        while (elapsedTIme < lerpTime) {
            RenderSettings.ambientIntensity = Mathf.Lerp(initialIntensity, targetIntensity, (elapsedTIme/lerpTime));

            elapsedTIme += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
