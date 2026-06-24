using UnityEngine;
using UnityEngine.VFX;


public class PlayerVFXManager : MonoBehaviour
{
    public static PlayerVFXManager Instance { get; private set; }
    [SerializeField] private VisualEffect footStep;
    [SerializeField] private ParticleSystem Blade01;

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
}
