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
    enum ShootType { Gun, Rifle, MachineGun}

    class Weapon
    {
        int maxAmmo;
        ShootType shootType;
        bool canPlaySound;
        bool canShoot;
        int autoShootTimer;
        MouseState mouseState;
        Song sound;
        int reloadTime;
        public string Name { get; private set; }
        public int Ammo { get; private set; }
        int damageLevel;
        public Texture2D Sprite;
        public int CurrentReloadTime { get; private set; }
        public bool IsReloading;
        
        public Weapon(string name, int maxAmmo, int damageLevel, int reloadTime, ShootType shootType)
        {
            this.Name = name;
            this.maxAmmo = this.Ammo = maxAmmo;
            this.damageLevel = damageLevel;
            this.reloadTime = this.CurrentReloadTime = reloadTime;
            this.shootType = shootType;
            canPlaySound = true;
            IsReloading = false;
            autoShootTimer = 0;  
        }

        /// <summary>
        /// Loads the player's content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            if (shootType == ShootType.Gun)
            {
                sound = content.Load<Song>("pistol");
                Sprite = content.Load<Texture2D>("pistolsprite");
            }
            else if (shootType == ShootType.Rifle)
            {
                sound = content.Load<Song>("sniper");
                Sprite = content.Load<Texture2D>("riflesprite");
            }
            else if (shootType == ShootType.MachineGun)
            {
                sound = content.Load<Song>("sniper");
                Sprite = content.Load<Texture2D>("machinegunsprite");
            }
        }

        public void UpdateWeaponStatus()
        {
            mouseState = Mouse.GetState();

            if (IsReloading) Reload();
            else if (Ammo <= 0 && !IsReloading)
            {
                canShoot = false;
                IsReloading = true;
                Reload();
            }
            else if (mouseState.LeftButton == ButtonState.Pressed && canShoot && !IsReloading)
            {
                if(shootType==ShootType.Gun || shootType == ShootType.Rifle)
                {
                    Player.PlayAnimation = true;
                    Ammo--;
                    MediaPlayer.Play(sound);
                    canShoot = false;
                }
                else if(shootType == ShootType.MachineGun)
                {
                    autoShootTimer++;
                    if(autoShootTimer >= 8)
                    {
                        Player.PlayAnimation = true;
                        Ammo--;
                        MediaPlayer.Play(sound);
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
            if (CurrentReloadTime <= 0)
            {
                Ammo = maxAmmo;
                CurrentReloadTime = reloadTime;
                IsReloading = false;
            }
            else CurrentReloadTime -= 20;
            
        }
    }
}
