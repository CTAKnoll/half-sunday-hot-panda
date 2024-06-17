using Services;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, Damageable
{
    public int MaxHealth;
    public float DeltaSpeed;
    public CharacterController Controller;
    public Weapon CurrentWeapon;

    public InputActionReference PlayerMoveUpDown;
    public InputActionReference PlayerMoveLeftRight;
    public InputActionReference PlayerFire;

    private bool TryFire;
    private int Health;
    private Vector2 MoveVector;

    private TemplateServer TemplateServer;
    
    // Start is called before the first frame update
    private void Start()
    {
        Health = MaxHealth;
        PlayerMoveUpDown.action.performed += (cxt) => MoveVector.y = cxt.ReadValue<Vector2>().y;
        PlayerMoveUpDown.action.canceled += (_) => MoveVector.y = 0;
        PlayerMoveLeftRight.action.performed += (cxt) => MoveVector.x = cxt.ReadValue<Vector2>().x;
        PlayerMoveLeftRight.action.canceled += (_) => MoveVector.x = 0;
        PlayerFire.action.started += (_) => TryFire = true;
        PlayerFire.action.canceled += (_) => TryFire = false;
    }

    private void Update()
    {
        if(MoveVector != Vector2.zero)
            Controller.Move(new Vector3(MoveVector.x * Time.deltaTime * DeltaSpeed, 0, 
                MoveVector.y * Time.deltaTime * DeltaSpeed));
        if(TryFire)
            CurrentWeapon.TryFireWeapon(gameObject, GetTargetFromMouse());
    }

    public void AcquireWeapon(Weapon weapon)
    {
        CurrentWeapon = weapon;
        CurrentWeapon.OnWeaponEmpty += ReEquipPistol;
    }

    private void ReEquipPistol()
    {
        if (TemplateServer == null)
            ServiceLocator.TryGetService(out TemplateServer);
        CurrentWeapon = new Weapon(TemplateServer.PistolTemplate);
    }
    
    private Vector3 GetTargetFromMouse()
    {
        // create a ray from the camera towards the mouse, get the first thing it hits
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
        // project onto the XZ plane
        Vector3 projection = Vector3.ProjectOnPlane(hit.point, Vector3.up);
        // increase Y to meet the car
        return new Vector3(projection.x, gameObject.transform.position.y, projection.z);
    }

    void Damageable.Damage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            // You died loser lol get rekt
        }
    }
}
