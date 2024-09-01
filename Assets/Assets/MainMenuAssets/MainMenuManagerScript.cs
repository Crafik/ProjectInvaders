using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManagerScript : MonoBehaviour
{
    [Header ("===== References =====")]
    [SerializeField] private ParticleSystem m_particles;
    [SerializeField] private GameObject moon;

    void Start(){
        ParticleSystem.Particle[] existingParticles = new ParticleSystem.Particle[m_particles.particleCount];
        m_particles.GetParticles(existingParticles);
        var v_emission = m_particles.emission;
        v_emission.enabled = false;
        for (int i = 0; i < existingParticles.Length; ++i){
            existingParticles[i].velocity = Vector3.zero;
        }
    }

    void Update(){
        moon.transform.RotateAround(moon.transform.position, moon.transform.up, 15f * Time.deltaTime);
    }
}
