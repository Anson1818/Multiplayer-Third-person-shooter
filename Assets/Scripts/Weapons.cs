using UnityEngine;

public class Weapons : MonoBehaviour
{
     public float fireRate = 0.2f;
    public float damage ;
    public float range = 200f;
    public int Toatalammo;
    public int  currentammo;
    public Sprite Weaponicon;

    public Transform leftHandIK;
    public Transform muzzlePoint;

  
    public ParticleSystem muzzleFlash;
    public AudioClip shootSound;

     AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
       currentammo=Toatalammo;
    }

    public void PlayMuzzleFlash()
    {
        if (muzzleFlash) muzzleFlash.Play();
    }

    public void PlaySound()
    {
        if (shootSound&&!audioSource.isPlaying)
        { 
            audioSource.PlayOneShot(shootSound);
        }
    }
   
}
