using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class PlayerSpawnManager : NetworkBehaviour
{
    private static List<Transform> _SpawnPoints = new List<Transform>();

    public static void AddSpawnPoint(Transform transform)
    {
        _SpawnPoints.Add(transform);
        _SpawnPoints = _SpawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }

    public static void RemoveSpawnPoint(Transform transform) => _SpawnPoints.Remove(transform);
}