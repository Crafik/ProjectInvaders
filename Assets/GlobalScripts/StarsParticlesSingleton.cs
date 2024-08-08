using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsParticlesSingleton : MonoBehaviour
{
    public static StarsParticlesSingleton Instance { get; private set; }

    [Header ("===== Components =====")]
    [SerializeField] private ParticleSystem m_particles;

    void Awake(){
        if (Instance != null && Instance != this){
            Destroy(Instance);
        }
        else{
            Instance = this;
        }
    }

    private bool isChangingSpeed = false;
    private bool isAccelerating;
    private float targetSpeed;
    private float curSpeed = 30f;
    public void SetSpeed(float speed){
        targetSpeed = speed;
        isAccelerating = targetSpeed > curSpeed;
        isChangingSpeed = true;
    }

    void Update(){
        if (GameManagerSingleton.Instance.isGameActive){
            if (m_particles.isPaused){
                m_particles.Play(); // this may not work as i intent it to, need investigation
            }
            if (isChangingSpeed){
                // Hope it wont break on me sometime in the future
                ParticleSystem.Particle[] existingParticles = new ParticleSystem.Particle[m_particles.particleCount];
                m_particles.GetParticles(existingParticles);
                var main = m_particles.main;
                if (isAccelerating){
                    if (curSpeed < targetSpeed){
                        curSpeed += Time.deltaTime * 10f;
                    }
                    else{
                        isChangingSpeed = false;
                    }
                }
                else{
                    if (curSpeed > targetSpeed){
                        curSpeed -= Time.deltaTime * 10f;
                    }
                    else{
                        isChangingSpeed = false;
                    }
                }
                main.startSpeed = curSpeed;
                var emission = m_particles.emission;
                emission.rateOverTime = curSpeed / 2;
                for (int i = 0; i < existingParticles.Length; ++i){
                    existingParticles[i].velocity = existingParticles[i].velocity.normalized * curSpeed;
                }
                m_particles.SetParticles(existingParticles, existingParticles.Length);
            }
        }
        else{
            if (!m_particles.isPaused){
                m_particles.Pause();
            }
        }
    }
}
