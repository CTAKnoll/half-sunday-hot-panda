using Services;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, Damageable, IService
{
    public int MaxHealth;
    public float DeltaSpeed;
    public CharacterController Controller;
    [SerializeField] private HurtboxType _hurtboxType;
    public HurtboxType HurtboxType => _hurtboxType;

    public Weapon CurrentWeapon;
    public GameObject ProjectileSource;

    public InputActionReference PlayerMoveUpDown;
    public InputActionReference PlayerMoveLeftRight;
    public InputActionReference PlayerFire;

    private bool TryFire;
    private int Health;
    private Vector2 MoveVector;

    private TemplateServer TemplateServer;
    private AudioService _audio;

    [Header("Damage SFX IDs")]
    [Min(0)]
    public int damage_sfx = 0;
    [Min(0)]
    public int death_sfx = 1;

    private void Awake()
    {
        ServiceLocator.RegisterAsService(this);
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        ServiceLocator.TryGetService(out TemplateServer);
        ServiceLocator.TryGetService(out _audio);
        
        Health = MaxHealth;
        CurrentWeapon = new Weapon(TemplateServer.PistolTemplate, gameObject);
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
            Controller.Move(Time.deltaTime * DeltaSpeed * new Vector3(MoveVector.x, 0, 
                MoveVector.y).normalized);
        if(TryFire)
            CurrentWeapon.TryFireWeapon(ProjectileSource, GetTargetFromMouse());
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
        return new Vector3(projection.x, 0, projection.z);
    }

    void Damageable.Damage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            _audio.PlaySound(Damageable.DAMAGE_SFX_BANK, death_sfx);
            Destroy(gameObject);
            // You died loser lol get rekt
            return;
        }

        _audio.PlaySound(Damageable.DAMAGE_SFX_BANK, damage_sfx);
    }
}
