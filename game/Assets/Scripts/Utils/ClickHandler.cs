using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public delegate void OnClick();

public class ClickHandler {

    private Vector2 Button0DownPoint;
    private float ClickMargin = 30f;

    private List<Transform> targets = new List<Transform>();
    private OnClick onClick = null;


    public ClickHandler(Transform[] targets, OnClick onClick) {
        this.targets.AddRange(targets);
        this.onClick = onClick;
    }

    public void Update() {
        if (Input.GetButtonDown("MouseButton1")) {
            Button0DownPoint = Input.mousePosition;
        }

        if (Input.GetButtonUp("MouseButton1")) {
            if (Helpers.IsInRange(Button0DownPoint, Input.mousePosition, ClickMargin)) {
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo);
                if (targets.Any(target => target.transform == hitInfo.transform)) {
                    onClick();
                }
            }
        }
    }
}

