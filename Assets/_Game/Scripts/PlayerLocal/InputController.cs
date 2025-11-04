using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance { get; private set; }

    private bool IsCursorUnlocked => Cursor.lockState == CursorLockMode.None;

    public float AxisRawHorizontal => IsCursorUnlocked ? 0f : Input.GetAxisRaw("Horizontal");
    public float AxisRawVertical => IsCursorUnlocked ? 0f : Input.GetAxisRaw("Vertical");
    public float MouseAxisY => IsCursorUnlocked ? 0f : Input.GetAxis("Mouse Y");
    public float MouseAxisX => IsCursorUnlocked ? 0f : Input.GetAxis("Mouse X");

    public bool JumpKeyPressed => IsCursorUnlocked ? false : Input.GetKeyDown(KeyCode.Space);
    public bool MouseButtonDown0 => IsCursorUnlocked ? false : Input.GetMouseButtonDown(0);
    public bool SittingKeyPressed => IsCursorUnlocked ? false : Input.GetKey(KeyCode.LeftControl);
    public bool MouseLeftButtonDown => IsCursorUnlocked ? false : Input.GetMouseButtonDown(0);
    public bool MouseLeftButtonPressed => IsCursorUnlocked ? false : Input.GetMouseButton(0);
    public bool MouseLeftButtonUp => IsCursorUnlocked ? false : Input.GetMouseButtonUp(0);

    public bool ReloadWeaponKeyPressed => IsCursorUnlocked ? false : Input.GetKeyDown(KeyCode.R);


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
