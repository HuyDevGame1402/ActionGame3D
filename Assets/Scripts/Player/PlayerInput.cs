using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    public float horizontalInput { get; private set; }
    public float verticalInput { get; private set; }

    public bool mouseButtonDown;

    private void Update()
    {

        if(!mouseButtonDown && Time.timeScale != 0)
        {
            mouseButtonDown = Input.GetMouseButtonDown(0);
        }

        horizontalInput = Input.GetAxisRaw(HORIZONTAL);
        verticalInput = Input.GetAxisRaw(VERTICAL);
    }

    private void OnDisable()
    {
        mouseButtonDown = false;
        horizontalInput = 0;
        verticalInput = 0;
    }
}