using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSpawnSystem : NetworkBehaviour
{
    [SerializeField] private List<GameObject> itemPrefabs = null;

    private static List<Transform> spawnPoints = new List<Transform>();

    private int nextIndex = 0;

    public static void AddSpawnPoint(Transform transform)
    {
        spawnPoints.Add(transform);

        spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }
    public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

    public override void OnStartServer() => NetworkManagerLobby.OnServerReadied += SpawnItems;

    public override void OnStartClient()
    {
        // InputManager.Add(ActionMapNames.Player);
        // InputManager.Controls.Player.Look.Enable();
    }

    [ServerCallback]
    private void OnDestroy() => NetworkManagerLobby.OnServerReadied -= SpawnItems;

    [Server]
    public void SpawnItems(NetworkConnection conn)
    {
        itemPrefabs.Shuffle();

        foreach (GameObject itemPrefab in itemPrefabs)
        {
            Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);

            if (spawnPoint == null)
            {
                Debug.LogError($"Missing spawn point for item {nextIndex}");
                return;
            }

            GameObject itemInstance = Instantiate(itemPrefab, spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation);
            NetworkServer.Spawn(itemInstance, conn);
            nextIndex++;
        }
        nextIndex = 0;
    }
}
