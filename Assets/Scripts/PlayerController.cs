using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float DeltaSpeed;
    public CharacterController Controller;

    public InputActionReference PlayerMoveUpDown;
    public InputActionReference PlayerMoveLeftRight;
    public InputActionReference PlayerFire;

    private Vector2 MoveVector;
    
    // Start is called before the first frame update
    private void Start()
    {
        PlayerMoveUpDown.action.performed += (cxt) => MoveVector.y = cxt.ReadValue<Vector2>().y;
        PlayerMoveUpDown.action.canceled += (_) => MoveVector.y = 0;
        PlayerMoveLeftRight.action.performed += (cxt) => MoveVector.x = cxt.ReadValue<Vector2>().x;
        PlayerMoveLeftRight.action.canceled += (_) => MoveVector.x = 0;
        PlayerFire.action.performed += OnFire;
    }

    private void Update()
    {
        if(MoveVector != Vector2.zero)
            Controller.Move(new Vector3(MoveVector.x * Time.deltaTime * DeltaSpeed, 0, 
                MoveVector.y * Time.deltaTime * DeltaSpeed));
    }
    
    public void OnFire(InputAction.CallbackContext context)
    {
        Debug.Log("HI");
    }
}
