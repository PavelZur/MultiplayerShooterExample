using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance { get; private set; }

    public float AxisRawHorizontal => Input.GetAxisRaw("Horizontal");
    public float AxisRawVertical => Input.GetAxisRaw("Vertical");
    public float MouseAxisY => Input.GetAxis("Mouse Y");
    public float MouseAxisX => Input.GetAxis("Mouse X");
    public bool JumpKeyPressed => Input.GetKeyDown(KeyCode.Space);
    public bool MouseButtonDown0 => Input.GetMouseButtonDown(0);


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
