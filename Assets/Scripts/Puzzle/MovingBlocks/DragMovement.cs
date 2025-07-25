using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CollisionHandler))]
public class DragMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _collisionCheckRadius = 0.5f;
    [SerializeField] private float _stoppingDistance = 0.01f;
    [SerializeField] private float _velocityThreshold = 0.1f;
    [SerializeField] private float _dragDamping = 0.001f;

    private Rigidbody _rigidbody;
    private CollisionHandler _collisionHandler;
    private float _sqrDistance;
    private float _distance;
    private float _moveSpeed;
    private Vector3 _toTarget;
    private Vector3 _direction;
    private Vector3 _currentTarget;
    private bool _isDragging;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collisionHandler = GetComponent<CollisionHandler>();
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    public void StartDragging()
    {
        _rigidbody.isKinematic = false;
        _isDragging = true;
        _rigidbody.velocity = Vector3.zero;
    }

    public void StopDragging()
    {
        _isDragging = false;
        _rigidbody.isKinematic = true;
    }

    public void HandleMovement(Vector3 newPosition)
    {
        //_currentTarget = newPosition;

        if (HasReachedTarget(newPosition))
        {
            _rigidbody.velocity *= _dragDamping;
            return;
        }

        if (IsPathBlocked(newPosition))
        {
            _rigidbody.velocity *= _dragDamping;
            return;
        }

        ApplyMovement();
    }

    public bool ConfirmMovement()
    {
        return _rigidbody.velocity.sqrMagnitude < _velocityThreshold * _velocityThreshold;
    }

    private bool HasReachedTarget(Vector3 targetPosition)
    {
        _toTarget = targetPosition - _rigidbody.position;
        _sqrDistance = _toTarget.sqrMagnitude;
        return _sqrDistance < _stoppingDistance * _stoppingDistance;
    }

    private bool IsPathBlocked(Vector3 targetPosition)
    {
        if (_collisionHandler.IsCollidingWithObstacle) return true;

        _toTarget = targetPosition - _rigidbody.position;
        _distance = _toTarget.magnitude;
        _direction = _toTarget / _distance;

        return _collisionHandler.CheckPathObstructed(_rigidbody.position, _direction, _distance, _collisionCheckRadius);
    }

    private void ApplyMovement()
    {
        _moveSpeed = Mathf.Min(_speed, _distance * _speed);

        Vector3 targetVelocity = _direction * _moveSpeed;
        targetVelocity.y = _rigidbody.velocity.y;

        if (_isDragging)
        {
            _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity,targetVelocity,Time.deltaTime * _speed);
        }
    }
}