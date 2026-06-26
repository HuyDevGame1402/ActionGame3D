using UnityEngine;
using UnityEngine.VFX;

public class EnemyVFXManager : MonoBehaviour
{
    public VisualEffect footStep;
    public VisualEffect attackVFX;
    public ParticleSystem beingHitVFX;
    public VisualEffect beingHitSplashVFX;

    public void BurstFootStep()
    {
        footStep.SendEvent("OnPlay");
    }
    public void PlayAttackVFX()
    {
        attackVFX.SendEvent("OnPlay");
    }
    public void PlayBeingHitVFX(Vector3 attackerPos)
    {
        Vector3 forceForward = transform.position - attackerPos;
        forceForward.Normalize();
        forceForward.y = 0;
        beingHitVFX.transform.rotation = Quaternion.LookRotation(forceForward);
        beingHitVFX.Play();

        // chỉnh ở ngoài pos + thêm 2
        Vector3 splashPos = transform.position;
        splashPos.y += 2;
        VisualEffect newSpalshVFX = Instantiate(beingHitSplashVFX, splashPos, Quaternion.identity) as VisualEffect;
        newSpalshVFX.SendEvent("OnPlay");
        Destroy(newSpalshVFX.gameObject, 10f);
    }
}
