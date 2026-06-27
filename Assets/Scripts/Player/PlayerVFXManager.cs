using UnityEngine;
using UnityEngine.VFX;


public class PlayerVFXManager : MonoBehaviour
{
    public static PlayerVFXManager Instance { get; private set; }
    [SerializeField] private VisualEffect footStep;
    [SerializeField] private ParticleSystem Blade01;
    [SerializeField] private ParticleSystem Blade02;
    [SerializeField] private ParticleSystem Blade03;
    [SerializeField] private VisualEffect slash;
    [SerializeField] private VisualEffect healthVFX;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateFootStep(bool state)
    {
        if (state)
        {
            footStep.Play();
        }
        else
        {
            footStep.Stop();
        }
    }
    public void PlayBlade01()
    {
        Blade01.Play();
    }
    public void PlayBlade02()
    {
        Blade02.Play();
    }
    public void PlayBlade03()
    {
        Blade03.Play();
    }
    public void StopBlade()
    {
        Blade01.Simulate(0);
        Blade01.Stop();
        Blade02.Simulate(0);
        Blade02.Stop();
        Blade03.Simulate(0);
        Blade03.Stop();

    }
    public void PlaySlash(Vector3 pos)
    {
        slash.transform.position = pos;
        slash.Play();
    }
    public void PlayHealVFX()
    {
        healthVFX.Play();
    }
}
