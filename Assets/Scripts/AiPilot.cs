using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using static GameController;

public class AiPilot : MonoBehaviour
{
    NavMeshAgent _myNavMeshAgent;
    NavMeshPath _navMeshPath;
    readonly List<Vector3> _pathPoints = new();
    Vector3 _targetPosition;
    int _actualPathPointIndex = -1;
    State _state;
    Behaviour _behaviour;
    [HideInInspector]
    public Transform tr;
    Tank _tank;
    Vector3 _toPathPointV3;
    LineRenderer _lineRenderer;
    bool _needsNewTarget;

    enum Behaviour
    {
        RandomRoam
    }

    public enum State
    {
        StandingStill,
        GoingToTarget
    }

    void Awake()
    {
        _tank = GetComponent<Tank>();
        Enemies.Add(_tank);
        tr = transform;
        _navMeshPath = new();
        _lineRenderer = GetComponent<LineRenderer>();
        GetRandomTarget();

        StartCoroutine(LazyUpdateLoop());
    }

    bool GetRandomTarget()
    {
        float a = 100;
        _targetPosition = new(Random.Range(- a, a), 0, Random.Range(- a, a));

        GeneratePathTo(_targetPosition);

        if (!GeneratePathTo(_targetPosition))
        {
            print("New random target NOT got.");
            SetState(State.StandingStill);
            _needsNewTarget = true;
            return false;
        }

        SetState(State.GoingToTarget);
        return true;
    }

    void Update()
    {
        UpdateTransform();
    }

    bool GeneratePathTo(Vector3 targetLocation)
    {
        // var result = NavMesh.CalculatePath(new(tr.position.x, 0, tr.position.z), targetLocation, NavMesh.AllAreas, _navMeshPath);
        var result = NavMesh.CalculatePath(transform.position, targetLocation, NavMesh.AllAreas, _navMeshPath);

        if (!result)
        {
            if (!result)
            {
                Debug.LogWarning("Path was NOT generated!");

                // if (_state == State.GoingToTarget)
                // {
                //         SetState(State.StandingStill);
                //         _needsNewTarget = true;
                //         Debug.LogWarning("TODO: To bude chtít ošetřit kvalitněji. Optimálně by k tomu mohle ndojít :).");  // TODO
                // }
            }

            return false;
        }

        _pathPoints.Clear();
        _pathPoints.AddRange(_navMeshPath.corners);
        _actualPathPointIndex = 1;  // index = 0 is at actual agent position

        // ShowPath();

        return true;
    }

    bool ReGeneratePath()
    {
        return GeneratePathTo(_targetPosition);
    }

    public void SetState(State newState)
    {
        _state = newState;
    }

    void CheckGoingToTargetState()
    {
        if (IsPathPointReached())
        {
            _actualPathPointIndex++;
            
            if (IsTargetReached())
            {
                SetState(State.StandingStill);
                GetRandomTarget();
            }
        }
    }

    bool IsPathPointReached()
    {
        return _toPathPointV3.sqrMagnitude < 10;
    }

    bool IsTargetReached()
    {
        return _actualPathPointIndex == _pathPoints.Count;
    }

    Vector3 GetActualPathPoint()
    {
        return _pathPoints[_actualPathPointIndex];
    }

    void UpdateTransform()
    {
        if (_state == State.StandingStill)
        {
            _tank.controlsForward = 0;
            _tank.controlsLeftRight = 0;
        }
        else if (_state == State.GoingToTarget)
        {
            _toPathPointV3 = GetActualPathPoint() - tr.position;
            _tank.controlsForward = 1;

            // TODO: stačí .DOT
            var angle = Vector3.SignedAngle(_toPathPointV3, transform.forward, Vector3.up);
        
            if (angle < - 2)
            {
                _tank.controlsLeftRight = 1;
                // _tank.controlsForward = 1;
            }
            else if (angle > 2)
            {
                _tank.controlsLeftRight = - 1;
                // _tank.controlsForward = 1;
            }
            else
            {
                _tank.controlsLeftRight = 0;
                // _tank.controlsForward = 0;
            }

            // var dot = Vector3.Dot(_toPathPointV3, transform.forward);
            //
            // if (dot < 85)
            //     _tank.controlsLeftRight = - 1;
            // else if (dot > 95)
            //     _tank.controlsLeftRight = 1;
            // else
            //     _tank.controlsLeftRight = 0;
            // print("dot: " + dot);
            
            CheckGoingToTargetState();
        }
    }

    void ShowPath()
    {
        _lineRenderer.positionCount = _navMeshPath.corners.Length;
        _lineRenderer.SetPositions(_navMeshPath.corners);

        // foreach (var point in _navMeshPath.corners)
        // {
        //     _pointVisualizer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //     DestroyImmediate(_pointVisualizer.GetComponent<BoxCollider>());
        //     _pointVisualizer.transform.position = point;
        //     _pointVisualizer.name = $"{name}_path_point";
        // }
    }

    IEnumerator LazyUpdateLoop()
    {
        for(;;)
        {
            yield return new WaitForSeconds(.5f);
            ProcessLazyUpdate();
        }
    }

    void ProcessLazyUpdate()
    {
        if (_needsNewTarget)
        {
            print("regenerating path");
            if (GetRandomTarget())
            {
                print("OK");
                _needsNewTarget = false;
                SetState(State.GoingToTarget);
            }
        }
    }
}
