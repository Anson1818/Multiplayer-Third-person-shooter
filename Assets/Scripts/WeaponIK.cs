using UnityEngine;

public class WeaponIK : MonoBehaviour
{
    public Transform leftHandTarget;
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (!leftHandTarget) return;

        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);

        anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
        anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
    }
}
