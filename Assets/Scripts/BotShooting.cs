using UnityEngine;

public class BotShooting : MonoBehaviour
{
    public Weapons currentWeapon;
       bool canshoot=true;
    private Animator botanimation;

    public Transform shootpoint;

    void Start()
    {
        botanimation=GetComponent<Animator>();   
    }
    public void Fire()
    {
           if(!canshoot)
        {
            return;
        }
          
         
         if(currentWeapon!=null)
         {
           currentWeapon.PlaySound();
           currentWeapon.PlayMuzzleFlash();
         }

       

       
        Ray ray = new Ray(shootpoint.position, shootpoint.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, currentWeapon.range))
        {
            Health h = hit.collider.GetComponent<Health>();
           
            if (h != null)
            {
                h.Damage(currentWeapon.damage);
            }
            
        }
    }

  public  void StopShooting()
    {
         canshoot=false;
         botanimation.SetBool("Fire",false);
    }

    public  void allowshooting()
    {
        canshoot=true;
    }
}
