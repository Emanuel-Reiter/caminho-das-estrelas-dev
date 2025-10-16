using System.Collections;
using UnityEngine;

public class BossExplosionBehaviour : MonoBehaviour
{
    public ParticleSystem effect1;
    public ParticleSystem effect2;
    public float damage = 50.0f;

    private Collider areaOfEffect;
    private Light effectLight;

    private void Start() {
        areaOfEffect = GetComponent<Collider>();
        effectLight = GetComponentInChildren<Light>();
        StartCoroutine(EffectCoroutine());
    }

    public void OnTriggerEnter(Collider other) {
        GameObject hitGO = other.gameObject;

        if (hitGO != null) {
            if (hitGO.tag == "Player") {
                PlayerStatusManager player = hitGO.GetComponent<PlayerStatusManager>();
                player.TakeDamage(damage);
            }
        }
    }

    private IEnumerator EffectCoroutine() {
        effect1.Play();
        effect2.Play();

        yield return new WaitForSeconds(0.25f);
        effectLight.gameObject.SetActive(false);
        areaOfEffect.enabled = false;

        yield return new WaitForSeconds(3.75f);
        Destroy(this.gameObject);
    }
}
