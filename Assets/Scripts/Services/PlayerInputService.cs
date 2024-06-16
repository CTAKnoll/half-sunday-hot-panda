using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputService : MonoBehaviour, IService
{
    public PlayerInput PlayerInput;
    public InputActionReference PlayerMove;
    public InputActionReference PlayerShoot;

    public void Awake()
    {
        ServiceLocator.RegisterAsService(this);
    }
}
