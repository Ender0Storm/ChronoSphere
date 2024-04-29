using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionAnimation : MonoBehaviour
{
    [SerializeField]
    private List<ParticleSystem> particles;
    [SerializeField]
    private List<MeshRenderer> objectMeshes;
    [SerializeField]
    private float lerpTime;

    private float animationLerp = 0;
    private bool triggered = false;

    void Update() {
        if (animationLerp > 0)
        {
            float progress = animationLerp / lerpTime;

            foreach (ParticleSystem particle in particles)
            {
                var psMain = particle.main;
                var currentColor = psMain.startColor.color;

                psMain.startColor = new Color(currentColor.r, currentColor.g, currentColor.b, progress);
            }

            foreach (MeshRenderer mesh in objectMeshes)
            {
                mesh.material.SetFloat("ShaderLerp", progress);
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
