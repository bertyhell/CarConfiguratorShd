using System;
using UnityEngine;

public enum AxisDirection {
    x,
    y,
    z
}

public class FlipOpen : MonoBehaviour {
    [SerializeField] AxisDirection axis = AxisDirection.x;
    [SerializeField] GameObject enterViewpointTrigger = null;
    [SerializeField] float closedAngle = 270f;
    [SerializeField] float openAngle = 310f;
    [SerializeField] float animationDuration = 0.5f;

    private Vector3 initialRotation;

    private AnimateInAndOut animator = null;

    void Start() {
        animator = new AnimateInAndOut(new Transform[] { transform }, animationDuration);
        animator.OnEntered += OnEntered;
        animator.OnExiting += OnExiting;
        animator.OnAnimate += OnAnimate;
        
       initialRotation = transform.rotation.eulerAngles;
    }

    void OnEntered(object sender,  EventArgs e) {
        if (enterViewpointTrigger != null) {
            enterViewpointTrigger.SetActive(true);
        }
    }

    void OnExiting(object sender, EventArgs e) {
        if (enterViewpointTrigger != null) {
            enterViewpointTrigger.SetActive(false);
        }
    }

    void OnAnimate(object sender, OnAnimateEventArgs e) {
        if (enterViewpointTrigger != null) {
            enterViewpointTrigger.SetActive(false);
        }

        float rotationAngle = Mathf.Lerp(
            e.IsEntering? closedAngle : openAngle,
            e.IsEntering? openAngle : closedAngle,
            e.TimeFactor);

        SetRotation(rotationAngle, axis);
    }

    void Update() {
        animator.Update();
    }

    void SetRotation(float angle, AxisDirection axis) {
        switch (axis) {
            case AxisDirection.x:
                transform.rotation = Quaternion.Euler(angle, initialRotation.y, initialRotation.z);
                break;

            case AxisDirection.y:
                transform.rotation = Quaternion.Euler(initialRotation.x, angle, initialRotation.z);
                break;

            case AxisDirection.z:
                transform.rotation = Quaternion.Euler(initialRotation.x, initialRotation.z, angle);
                break;

            default:
                break;
        }
    }
}
