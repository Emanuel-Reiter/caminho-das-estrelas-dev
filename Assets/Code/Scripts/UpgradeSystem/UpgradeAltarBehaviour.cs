using UnityEngine;

public class UpgradeAltarBehaviour : MonoBehaviour, IInteractable {

    [HideInInspector] public bool hasBeenUsed { get; private set; } = false;
    private Animator altarAnimator;
    private AudioSource altarAudioSource;
    private ParticleSystem altarParticleEffect;
    private Light altarPointLight;

    private InterfaceManager playerInterface;
    private PlayerStatusManager playerStatusManager;

    private bool enableAltarDeactivationAnimation;

    private float deactivateTime = 4.0f;

    private void Start () {
        altarAnimator = GetComponentInChildren<Animator>();
        altarAudioSource = GetComponent<AudioSource>();
        altarParticleEffect = GetComponentInChildren<ParticleSystem>();
        altarPointLight = GetComponentInChildren<Light>();

        playerInterface = GameObject.FindGameObjectWithTag("Player").GetComponent<InterfaceManager>();
        playerStatusManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatusManager>();
    }

    public void Interact() {
        if (!hasBeenUsed) {
            playerStatusManager.SetCurrentAltar(this);
            playerInterface.OpenUpgradeMenu();
        }
        else return;
    }

    private void Update () {
        if(enableAltarDeactivationAnimation) {
            altarAudioSource.volume = Mathf.Lerp(altarAudioSource.volume, 0.0f, deactivateTime);
            altarPointLight.shadowStrength = Mathf.Lerp(altarPointLight.shadowStrength, 0.0f, deactivateTime);
            altarPointLight.intensity = Mathf.Lerp(altarPointLight.intensity, 0.0f, deactivateTime);
        }
    }

    public void SetAltarAsUsed() { 
        hasBeenUsed = true;
        altarAnimator.CrossFadeInFixedTime("Deactivated", deactivateTime);
        altarParticleEffect.Stop();
        enableAltarDeactivationAnimation = true;

        playerInterface.EnableHud(InterfaceManager.HUDs.PlayerStatus);
    }
}
