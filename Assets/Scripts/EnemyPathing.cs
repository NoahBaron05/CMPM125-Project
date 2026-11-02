using UnityEngine;
using Pathfinding;

public class EnemyPathing : MonoBehaviour
{
    AIDestinationSetter destinationSetter;
    AIPath aiPath;

    [SerializeField] float chaseDistance = 15f;
    [SerializeField] float resumeDistance = 13f;

    void Start()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            destinationSetter.target = player.transform;
        }
    }

    void Update()
    {
        if (destinationSetter.target == null) return;

        float distance = Vector2.Distance(transform.position, destinationSetter.target.position);

        if (!aiPath.isStopped && distance > chaseDistance)
        {
            aiPath.isStopped = true;
        }
        else if (aiPath.isStopped && distance < resumeDistance)
        {
            aiPath.isStopped = false;
        }
    }
}
