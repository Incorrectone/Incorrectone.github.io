using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchCheck : MonoBehaviour {

    [SerializeField] private LayerMask platformLayerMask;

    public bool isCrouchCheck;

    private void OnTriggerStay2D(Collider2D collider) {
        isCrouchCheck = collider != null && (((1 << collider.gameObject.layer) & platformLayerMask) != 0);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        isCrouchCheck = false;
    }

}
