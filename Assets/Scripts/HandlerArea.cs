using UnityEngine;
using UnityEngine.InputSystem;

public class HandlerArea : MonoBehaviour
{
    [SerializeField] private Collider2D _areaCollider;

    public bool IsWithinSlingShotArea()
    {
        if (Mouse.current != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            return _areaCollider.OverlapPoint(mousePosition);
        }
        return false;
    }
}
