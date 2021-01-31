using Mirror;
using UnityEngine;

public class PickupObjectController : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log($"{gameObject.name} picked up.");
        }
    }
}