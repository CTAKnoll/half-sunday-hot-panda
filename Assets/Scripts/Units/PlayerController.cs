using Services;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, Damageable, IService
{
    public int MaxHealth;
    [DoNotSerialize] public int Health;
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
    private Vector2 MoveVector;

    private TemplateServer TemplateServer;
    private AudioService _audio;

    private IView<PlayerStatsModel> _view;
    private PlayerStatsModel _uiModel;

    [Header("Damage SFX IDs")]
    [Min(0)]
    public int damage_sfx = 0;
    [Min(0)]
    public int death_sfx = 1;

    [Header("Engine Sounds")]
    public AudioSourceTuner engineTuner;

    private void Awake()
    {
        ServiceLocator.RegisterAsService(this);
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        ServiceLocator.TryGetService(out TemplateServer);
        ServiceLocator.TryGetService(out _audio);
        ServiceLocator.TryGetService(out _view);
        
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
        var movement = Time.deltaTime * DeltaSpeed * new Vector3(MoveVector.x, 0, MoveVector.y).normalized;
        var engineSetpoint = movement.x > 0 ? 1 : -1;

        if(MoveVector != Vector2.zero)
            Controller.Move(movement);
        else
            engineSetpoint = 0;

        if (TryFire)
        {
            CurrentWeapon.TryFireWeapon(ProjectileSource, GetTargetFromMouse());
        }

        UpdateUIView();
        engineTuner.SetSetpoint(engineSetpoint);
    }

    private void UpdateUIView()
    {
        if (_view == null)
            return;

        _uiModel.Icon = CurrentWeapon.Data.icon;
        _uiModel.AmmoCount = CurrentWeapon.CurrentAmmo;
        _uiModel.MaxAmmo = CurrentWeapon.Data.MaxAmmo;
        _uiModel.Health = (float)Health/MaxHealth;
        _view.UpdateViewWithModel(_uiModel);
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
