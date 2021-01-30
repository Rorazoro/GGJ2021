using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public LayerMask _CanBeClicked;
    private NavMeshAgent _NavMeshAgent;
    private PlayerInput _PlayerInput;
    private Vector2 _MousePosition;

    private void Start() {
        _NavMeshAgent = GetComponent<NavMeshAgent>();
        _PlayerInput = GetComponent<PlayerInput>();
    }

    public void OnMousePosition(InputValue value)
    {
        _MousePosition = value.Get<Vector2>();
    }

    public void OnMouseClick(InputValue value)
    {
        Vector2 mousePosition = _MousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 100, _CanBeClicked))
        {
            _NavMeshAgent.SetDestination(hitInfo.point);
        }
    }
}
