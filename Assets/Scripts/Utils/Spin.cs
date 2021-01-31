using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField]
    private float rotationsPerMinute = 10.0f;
    private void Update()
    {
        transform.Rotate(0, 6.0f * rotationsPerMinute * Time.deltaTime, 0);
    }
}