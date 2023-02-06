using System.Collections.Generic;
using MainGameFramework;
using ReFactory.ParticleSystem.Particles;

namespace ParticleSystem.Particles;

public class ParticleSystem
{
    private readonly List<Particle> _particles = new ();

    public ParticleSystem()
    {
        MainGame.OnUpdate += Update;
    }
    
    ~ParticleSystem()
    {
        MainGame.OnUpdate -= Update;
    }
    
    public void AddParticle(Particle particle)
    {
        _particles.Add(particle);
    }

    public void Update(float deltaTime)
    {
        foreach (var particle in _particles)
        {
            particle.Update();
        }

        _particles.RemoveAll(p => p.isFinished);
    }
}