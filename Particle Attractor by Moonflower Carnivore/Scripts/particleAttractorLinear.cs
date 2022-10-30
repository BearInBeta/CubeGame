using System.Collections;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class particleAttractorLinear : MonoBehaviour {
	ParticleSystem ps;
	ParticleSystem.Particle[] m_Particles;
	Transform target;
    [SerializeField] float minDis;
	public float speed = 5f;
	int numParticlesAlive;
	void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
		ps = GetComponent<ParticleSystem>();
		if (!GetComponent<Transform>()){
			GetComponent<Transform>();
		}
	}
	void Update () {
        if (target == null)
        {
            return;
        }
        if (Mathf.Sqrt((Mathf.Pow(target.position.x - transform.position.x, 2)) + (Mathf.Pow(target.position.y - transform.position.y, 2))) < minDis)
        {
            m_Particles = new ParticleSystem.Particle[ps.main.maxParticles];
            numParticlesAlive = ps.GetParticles(m_Particles);
            float step = speed * Time.deltaTime;
            for (int i = 0; i < numParticlesAlive; i++)
            {
                m_Particles[i].position = Vector3.LerpUnclamped(m_Particles[i].position, target.position, step);
            }
            ps.SetParticles(m_Particles, numParticlesAlive);
        }
	}
}
