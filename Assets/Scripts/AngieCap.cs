using UnityEngine;

public class AngieCap : MonoBehaviour
{
    private Rigidbody2D _rb;
    private CapsuleCollider2D _capsuleCollider;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        _rb.bodyType = RigidbodyType2D.Kinematic; // Use bodyType instead of isKinematic
        _capsuleCollider.enabled = false;
    }

    public void LaunchCap(Vector2 direction, float force)
    {
        _rb.bodyType = RigidbodyType2D.Dynamic; // Use bodyType instead of isKinematic
        _capsuleCollider.enabled = true;

        _rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
