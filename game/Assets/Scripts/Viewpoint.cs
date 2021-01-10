using System;
using UnityEngine;

public class Viewpoint : MonoBehaviour {
    [SerializeField] GameObject enterViewpointTrigger = null;
    [SerializeField] GameObject exitViewpointTrigger = null;
    [SerializeField] float animationDuration = 2f;

    [SerializeField] float horRotationSpeed = 150f;
    [SerializeField] float vertRotationSpeed = 100f;

    private Vector3 initialCameraPosition;
    private Vector3 initialCameraRotation;

    private CameraMovement cameraMovement = null;

    private float rotationX = 0;
    private float rotationY = 0;
    private float rotationZ = 0;

    private AnimateInAndOut animator = null;

    private bool isAtViewpoint = false;

    void Start() {
        animator = new AnimateInAndOut(
            new Transform[] { enterViewpointTrigger.transform, exitViewpointTrigger.transform }, 
            animationDuration
        );
        animator.OnShouldAnimate += ShouldAnimate;
        animator.OnEntering += OnEntering;
        animator.OnEntered += OnEntered;
        animator.OnExiting += OnExiting;
        animator.OnExited += OnExited;
        animator.OnAnimate += OnAnimate;

        cameraMovement = GameObject.FindGameObjectWithTag("CameraRig").GetComponent<CameraMovement>();
    }

    void ShouldAnimate(object sender, ShouldAnimateEventArgs e) {
        e.ShouldAnimate = 
            GlobalState.ActiveViewpoint == null || 
            transform == GlobalState.ActiveViewpoint.transform;
    }

    void OnEntering(object sender, EventArgs e) {
        GlobalState.ActiveViewpoint = gameObject;
        cameraMovement.SendMessage("SetEnabled", false);

        initialCameraPosition = Camera.main.transform.position;
        initialCameraRotation = Camera.main.transform.rotation.eulerAngles;

        rotationX = transform.rotation.eulerAngles.x;
        rotationY = transform.rotation.eulerAngles.y;
        rotationZ = transform.rotation.eulerAngles.z;
    }

    void OnEntered(object sender, EventArgs e) {
        enterViewpointTrigger.SetActive(false);
        exitViewpointTrigger.SetActive(true);

        isAtViewpoint = true;
    }

    void OnExiting(object sender, EventArgs e) {
        exitViewpointTrigger.SetActive(false);

        isAtViewpoint = false;
    }

    void OnExited(object sender, EventArgs e) {
        enterViewpointTrigger.SetActive(true);
        cameraMovement.SendMessage("SetEnabled", true);
        GlobalState.ActiveViewpoint = null;
    }

    void OnAnimate(object sender, OnAnimateEventArgs e) {
        Vector3 newPosition = Vector3.Lerp(
            e.IsEntering ? initialCameraPosition : transform.position,
            e.IsEntering ? transform.position : initialCameraPosition,
            e.TimeFactor
        );
        Vector3 newRotation = Vector3.Lerp(
            e.IsEntering ? initialCameraRotation : new Vector3(rotationX, rotationY, rotationZ),
            e.IsEntering ? new Vector3(rotationX, rotationY, rotationZ) : initialCameraRotation,
            e.TimeFactor
        );

        Camera.main.transform.position = newPosition;
        Camera.main.transform.rotation = Quaternion.Euler(newRotation);
    }

    void Update() {
        animator.Update();

        // Once we're in the viewpoint location, we can move the camera with the mouse
        if (isAtViewpoint && Input.GetButton("MouseButton1")) {
            //CLAMPS
            float mouseDeltaX = Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1);
            float mouseDeltaY = Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1);
            float mouseX = mouseDeltaX * horRotationSpeed * Time.deltaTime;
            float mouseY = mouseDeltaY * vertRotationSpeed * Time.deltaTime;

            rotationX += mouseY;
            rotationY -= mouseX;

            Camera.main.transform.rotation = Quaternion.Euler(
                rotationX,
                rotationY,
                rotationZ
            );
        }
    }
}
