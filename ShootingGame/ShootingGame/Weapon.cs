using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    /// <summary>
    /// Types of weapon
    /// </summary>
    enum WeaponType { BoltAction, SemiAuto, FullAuto}

    /// <summary>
    /// Represents the Weapon
    /// </summary>
    class Weapon
    {
        WeaponType shootType;
        int maxAmmo;
        int autoShootTimer;
        int totalAmmo;
        int reloadTime;
        bool canShoot;
        bool canPlayGunCockingSound;
        object thisLock = new object();
        MouseState mouseState;
        SoundEffect effect;
        SoundEffect effectGunCocking;
        public bool IsReloading { get; private set; }
        public string Name { get; private set; }
        public int Ammo { get; private set; }
        public int DamageLevel { get; private set; }
        public Texture2D Sprite { get; private set; }
        public int CurrentReloadTime { get; private set; }
        public int TotalAmmo
        {
            get
            {
                lock(thisLock)
                return totalAmmo;
            }
            set
            {
                lock(thisLock)
                {
                    totalAmmo = value;
                    if (totalAmmo > 200) totalAmmo = 200;
                }
            }
        }
        
        /// <summary>
        /// The Weapon's constructor
        /// </summary>
        /// <param name="name">Weapon's name</param>
        /// <param name="maxAmmo">Weapon's max ammo</param>
        /// <param name="damageLevel">Bullet's damage level</param>
        /// <param name="reloadTime">Reloading time</param>
        /// <param name="shootType">Weapon type</param>
        public Weapon(string name, int maxAmmo, int damageLevel, int reloadTime, WeaponType shootType)
        {
            this.Name = name;
            this.TotalAmmo = 0;
            this.maxAmmo = this.Ammo = maxAmmo;
            this.DamageLevel = damageLevel;
            this.reloadTime = this.CurrentReloadTime = reloadTime;
            this.shootType = shootType;
            IsReloading = false;
            autoShootTimer = 0;
            canShoot = false;
            canPlayGunCockingSound = true;
        }

        /// <summary>
        /// Loads the weapon's content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            if (shootType == WeaponType.BoltAction)
            {
                effect = content.Load<SoundEffect>("gunshot");
                Sprite = content.Load<Texture2D>("pistolsprite");
            }
            else if (shootType == WeaponType.SemiAuto)
            {
                effect = content.Load<SoundEffect>("hithard");
                Sprite = content.Load<Texture2D>("riflesprite");
            }
            else if (shootType == WeaponType.FullAuto)
            {
                effect = content.Load<SoundEffect>("gunshot");
                Sprite = content.Load<Texture2D>("machinegunsprite");
            }
            effectGunCocking = content.Load<SoundEffect>("gun-cocking-03");
        }

        public void UpdateWeaponStatus()
        {
            mouseState = Mouse.GetState();

            if (IsReloading && TotalAmmo > 0) Reload();
            else if (Ammo <= 0 && !IsReloading)
            {
                canShoot = false;
                IsReloading = true;
                Reload();
            }
            else if (mouseState.LeftButton == ButtonState.Pressed && canShoot && !IsReloading)
            {
                if (shootType == WeaponType.BoltAction || shootType == WeaponType.SemiAuto)
                {
                    Player.PlayAnimation = true;
                    Ammo--;
                    GameWorld.Instance.CanAddPlayerBullet = true;
                    effect.Play();
                    canShoot = false;
                }
                else if (shootType == WeaponType.FullAuto)
                {
                    autoShootTimer++;
                    if (autoShootTimer >= 7)
                    {
                        Player.PlayAnimation = true;
                        Ammo--;
                        GameWorld.Instance.CanAddPlayerBullet = true;
                        effect.Play();
                        autoShootTimer = 0;
                    }
                }
            }
            else if (mouseState.LeftButton == ButtonState.Released) canShoot = true;
        }

        public void Reload()
        {
            if(CurrentReloadTime < reloadTime/2 && canPlayGunCockingSound)
            {
                effectGunCocking.Play();
                canPlayGunCockingSound = false;
            }
            if (CurrentReloadTime <= 0)
            {
                lock(thisLock)
                {
                    if (maxAmmo <= totalAmmo)
                    {
                        Ammo = maxAmmo;
                        totalAmmo -= maxAmmo;
                    }
                    else
                    {
                        Ammo = totalAmmo;
                        totalAmmo = 0;
                    }
                }  
                CurrentReloadTime = reloadTime;
                IsReloading = false;
                canPlayGunCockingSound = true;
            }
            else CurrentReloadTime -= 20;   
        }

        public void RestartWeapon()
        {
            TotalAmmo = 0;
            Ammo = maxAmmo;
            CurrentReloadTime = reloadTime;
            IsReloading = false;
            canShoot = false;
            autoShootTimer = 0;
            canPlayGunCockingSound = true;
        }
    }
}
