using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public GameObject gateVisual;
    private Collider gateCollider;
    public float openDuration = 2f;
    public float openTargetY = -1.5f;

    private void Awake()
    {
        gateCollider = GetComponent<Collider>();
    }

    private IEnumerator OpenGateAnimation()
    {
        float currentOpenDuration = 0f;
        Vector3 startPos = gateVisual.transform.position;
        Vector3 targetPos = startPos + Vector3.up * openTargetY;

        while (currentOpenDuration < openDuration)
        {
            currentOpenDuration += Time.deltaTime;
            gateVisual.transform.position = Vector3.Lerp(startPos, targetPos, currentOpenDuration / openDuration);
            yield return null;
        }

        gateCollider.enabled = false;
    }

    public void Open()
    {
        StartCoroutine(OpenGateAnimation());
    }
}
