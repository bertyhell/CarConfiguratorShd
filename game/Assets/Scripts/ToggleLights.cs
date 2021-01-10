using UnityEngine;
using System.Collections.Generic;

public class ToggleLights : MonoBehaviour {
    [SerializeField] GameObject[] LightGameObjects = null;

    [SerializeField] float OffIntensity = 0f;
    [SerializeField] float OnIntensity = 13f;
    [SerializeField] float animationDuration = 0.2f;

    private List<Light> Lights = new List<Light>();

    private AnimateInAndOut animator = null;

    void Start() {
        animator = new AnimateInAndOut(new Transform[] { transform }, animationDuration);
        animator.OnAnimate += OnAnimate;

        foreach (GameObject lightGameObject in LightGameObjects) {
            Lights.Add(lightGameObject.GetComponent<Light>());
        }
    }

    void OnAnimate(object sender, OnAnimateEventArgs e) {
        float intensity = Mathf.Lerp(
            e.IsEntering ? OffIntensity : OnIntensity,
            e.IsEntering ? OnIntensity : OffIntensity,
            e.TimeFactor
        );

        foreach (Light light in Lights) {
            light.intensity = intensity;
        }
    }

    void Update() {
        animator.Update();
    }
}
