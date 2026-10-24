using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Unity.AI.Navigation;

public class NavigationDemo : MonoBehaviour
{
    [Header("Scene references")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform destinationMarker;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private NavMeshLink bridgeLink;
    [SerializeField] private GameObject bridgeVisual;
    [SerializeField] private DataCollectible[] collectibles;
    [SerializeField] private Transform[] destinations;

    [Header("Runtime observation")]
    [SerializeField] private int score;
    [SerializeField] private string navigationState = "Choose a destination";

    private Vector3 startingPosition;
    private bool hasDestination;

    public string Status => navigationState + "\nCollected value: " + score;

    private void Start()
    {
        startingPosition = agent.transform.position;
        bridgeVisual.SetActive(bridgeLink.enabled);
    }

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame &&
            (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject()))
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out var hit, 200f))
                MoveTo(hit.point);
        }

        if (!agent.isOnNavMesh)
        {
            navigationState = "Agent is not on a NavMesh";
            return;
        }

        if (hasDestination)
        {
            if (agent.pathPending)
                navigationState = "Calculating path...";
            else if (agent.pathStatus != NavMeshPathStatus.PathComplete)
                navigationState = "Path: " + agent.pathStatus + " (destination unreachable)";
            else if (agent.remainingDistance <= agent.stoppingDistance + 0.05f && agent.velocity.sqrMagnitude < 0.01f)
                navigationState = "Arrived: complete path";
            else
                navigationState = "Complete path: " + agent.remainingDistance.ToString("F1") + " m remaining";
        }

        foreach (var collectible in collectibles)
        {
            if (!collectible.gameObject.activeSelf)
                continue;

            if (Vector3.Distance(agent.transform.position, collectible.transform.position) < 1.5f)
            {
                score += collectible.Data.Value;
                collectible.gameObject.SetActive(false);
            }
        }
    }

    public void Visit(int index)
    {
        MoveTo(destinations[index].position);
    }

    private void MoveTo(Vector3 position)
    {
        if (!NavMesh.SamplePosition(position, out var hit, 2f, agent.areaMask))
        {
            navigationState = "No nearby walkable surface";
            hasDestination = false;
            return;
        }

        destinationMarker.position = hit.position + Vector3.up * 0.1f;
        hasDestination = agent.SetDestination(hit.position);

        if (!hasDestination)
            navigationState = "Destination request rejected";
    }

    public void ToggleObstacle()
    {
        obstacle.SetActive(!obstacle.activeSelf);
    }

    public void ToggleLink()
    {
        bridgeLink.enabled = !bridgeLink.enabled;
        bridgeVisual.SetActive(bridgeLink.enabled);

        if (hasDestination)
            agent.SetDestination(destinationMarker.position);
    }

    public void ResetDemo()
    {
        obstacle.SetActive(false);
        bridgeLink.enabled = true;
        bridgeVisual.SetActive(true);
        agent.Warp(startingPosition);
        agent.ResetPath();
        hasDestination = false;
        score = 0;
        navigationState = "Choose a destination";
        destinationMarker.position = startingPosition;

        foreach (var collectible in collectibles)
            collectible.gameObject.SetActive(true);
    }
}
