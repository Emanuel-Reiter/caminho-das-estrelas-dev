using UnityEngine;

interface IInteractable {
    public void Interact();
}

public class Interaction : MonoBehaviour {

    [SerializeField] private Transform interactionOrigin;
    [SerializeField] private float interactionRadius;
    [SerializeField] private LayerMask interactionLayer;

    private PlayerInputManager input;
    private InterfaceManager interfaceManager;

    private void Start() {
        input = GetComponent<PlayerInputManager>();
        interfaceManager = GetComponent<InterfaceManager>();
}

    private void Update() {
        Collider[] objectsInRange = Physics.OverlapSphere(interactionOrigin.position, interactionRadius, interactionLayer);
        foreach (Collider hitInfo in objectsInRange) {
            if (hitInfo.gameObject.TryGetComponent(out IInteractable interactionTarget)) {
                if (input.interactInput) interactionTarget.Interact();
            }
        }

        if (objectsInRange.Length > 0) {
            foreach(Collider hit in objectsInRange) {
                 if(hit.gameObject.TryGetComponent(out UpgradeAltarBehaviour selectedAltar)) {
                    if (!selectedAltar.hasBeenUsed) interfaceManager.EnableInteractionPopup();
                    else interfaceManager.CloseAllInteractionPopups();
                }
            }
        }
        else interfaceManager.CloseAllInteractionPopups();
    }
}
