﻿using Microsoft.Xna.Framework.Content;
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
    enum ShootType { Gun, Rifle, MachineGun}

    class Weapon
    {
        int maxAmmo;
        ShootType shootType;
        bool canPlaySound;
        bool canShoot;
        int autoShootTimer;
        MouseState mouseState;
        //Song sound;
        int reloadTime;
        //Song gunCocking;
        bool canPlayGunCockingSound;
        public int TotalAmmo { get; set; }
        public string Name { get; private set; }
        public int Ammo { get; private set; }
        public int DamageLevel { get; private set; }
        public Texture2D Sprite;
        public int CurrentReloadTime { get; private set; }
        public bool IsReloading;
        
        public Weapon(string name, int maxAmmo, int damageLevel, int reloadTime, ShootType shootType)
        {
            this.Name = name;
            this.TotalAmmo = 0;
            this.maxAmmo = this.Ammo = maxAmmo;
            this.DamageLevel = damageLevel;
            this.reloadTime = this.CurrentReloadTime = reloadTime;
            this.shootType = shootType;
            canPlaySound = true;
            IsReloading = false;
            autoShootTimer = 0;
            canShoot = false;
            canPlayGunCockingSound = true;
        }

        /// <summary>
        /// Loads the player's content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            if (shootType == ShootType.Gun)
            {
                GameWorld.Instance.GunIsActive = true;
                GameWorld.Instance.MachineGunIsActive = false;
                GameWorld.Instance.RifleIsActive = false;
                //sound = content.Load<Song>("pistol");
                Sprite = content.Load<Texture2D>("pistolsprite");
            }
            else if (shootType == ShootType.Rifle)
            {
                GameWorld.Instance.GunIsActive = false;
                GameWorld.Instance.MachineGunIsActive = false;
                GameWorld.Instance.RifleIsActive = true;
                //sound = content.Load<Song>("sniper");
                Sprite = content.Load<Texture2D>("riflesprite");
            }
            else if (shootType == ShootType.MachineGun)
            {
                GameWorld.Instance.GunIsActive = false;
                GameWorld.Instance.MachineGunIsActive = true;
                GameWorld.Instance.RifleIsActive = false;
                //sound = content.Load<Song>("sniper");
                Sprite = content.Load<Texture2D>("machinegunsprite");
            }
            //gunCocking = content.Load<Song>("gun-cocking-03");
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
                if (shootType == ShootType.Gun)
                {
                    Player.PlayAnimation = true;
                    Ammo--;
                    GameWorld.Instance.CanAddPlayerBullet = true;
                    GameWorld.Instance.Engine.Play2D("Content/pistol.wav", false);
                    //MediaPlayer.Play(sound);
                    canShoot = false;
                }
                else if (shootType == ShootType.Rifle)
                {
                    Player.PlayAnimation = true;
                    Ammo--;
                    GameWorld.Instance.CanAddPlayerBullet = true;
                    GameWorld.Instance.Engine.Play2D("Content/sniper.wav", false);
                    //MediaPlayer.Play(sound);
                    canShoot = false;
                }
                else if (shootType == ShootType.MachineGun)
                {
                    autoShootTimer++;
                    if (autoShootTimer >= 8)
                    {
                        Player.PlayAnimation = true;
                        Ammo--;
                        GameWorld.Instance.CanAddPlayerBullet = true;
                        GameWorld.Instance.Engine.Play2D("Content/pistol.wav", false);
                        //MediaPlayer.Play(sound);
                        autoShootTimer = 0;
                    }
                }
            }
            else if (mouseState.LeftButton == ButtonState.Released)
            {
                canShoot = true;
            }
        }

        public void Reload()
        {
            if(CurrentReloadTime < reloadTime/2 && canPlayGunCockingSound)
            {
                GameWorld.Instance.Engine.Play2D("Content/gun-cocking-03.wav", false);
                //MediaPlayer.Play(gunCocking);
                canPlayGunCockingSound = false;
            }
            if (CurrentReloadTime <= 0)
            {
                if(maxAmmo <= TotalAmmo)
                {
                    Ammo = maxAmmo;
                    TotalAmmo -= maxAmmo;
                }
                else
                {
                    Ammo = TotalAmmo;
                    TotalAmmo = 0;
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
            canPlaySound = true;
            IsReloading = false;
            canShoot = false;
            autoShootTimer = 0;
            canPlayGunCockingSound = true;
        }
    }
}
