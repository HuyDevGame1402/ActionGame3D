using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float speed = 80f;
    private Vector3 rotate = Vector3.zero;
    
    private void Update()
    {
        rotate.y = Time.deltaTime * speed;
        transform.Rotate(rotate, Space.World);
    }
}
