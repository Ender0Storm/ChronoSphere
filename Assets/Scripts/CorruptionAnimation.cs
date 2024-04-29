using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionAnimation : MonoBehaviour
{
    [SerializeField]
    private float lerpTime;

    private ParticleSystem[] particles;
    private MeshRenderer[] objectMeshes;
    private float animationLerp = 0;
    private bool triggered = false;

    void Start() {
        particles = GetComponentsInChildren<ParticleSystem>();
        objectMeshes = GetComponentsInChildren<MeshRenderer>();
    }

    void Update() {
        if (animationLerp > 0)
        {
            float progress = animationLerp / lerpTime;
            foreach (ParticleSystem particle in particles)
            {
                var psMain = particle.main;
                var currentColor = psMain.startColor.color;
                var newColor = new Color(currentColor.r, currentColor.g, currentColor.b, progress);
                psMain.startColor = new ParticleSystem.MinMaxGradient(newColor);
            }

            foreach (MeshRenderer mesh in objectMeshes)
            {
                mesh.material.SetFloat("_ShaderLerp", progress);
            }

            animationLerp -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !triggered)
        {
            animationLerp = lerpTime;
            triggered = true;
        }
    }
}
