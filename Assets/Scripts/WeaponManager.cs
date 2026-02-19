using System;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapons[] guns;     
    public int currentIndex = 0;

    public PlayerShooting shooter;
    public WeaponIK ik;
     public bool Bot;
    public static event Action<Sprite> OnEquip;

    void Start()
    {  if(Bot)
        {
            Equip(0);
        }
        else{
        Equip(GameData.selectedWeapon); 
        }
    }

   public void Equip(int index)
    {
        currentIndex = index;

       
        Weapons newWeapon = guns[index];
        newWeapon.gameObject.SetActive(true);
        if(!Bot)
        {
        OnEquip?.Invoke(newWeapon.Weaponicon);
        }
      
        if(shooter!=null)
        {
        shooter.currentWeapon = newWeapon;
        }

      
        ik.leftHandTarget = newWeapon.leftHandIK;
    }
}

