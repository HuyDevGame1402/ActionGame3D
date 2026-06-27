using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    public float horizontalInput { get; private set; }
    public float verticalInput { get; private set; }

    public bool mouseButtonDown;
    public bool spaceKeyDown;

    private void Update()
    {
        if(!mouseButtonDown && Time.timeScale != 0)
        {
            mouseButtonDown = Input.GetMouseButtonDown(0);
        }

        if(!spaceKeyDown && Time.timeScale != 0)
        {
            spaceKeyDown = Input.GetKeyDown(KeyCode.Space);
        }

        horizontalInput = Input.GetAxisRaw(HORIZONTAL);
        verticalInput = Input.GetAxisRaw(VERTICAL);
    }

    private void OnDisable()
    {
        ClearCache();
    }
    public void ClearCache()
    {
        mouseButtonDown = false;
        spaceKeyDown = false;
        horizontalInput = 0;
        verticalInput = 0;
    }

}