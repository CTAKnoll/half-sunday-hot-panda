using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController Controller;
    
    // Start is called before the first frame update
    private void Start()
    {
        Controller = gameObject.AddComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("HI");
        var moveVector = context.ReadValue<Vector2>();
        Controller.Move(new Vector3(moveVector.x, 0, moveVector.y));
    }
}
