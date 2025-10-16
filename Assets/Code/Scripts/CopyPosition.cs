using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour {

    [SerializeField] private Transform objectPosiotion;

    private void LateUpdate() {
        this.transform.position = objectPosiotion.position;
        this.transform.rotation = objectPosiotion.rotation;
    }
}
