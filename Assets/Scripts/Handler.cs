using UnityEngine;
using UnityEngine.InputSystem;

public class Handler : MonoBehaviour
{
    [Header("Line Renderers")]
    [SerializeField] private LineRenderer _leftLineRenderer;
    [SerializeField] private LineRenderer _rightLineRenderer;

    [Header("Transform References")]
    [SerializeField] private Transform _leftStartPosition;
    [SerializeField] private Transform _rightStartPosition;
    [SerializeField] private Transform _centerPosition;
    [SerializeField] private Transform _idlePosition;

    [Header("Slingshot Stats")]
    [SerializeField] private float _maxDistance = 3.5f;
    [SerializeField] private float _shotForce = 5f;

    [Header("Scripts")]
    [SerializeField] private HandlerArea _slingShotArea; // Use HandlerArea instead of SlingShotArea

    [Header("Caps")]
    [SerializeField] private AngieCap _capyPreFab;
    [SerializeField] private float _angieCapPositionOffset = 2f;

    private Vector2 _slingShotLinesPosition;

    private Vector2 _direction;
    private Vector2 _directionNormalized;

    private bool _clickedWithinArea;

    private AngieCap _spawnedAngieCap;

    private void Awake()
    {
        _leftLineRenderer.enabled = false;
        _rightLineRenderer.enabled = false;

        SpawnAngieCapy();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && _slingShotArea.IsWithinSlingShotArea())
        {
            _clickedWithinArea = true;
        }

        var mouse = Mouse.current;
        if (mouse != null && mouse.leftButton.isPressed && _clickedWithinArea)
        {
            DrawSlingshot();
            PositionAndRotateAngieCap();
        }

        if (mouse != null && mouse.leftButton.wasReleasedThisFrame)
        {
            _clickedWithinArea = false;

            _spawnedAngieCap.LaunchCap(_direction, _shotForce);
        }
    }

    #region SlingShot Methods    

    private void DrawSlingshot()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        touchPosition.z = 0;

        _slingShotLinesPosition = _centerPosition.position + Vector3.ClampMagnitude(touchPosition - _centerPosition.position, _maxDistance);
        
        SetLines(_slingShotLinesPosition);

        _direction = (Vector2)_centerPosition.position - _slingShotLinesPosition;
        _directionNormalized = _direction.normalized;
    }

    private void SetLines(Vector2 position)
    {
        if (!_leftLineRenderer.enabled && !_rightLineRenderer.enabled)
        {
            _leftLineRenderer.enabled = true;
            _rightLineRenderer.enabled = true;
        }

        _leftLineRenderer.SetPosition(0, position);
        _leftLineRenderer.SetPosition(1, _leftStartPosition.position);

        _rightLineRenderer.SetPosition(0, position);
        _rightLineRenderer.SetPosition(1, _rightStartPosition.position);
    }
    #endregion

    #region Capy Methods

    private void SpawnAngieCapy()
    {
        SetLines(_idlePosition.position);

        Vector2 dir = (_centerPosition.position - _idlePosition.position).normalized;
        Vector2 spawnPosition = (Vector2)_idlePosition.position - dir * _angieCapPositionOffset;

        _spawnedAngieCap = Instantiate(_capyPreFab, spawnPosition, Quaternion.identity);
        _spawnedAngieCap.transform.right = dir;
    }

    private void PositionAndRotateAngieCap()
    {
        _spawnedAngieCap.transform.position = _slingShotLinesPosition + _directionNormalized * _angieCapPositionOffset;
        _spawnedAngieCap.transform.right = _directionNormalized;
    }

    #endregion

}
