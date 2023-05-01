using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class EnemyController : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] private GameObject player;
    [SerializeField] private float runAwayDist;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float flashlightSpeed;
    [SerializeField] private float maxHealth;
    private float health;


    [Header("Health Bar")]
    [SerializeField] private Slider healthbar;
    [SerializeField] private GameObject healthFill;
    private NavMeshAgent navMeshAgent;
    private PlayerController playerController;

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = maxHealth;
        playerController = player.GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        // Update health
        healthbar.value = 1-(health/maxHealth);
        if (Mathf.Abs(health-maxHealth) < 0.01) healthFill.SetActive(false);
        else healthFill.SetActive(true);

        // Do navmesh stuff
        Vector3 target;
        if (playerController.inFlashlight(transform)) {
            navMeshAgent.speed = flashlightSpeed;
            target = player.transform.position + (transform.position-player.transform.position).normalized*runAwayDist;
        } else {
            navMeshAgent.speed = normalSpeed;
            target = player.transform.position;
        }
        navMeshAgent.destination = target;
    }

    public void Hit(float damage) {
        Debug.Log("got hit");
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }


}
