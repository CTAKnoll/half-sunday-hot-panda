using Services;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsView : MonoBehaviour, IView<PlayerStatsModel>
{
    public Slider healthSlider;
    public TMP_Text currAmmoText;
    public TMP_Text maxAmmoText;


    public Image weaponIcon;

    public void UpdateViewWithModel(PlayerStatsModel model)
    {
        healthSlider.value = Mathf.Clamp01(model.Health);
        weaponIcon.sprite = model.Icon;

        if(model.isInfinite)
        {
            currAmmoText.text = "∞";
            maxAmmoText.text = "∞";
            return;
        }
        currAmmoText.text = model.AmmoCount.ToString();
        maxAmmoText.text = model.MaxAmmo.ToString();
    }

    private void Awake()
    {
        ServiceLocator.RegisterAsService(this as IView<PlayerStatsModel>);
    }
}

public struct PlayerStatsModel  
{
    public float Health;
    public int AmmoCount;
    public int MaxAmmo;

    public Sprite Icon;
    public bool isInfinite => MaxAmmo <= 0;
}
