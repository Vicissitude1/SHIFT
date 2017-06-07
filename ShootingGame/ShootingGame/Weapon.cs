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
    public enum WeaponType { BoltAction, SemiAuto, FullAuto}

    /// <summary>
    /// Represents the Weapon
    /// </summary>
    public class Weapon
    {
        /// <summary>
        /// Type of weapon
        /// </summary>
        WeaponType shootType;

        /// <summary>
        /// Shooting timer for Machinegun (FullAuto) 
        /// </summary>
        int autoShootTimer;

        /// <summary>
        /// Reserve ammo
        /// </summary>
        int totalAmmo;

        /// <summary>
        /// Reloading time
        /// </summary>
        int reloadTime;

        /// <summary>
        /// The object that is going to be locked
        /// </summary>
        object thisLock = new object();

        /// <summary>
        /// th mouse state
        /// </summary>
        MouseState mouseState;

        /// <summary>
        /// The weapon's sound effects
        /// </summary>
        SoundEffect effect;
        SoundEffect effectGunCocking;

        /// <summary>
        /// Checks if weapon can shoot
        /// </summary>
        public bool CanShoot { get; set; }

        /// <summary>
        /// Checks if necassery to play  gun cocking sound effect
        /// </summary>
        public bool CanPlayGunCockingSound { get; set; }

        /// <summary>
        /// Max possible ammo
        /// </summary>
        public int MaxAmmo { get; private set; }

        /// <summary>
        /// Checks if weapon is reloading
        /// </summary>
        public bool IsReloading { get; set; }

        /// <summary>
        /// The weapon's name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The weapos ammo
        /// </summary>
        public int Ammo { get; set; }

        /// <summary>
        /// The bullets damage level
        /// </summary>
        public int DamageLevel { get; private set; }

        /// <summary>
        /// The weapon's sprite
        /// </summary>
        public Texture2D Sprite { get; private set; }

        /// <summary>
        /// The weapon's current reloading time
        /// </summary>
        public int CurrentReloadTime { get; set; }

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
                    if (totalAmmo > 100) totalAmmo = 100;
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
            this.MaxAmmo = this.Ammo = maxAmmo;
            this.DamageLevel = damageLevel;
            this.reloadTime = this.CurrentReloadTime = reloadTime;
            this.shootType = shootType;
            IsReloading = false;
            autoShootTimer = 0;
            CanShoot = false;
            CanPlayGunCockingSound = true;
        }

        /// <summary>
        /// Loads the weapon's content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            //  Loads content for Gun (sound and sprite)
            if (shootType == WeaponType.BoltAction)
            {
                effect = content.Load<SoundEffect>("gunshot");
                Sprite = content.Load<Texture2D>("pistolsprite1");
            }
            // Loads content for Rifle
            else if (shootType == WeaponType.SemiAuto)
            {
                effect = content.Load<SoundEffect>("rifleshot");
                Sprite = content.Load<Texture2D>("riflesprite1");
            }
            // Loads content for MachineGun
            else if (shootType == WeaponType.FullAuto)
            {
                effect = content.Load<SoundEffect>("gunshot");
                Sprite = content.Load<Texture2D>("machinegunsprite1");
            }
            effectGunCocking = content.Load<SoundEffect>("gun-cocking-03");
        }

        /// <summary>
        /// Updates the weapon's status
        /// </summary>
        public void UpdateWeaponStatus()
        {
            mouseState = Mouse.GetState();
            // Performs reloading the weapon if it is started and there is total ammo (reserve)
            if (IsReloading && TotalAmmo > 0) Reload();
            // Starts the reloading if there is no more ammo in the weapon
            else if (Ammo <= 0 && !IsReloading)
            {
                CanShoot = false;
                IsReloading = true;
                Reload();
            }
            // Checks UI, if necassery to perform shooting
            else if (mouseState.LeftButton == ButtonState.Pressed && CanShoot && !IsReloading)
            {
                // Performs shooting if Gun or Rifle
                if (shootType == WeaponType.BoltAction || shootType == WeaponType.SemiAuto)
                {
                    Player.PlayAnimation = true;
                    Ammo--;
                    GameWorld.Instance.CanAddPlayerBullet = true;
                    effect.Play();
                    CanShoot = false;
                }
                // Performs shooting if Machinegun
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
            else if (mouseState.LeftButton == ButtonState.Released) CanShoot = true;
        }

        /// <summary>
        /// Reloads the weapon
        /// </summary>
        public void Reload()
        {
            // Plays gun cocking sound effect
            if(CurrentReloadTime < reloadTime/1.3 && CanPlayGunCockingSound)
            {
                effectGunCocking.Play();
                CanPlayGunCockingSound = false;
            }
            // Subtracts ammo from total ammo (reserve) to the current ammo
            if (CurrentReloadTime <= 0)
            {
                lock(thisLock)
                {
                    if (MaxAmmo <= totalAmmo)
                    {
                        Ammo = MaxAmmo;
                        totalAmmo -= MaxAmmo;
                    }
                    else
                    {
                        Ammo = totalAmmo;
                        totalAmmo = 0;
                    }
                }  
                CurrentReloadTime = reloadTime;
                IsReloading = false;
                CanPlayGunCockingSound = true;
            }
            else CurrentReloadTime -= 20;   
        }

        /// <summary>
        ///  Resets all the fields when the game is restarted 
        /// </summary>
        public void RestartWeapon()
        {
            TotalAmmo = 0;
            Ammo = MaxAmmo;
            CurrentReloadTime = reloadTime;
            IsReloading = false;
            CanShoot = false;
            autoShootTimer = 0;
            CanPlayGunCockingSound = true;
        }
    }
}
