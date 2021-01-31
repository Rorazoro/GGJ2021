using UnityEngine;

public class ItemSpawnPoint : MonoBehaviour
{
    private void Awake() => ItemSpawnSystem.AddSpawnPoint(transform);
    private void OnDestroy() => ItemSpawnSystem.RemoveSpawnPoint(transform);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
    }
}