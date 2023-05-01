using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] private float flashlightEffectRange;
    [SerializeField][Range(0, 1)] private float flashlightEffectAngleRange;
    [SerializeField] private float attackRange;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] private float damage;
    
    [Header("Input")]
    [SerializeField] private PlayerInput playerInput;
    private InputAction flashAction;
    private InputAction attackAction;

    // Start is called before the first frame update
    void Start() {
        flashAction = playerInput.actions.FindAction("flashlight");
        attackAction = playerInput.actions.FindAction("attack");

    }

    // Update is called once per frame
    void Update()
    {
        if (attackAction.triggered) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, enemyLayer)) {
                GameObject enemy = hit.collider.gameObject;
                if (Vector3.Distance(enemy.transform.position, transform.position) <= attackRange && inFlashlight(enemy.transform)) {
                    enemy.GetComponent<EnemyController>().Hit(damage);
                }
            }
        }
    }

    public bool inFlashlight(Transform other) {
        if (!(flashAction.ReadValue<float>() > 0.125)) return false; // Flashlight is currently disabled
        double angle = Vector3.Dot(other.position - transform.position, transform.forward);
        if (angle < flashlightEffectAngleRange || Vector3.Distance(other.position, transform.position) > flashlightEffectRange)
            return false;
        return true;
    }
}
