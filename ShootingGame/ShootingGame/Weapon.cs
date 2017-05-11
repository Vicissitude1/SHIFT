using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    enum WeaponType { Gun, Rifle, MachineGun}

    class Weapon
    {
        WeaponType weaponType;
        bool canShoot;
        int ammo;
        int damageLevel;
        int reloadTime;

        public Weapon(WeaponType weapontype, int ammo, int damageLevel, int reloadTime)
        {
            this.weaponType = weaponType;
            this.ammo = ammo;
            this.damageLevel = damageLevel;
            this.reloadTime = reloadTime;
        }

        public void Shoot()
        {

        }
    }
}
