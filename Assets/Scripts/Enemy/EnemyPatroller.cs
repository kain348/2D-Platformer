using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[AddComponentMenu("Game/Enemy/Enemy Patroller")]
public class EnemyPatroller : MonoBehaviour
{
    private const float DirectionDeadZone = 0.001f;

    [Header("Route")]
    [SerializeField] private PatrolRoute _route;
    [SerializeField] private bool _startOnStart = true;

    [Header("Movement")]
    [SerializeField] private float _maxSpeed = 2.2f;
    [SerializeField] private float _acceleration = 25f;
    [SerializeField] private float _reachedPointDistance = 0.05f;

    private Rigidbody2D _rigidbody;
    private Vector2[] _routePoints;
    private bool _isActive;

    private int _targetIndex;
    private int _directionSign = 1;
    private int _facingSign = 1;

    public int FacingSign => _facingSign;

    public event Action<int> FacingChanged;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        if (_route == null)
            _route = GetComponentInChildren<PatrolRoute>();

        if (ValidateSettings() == false)
        {
            enabled = false;

            return;
        }

        if (TryCacheRoutePoints() == false)
        {
            enabled = false;

            return;
        }

        _targetIndex = 1;
        _directionSign = 1;

        UpdateFacingByTarget();
    }

    private void Start()
    {
        if (_startOnStart == false)
            return;

        Begin();
    }

    private void FixedUpdate()
    {
        if (_isActive == false)
            return;

        MoveAlongRoute();
    }

    public void Begin()
    {
        _isActive = true;
        UpdateFacingByTarget();
    }

    public void Stop()
    {
        _isActive = false;

        Vector2 currentVelocity = _rigidbody.velocity;
        currentVelocity.x = 0f;
        _rigidbody.velocity = currentVelocity;
    }

    public void GetResumeData(out int targetWaypointIndex, out int directionSign)
    {
        targetWaypointIndex = _targetIndex;
        directionSign = _directionSign;
    }

    public void SetResumeData(int targetWaypointIndex, int directionSign)
    {
        _targetIndex = Mathf.Clamp(targetWaypointIndex, 0, _route.WaypointCount - 1); ;
        _directionSign = directionSign >= 0 ? 1 : -1;

        UpdateFacingByTarget();
    }

    private bool ValidateSettings()
    {
        if (_route == null)
        {
            Debug.LogError($"{nameof(EnemyPatroller)} on {name}: PatrolRoute is not assigneed.", this);

            return false;
        }

        if (_route.WaypointCount < 2)
        {
            Debug.LogError($"{nameof(EnemyPatroller)} on {name}: PatrolRoute must have at least 2 waypoints.", this);

            return false;
        }

        if (_maxSpeed <= 0f)
        {
            Debug.LogError($"{nameof(EnemyPatroller)} on {name}: max speed must be greater than 0.", this);

            return false;
        }

        if (_acceleration <= 0f)
        {
            Debug.LogError($"{nameof(EnemyPatroller)} on {name}: acceleration must be greater than 0.", this);

            return false;
        }

        if (_reachedPointDistance <= 0f)
        {
            Debug.LogError($"{nameof(EnemyPatroller)} on {name}: reached point distance must be greater than 0.", this);

            return false;
        }

        return true;
    }

    private bool TryCacheRoutePoints()
    {
        _routePoints = new Vector2[_route.WaypointCount];

        for (int i = 0; i < _route.WaypointCount; i++)
        {
            Transform waypoint = _route.GetWaypoint(i);

            if (waypoint == null)
            {
                Debug.LogError($"{nameof(EnemyPatroller)} on {name}: waypoint at index {i} is not assigned.", this);

                return false;
            }

            _routePoints[i] = waypoint.position;
        }

        return true;
    }
    private void MoveAlongRoute()
    {
        Vector2 currentPosition = _rigidbody.position;
        Vector2 targetPosition = _routePoints[_targetIndex];

        float toTargetX = targetPosition.x - currentPosition.x;

        if (Mathf.Abs(toTargetX) <= _reachedPointDistance)
        {
            AdvanceTarget();

            targetPosition = _routePoints[_targetIndex];
            toTargetX = targetPosition.x - currentPosition.x;
        }

        UpdateFacingByDirection(toTargetX);

        float targetSpeedX = Mathf.Sign(toTargetX) * _maxSpeed;
        Vector2 currentVelocity = _rigidbody.velocity;

        currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, targetSpeedX, _acceleration * Time.fixedDeltaTime);
        _rigidbody.velocity = currentVelocity;
    }

    private void AdvanceTarget()
    {
        int nextIndex = _targetIndex + _directionSign;

        if (nextIndex < 0 || nextIndex >= _routePoints.Length)
        {
            _directionSign *= -1;
            nextIndex = _targetIndex + _directionSign;
        }

        _targetIndex = nextIndex;

        UpdateFacingByTarget();
    }

    private void UpdateFacingByTarget()
    {
        Vector2 currentPosition = _rigidbody.position;
        Vector2 targetPosition = _routePoints[_targetIndex];

        float toTargetX = targetPosition.x - currentPosition.x;
                
        UpdateFacingByDirection(toTargetX);
    }

    private void UpdateFacingByDirection(float directionX)
    {
        if (Mathf.Abs(directionX) < DirectionDeadZone)
            return;

        int newFacingSign = directionX > 0f ? 1 : -1;

        if (newFacingSign == _facingSign)
            return;

        _facingSign = newFacingSign;
        FacingChanged?.Invoke(_facingSign);
    }
}