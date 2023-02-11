using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class SwipeDetection : MonoBehaviour
{
    private InputManager inputManager;

    public UnityEvent OnSwipeLeft;
    public UnityEvent OnSwipeRight;
    public UnityEvent OnSwipeUp;
    public UnityEvent OnSwipeDown;
    
    [SerializeField] private float minimumDistance = 0.2f;
    [SerializeField] private float maximumTime = 1f;
    [SerializeField, Range(0f,1f)] private float directionThreshold = 0.9f;

    private Vector2 startPosition;
    private float startTime;
    
    private Vector2 endPosition;
    private float endTime;
    private void Awake()
    {
        inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
    }
    
    
    private void SwipeEnd(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Vector3.Distance(startPosition,endPosition) >= minimumDistance && (endTime - startTime) <= maximumTime)
        {
            var direction = endPosition - startPosition;
            var direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        if (Vector2.Dot(Vector2.up,direction) > directionThreshold)
        {
            OnSwipeUp.Invoke();
        }
        
        else if (Vector2.Dot(Vector2.down,direction) > directionThreshold)
        {
            OnSwipeDown.Invoke();
        }
        else if (Vector2.Dot(Vector2.left,direction) > directionThreshold)
        {
            OnSwipeLeft.Invoke();
        }
        else if (Vector2.Dot(Vector2.right,direction) > directionThreshold)
        {
            OnSwipeRight.Invoke();
        }
    }
}
