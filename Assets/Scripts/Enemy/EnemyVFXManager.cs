using UnityEngine;
using UnityEngine.VFX;

public class EnemyVFXManager : MonoBehaviour
{
    public VisualEffect footStep;
    public VisualEffect attackVFX;

    public void BurstFootStep()
    {
        footStep.SendEvent("OnPlay");
    }
    public void PlayAttackVFX()
    {
        attackVFX.SendEvent("OnPlay");
    }
}
