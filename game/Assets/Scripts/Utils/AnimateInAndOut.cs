using System;
using UnityEngine;

public delegate void OnEvent();
public delegate bool ShouldExecute();
public delegate void OnAnimate(float timeFactor, bool entering);

class AnimateInAndOut {
    public event EventHandler<ShouldAnimateEventArgs> OnShouldAnimate;
    public event EventHandler OnEntering;
    public event EventHandler OnEntered;
    public event EventHandler OnExiting;
    public event EventHandler OnExited;
    public event EventHandler<OnAnimateEventArgs> OnAnimate;

    private bool isInside = false;
    private bool Animating = false;
    private float TimeElapsed = 0f;

    private float duration;

    private ClickHandler clickHandler = null;

    public AnimateInAndOut(
        Transform[] targets,
        float duration
    ) {
        clickHandler = new ClickHandler(targets, OnClick);
        this.duration = duration;
    }

    void OnClick() {
        var eventArgs = new ShouldAnimateEventArgs();
        OnShouldAnimate?.Invoke(this, eventArgs);
        if (eventArgs.ShouldAnimate) {
            // Start animating
            Animating = true;
            TimeElapsed = 0;

            if (isInside && OnExiting != null) {
                OnExiting(this, EventArgs.Empty);
            }
            if (!isInside && OnEntering != null) {
                OnEntering(this, EventArgs.Empty);
            }
        }
    }

    public void Update() {
        clickHandler.Update();

        var eventArgs = new OnAnimateEventArgs {
            TimeFactor = TimeElapsed / duration,
            IsEntering = !isInside
        };
        if (Animating && OnAnimate != null) {
            OnAnimate(this, eventArgs);

            TimeElapsed += Time.deltaTime;

            if (TimeElapsed > duration) {
                // End animation
                Animating = false;
                isInside = !isInside;

                if (isInside && OnEntered != null) {
                    OnEntered(this, EventArgs.Empty);
                } 
                if (!isInside && OnExited != null) {
                    OnExited(this, EventArgs.Empty);
                }
            }
        }
    }
}

public class OnAnimateEventArgs : EventArgs {
    public float TimeFactor { get; set; }
    public bool IsEntering { get; set; }
}

public class ShouldAnimateEventArgs : EventArgs {
    public bool ShouldAnimate { get; set; } = true;
}