using UnityEngine;
using System.Collections;

public delegate void ParticlePlayEndDelegate();

public class ParticleController : MonoBehaviour {

    private ParticleSystem[] particleSystems;
    private ParticlePlayEndDelegate m_callback;

    void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    void Update()
    {
        bool allStopped = true;

        for (int i = 0;i < particleSystems.Length;i++)
        {
            ParticleSystem ps = particleSystems[i];
            if (!ps.isStopped)
            {
                allStopped = false;
            }
        }

        if (allStopped)
        {
            GameObject.Destroy(gameObject);
            if (m_callback != null)
                m_callback();
        }
    }

    public void SetParticleCallback(ParticlePlayEndDelegate callback)
    {
        m_callback = callback;
    }
}
