using UnityEngine;

public class SmoothMoveToTarget : MonoBehaviour
{
    [SerializeField] private Pen _target;
    [SerializeField] private Transform _waypoint;

    private SmoothAppearance _smoothAppearance;
    private bool _isMoving;
    private bool _destroyOnArrival;
    private float _minDistance;
    private float _delayTimer;
    private float _movementSpeed;
    private bool _reachedWaypoint;
    private float _waypointXOffset;
    private float _randomXOffset;
    private Vector3 _modifiedWaypointPosition;

    public bool IsMoving { get; private set; }

    private void Awake()
    {
        gameObject.SetActive(false);

        _waypointXOffset = 0.5f;
        _delayTimer = 1;
        _movementSpeed = 20;
        _minDistance = 0.1f;
        _isMoving = false;
        _destroyOnArrival = true;
        _smoothAppearance = GetComponent<SmoothAppearance>();
        _reachedWaypoint = false;
        _randomXOffset = 0f;
        _modifiedWaypointPosition = Vector3.zero;
    }

    private void Update()
    {
        if (IsMoving == false) return;
        
        HandleMovementDelay();

        if (_isMoving == false || _target == null) return;

        MoveTowardsWaypointOrTarget();
        CheckArrival();
    }

    public void TurnOff()
    {
        IsMoving = true;
        _randomXOffset = Random.Range(-_waypointXOffset, _waypointXOffset);

        if (_waypoint != null)
        {
            _modifiedWaypointPosition = _waypoint.position;
            _modifiedWaypointPosition.x += _randomXOffset;
        }
    }

    private void MoveTowardsWaypointOrTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, GetTargetPosition(), _movementSpeed * Time.deltaTime);
    }

    private void CheckArrival()
    {
        if (CheckWaypointArrival()) return;

        CheckTargetArrival();
    }

    private bool CheckWaypointArrival()
    {
        if (_reachedWaypoint || _waypoint == null)
            return false;

        if (GetDistance(transform.position, _modifiedWaypointPosition) <= _minDistance)
        {
            _reachedWaypoint = true;
            _smoothAppearance.Disappear();
            return true;
        }

        return false;
    }

    private void CheckTargetArrival()
    {
        if (_target == null)
            return;

        if (GetDistance(transform.position, _target.transform.position) <= _minDistance)
        {
            ArrivalAction();
        }
    }

    private float GetDistance(Vector3 position, Vector3 target)
    {
         return Vector3.Distance(position, target);
    }

    private Vector3 GetTargetPosition()
    {
        if (_reachedWaypoint || _waypoint == null)
            return _target.transform.position;

        return _modifiedWaypointPosition;
    }

    private void HandleMovementDelay()
    {
        if (_isMoving) return;

        _delayTimer -= Time.deltaTime;

        if (_delayTimer <= 0)
        {
            _isMoving = true;
        }
    }

    private void ArrivalAction()
    {
        if (_destroyOnArrival)
        {
            //_smoothAppearance.Disappear();
        }
    }
}