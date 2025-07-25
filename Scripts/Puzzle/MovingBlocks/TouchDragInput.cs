using UnityEngine;

[RequireComponent(typeof(DragMovement), typeof(Magnifier))]
public class TouchDragInput : MonoBehaviour
{
    [SerializeField] private EffectsHandler _effectsHandler;

    private IEffectsHandler _ieffectsHandler;
    private bool _isSelected;
    private Ray _ray;
    private Touch _touch;
    private Vector3 _offset;
    private Plane _movementPlane;
    private Camera _mainCamera;
    private DragMovement _dragMovement;
    private Magnifier _selectable;

    private bool _isMovement;

    public bool IsOver { get; private set; }

    private void Awake()
    {
        _mainCamera = Camera.main;
        _dragMovement = GetComponent<DragMovement>();
        _selectable = GetComponent<Magnifier>();

        _ieffectsHandler = _effectsHandler;

        if (_ieffectsHandler == null)
        {
            Debug.LogError("EffectsHandler not assigned in TouchDragInput", this);
        }
        if (_dragMovement == null)
        {
            Debug.LogError("DragMovement not assigned in TouchDragInput", this);
        }
        if (_selectable == null)
        {
            Debug.LogError("SelectableObject not assigned in TouchDragInput", this);
        }
        _movementPlane = new Plane(Vector3.down, transform.position);
    }

    private void Update()
    {
        if (Input.touchCount == 0 && IsOver == false) return;

        _touch = Input.GetTouch(0);

        switch (_touch.phase)
        {
            case TouchPhase.Began:
                SelectBlock(_touch.position);
                break;

            case TouchPhase.Moved:
                Move(_touch.position);
                break;

            case TouchPhase.Stationary:
                TurnOffSoundMovement();
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                ThrowOff();
                break;
        }
    }

    private void SelectBlock(Vector3 position)
    {
        if (IsTouchingThisObject(position))
        {
            _isSelected = true;
            _selectable.Select();
            _dragMovement.StartDragging();
            _offset = transform.position - SetContactPosition(position);
        }
    }

    private void Move(Vector3 position)
    {
        if (_isSelected)
        {
            _dragMovement.HandleMovement(SetContactPosition(position) + _offset);

            if (TryGetConfirmationOfTransfer())
            {
                _isMovement = true;
                _effectsHandler.PlayDragSound();
            }
        }
    }

    private void TurnOffSoundMovement()
    {
        if (TryGetConfirmationOfTransfer())
        {
            _isMovement = false;
            _ieffectsHandler?.Stop();
        }
    }


    private bool TryGetConfirmationOfTransfer()
    {
        return _dragMovement.ConfirmMovement() == _isMovement;
    }

    private void ThrowOff()
    {
        if (_isSelected)
        {
            _isSelected = false;
            _isMovement = false;
            _selectable.Deselect();
            _dragMovement.StopDragging();
            _ieffectsHandler?.Stop(); 
            _ieffectsHandler?.PlayEndDragging();
        }
    }

    private Vector3 SetContactPosition(Vector3 position)
    {
        return GetTouchWorldPosition(position);
    }

    private Vector3 GetTouchWorldPosition(Vector2 screenPosition)
    {
        if (_movementPlane.Raycast(GetRayFromScreen(screenPosition), out float distance))
        {
            return _ray.GetPoint(distance);
        }

        return transform.position;
    }

    private bool IsTouchingThisObject(Vector2 screenPosition)
    {
        if (Physics.Raycast(GetRayFromScreen(screenPosition), out RaycastHit hit))
        {
            return hit.collider.transform == transform || hit.collider.transform.IsChildOf(transform);
        }

        return false;
    }

    private Ray GetRayFromScreen(Vector2 vector)
    {
        _ray = _mainCamera.ScreenPointToRay(vector);

        return _ray;
    }
}