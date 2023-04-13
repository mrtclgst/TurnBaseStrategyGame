using System;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    private void Start()
    {
        ShootAction.OnEventAnyShoot += ShootAction_EventAnyShoot;
        GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnOnAnyGrenadeExploded;
    }

    private void OnDestroy()
    {
        ShootAction.OnEventAnyShoot -= ShootAction_EventAnyShoot;
        GrenadeProjectile.OnAnyGrenadeExploded -= GrenadeProjectile_OnOnAnyGrenadeExploded;
    }

    private void GrenadeProjectile_OnOnAnyGrenadeExploded(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(1);
    }


    private void ShootAction_EventAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        ScreenShake.Instance.Shake(1);
    }
}