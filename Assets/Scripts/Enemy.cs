using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public LayerMask detectionLayer;
    public float detectionRange = 10f;
    public float detectionAngle = 45f;
    public float playerStoppingDistance = 0;
    public NavMeshAgent _NavMeshAgent;
    public BoxCollider floorCollider;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public bool IsPathAssigned = false;
    public bool IsPlayerDetected = false;

    public float damage = 0;
    public float damageInterval = 1f;
    private float nextDamageTime = 0f;

    void Start()
    {
        nextDamageTime = Time.time + damageInterval;
    }
    void Update()
    {
        DetectEnemies();
        if(IsPathAssigned && !IsPlayerDetected)
            if (!_NavMeshAgent.pathPending && _NavMeshAgent.remainingDistance < 0.25f)
            {
                SetDestination();
            }
    }
    void SetDestination()
    {
        Vector3 randomPoint = Vector3.Lerp(startPoint, endPoint, Random.value);
        _NavMeshAgent.SetDestination(randomPoint);
    }

    void DetectEnemies()
    {
        for (float angle = -detectionAngle / 2f; angle <= detectionAngle / 2f; angle += 1f)
        {
            Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * transform.forward;

            if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, detectionRange, detectionLayer))
            {
                Transform target = hit.transform;
                _NavMeshAgent.stoppingDistance = playerStoppingDistance;
                _NavMeshAgent.SetDestination(target.position);
                if (Time.time >= nextDamageTime)
                {
                    hit.collider.GetComponent<PlayerHealth>().TakeDamage(damage);

                    nextDamageTime = Time.time + damageInterval;
                }
                if (hit.collider.CompareTag("Player"))
                {
                    IsPlayerDetected = true;
                    Debug.DrawRay(transform.position, rayDirection * detectionRange, Color.red);
                }
            }
            else
            {
                Debug.DrawRay(transform.position, rayDirection * detectionRange, Color.green);
                _NavMeshAgent.stoppingDistance = 0.1f;
                IsPlayerDetected = false;
            }
        }
    }
}

