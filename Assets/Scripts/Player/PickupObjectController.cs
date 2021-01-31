using Mirror;
using UnityEngine;

public class PickupObjectController : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            NetworkGamePlayer player = other.gameObject.GetComponent<NetworkGamePlayer>();
            player.IncreaseScore();
            Destroy(gameObject);
        }
    }
}