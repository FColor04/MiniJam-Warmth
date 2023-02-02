using CanvasManagement;
using MainGameFramework;
using ParticleSystemTesting;
using ReFactory;
using ReFactory.ParticleSystem.Particles;

namespace ParticleSystem.Particles;

public static class ParticleTester
{
    private static ParticleData ParticleData = new ();
    private static ParticleSystem _particleSystem;
    
    static ParticleTester()
    {
        _particleSystem = new ParticleSystem();
        MainGame.OnUpdate += Update;
    }

    private static void Update(float deltaTime)
    {
        if(Input.LeftMousePressed)
            _particleSystem.AddParticle(new Particle(CanvasLayer.Base.GetCanvas().MousePosition, ParticleData));
    }
}