using GTA;
using LemonUI.Menus;
using LemonUI;
using System;
using System.Windows.Forms;
using System.Security.Principal;
using GTA.Math;
using GTA.Native;
using System.Linq;

namespace Mxnu.Core
{
    public class Main : Script
    {
        private Player player { get; set; } = Game.Player;
        public ObjectPool pool { get; set; }
        public NativeMenu menu { get; set; }
        public NativeMenu moneyMenu { get; set; }
        public NativeMenu playerMenu { get; set; }
        public NativeMenu weaponsMenu { get; set; }
        public NativeMenu vehicleMenu { get; set; }
        public NativeMenu weatherMenu { get; set; }
        public NativeListItem<int> runSpeedItem { get; set; }
        public NativeListItem<int> swimSpeedItem { get; set; }
        public WeaponHash[] weapons { get; set; } = Weapon.GetAllWeaponHashesForHumanPeds();
        public bool explosiveBullets { get; set; } = false;
        public bool fireBullets { get; set; } = false;
        public bool multiplierEnabled { get; set; } = false;
        public float runSpeedMultipler { get; set; } = 1;
        public float swimSpeedMultipler { get; set; } = 1;
        public bool noClipEnable { get; set; } = false;
        public int noClipSpeed { get; set; } = 2;
        public float noClipedX { get; set; }
        public float noClipedY { get; set; }
        public float noClipedZ { get; set; }
        public bool ignoredByPoliceEnable { get; set; } = false;
        public bool superJumpEnable { get; set; } = false;
        public bool alwaysClean { get; set; } = false;
        public NativeListItem<int> customHourItem { get; set; }

        public Main()
        {
            Init();
        }

        private void Init() {
            pool = new ObjectPool();
            menu = new NativeMenu("Mxnu", "~b~ github.com/ofmxtheuuz");

            menu.BannerText.Font = GTA.UI.Font.Pricedown;
            menu.BannerText.Color = System.Drawing.Color.FromArgb(255, 255, 255);
            menu.Banner.Color = System.Drawing.Color.FromArgb(128, 0, 128);

            menu.NameFont = GTA.UI.Font.Monospace;
            menu.DescriptionFont = GTA.UI.Font.Monospace;

            InitPlayerMenu();
            InitWeaponsMenu();
            InitVehicleMenu();
            InitMoneyMenu();
            InitWeatherMenu();

            pool.Add(menu);
        }

        private void InitWeatherMenu()
        {
            var weatherItemMenu = new NativeItem("Weather Options");

            var weatherMenu = new NativeMenu("Weather");
            this.weatherMenu = weatherMenu;
        }

        private void InitMoneyMenu()
        {
            var moneyItemMenu = new NativeItem("Money Options");
            menu.Add(moneyItemMenu);
            moneyItemMenu.Activated += MoneyItemPressed;
            var moneyMenu = new NativeMenu("Money");

            var add1k = new NativeItem("Add 1.000");
            add1k.Activated += (object sender, EventArgs e) =>
            {
                AddMoney("1000");
            };
            moneyMenu.Add(add1k);

            var add1kk = new NativeItem("Add 1.000.000");
            add1kk.Activated += (object sender, EventArgs e) =>
            {
                AddMoney("1000000");
            };
            moneyMenu.Add(add1kk);

            var add1kkk = new NativeItem("Add 1.000.000.000");
            add1kkk.Activated += (object sender, EventArgs e) =>
            {
                AddMoney("1000000000");
            };
            moneyMenu.Add(add1kkk);

            var addCustom = new NativeItem("Add Custom");
            addCustom.Activated += (object sender, EventArgs e) =>
            {
                AddMoney(null);
            };
            moneyMenu.Add(addCustom);


            var diviser = new NativeItem("--------------------------");
            moneyMenu.Add(diviser);


            var remove1k = new NativeItem("Remove 1.000");
            remove1k.Activated += (object sender, EventArgs e) =>
            {
                RemoveMoney("1000");
            };
            moneyMenu.Add(remove1k);


            var remove1kk = new NativeItem("Remove 1.000.000");
            remove1kk.Activated += (object sender, EventArgs e) =>
            {
                RemoveMoney("1000000");
            };
            moneyMenu.Add(remove1kk);


            var remove1kkk = new NativeItem("Remove 1.000.000.000");
            remove1kkk.Activated += (object sender, EventArgs e) =>
            {
                RemoveMoney("1000000000");
            };
            moneyMenu.Add(remove1kkk);


            var removeCustom = new NativeItem("Remove Custom");
            removeCustom.Activated += (object sender, EventArgs e) =>
            {
                RemoveMoney(null);
            };
            moneyMenu.Add(removeCustom);

            pool.Add(moneyMenu);
            this.moneyMenu = moneyMenu;
        }

        private void InitVehicleMenu()
        {

            var vehicleItemMenu = new NativeItem("Vehicle Options");


            var vehicleMenu = new NativeMenu("Vehicle");
            this.vehicleMenu = vehicleMenu;

            var enableInvencibleVehicle = new NativeCheckboxItem("Invencible Vehicle");
            enableInvencibleVehicle.CheckboxChanged += (object sender, EventArgs e) =>
            {
                player.Character.CurrentVehicle.CanBeVisiblyDamaged = !enableInvencibleVehicle.Checked;
                player.Character.CurrentVehicle.CanEngineDegrade = !enableInvencibleVehicle.Checked;
                player.Character.CurrentVehicle.CanTiresBurst = !enableInvencibleVehicle.Checked;
            };
            vehicleMenu.Add(enableInvencibleVehicle);


            var repairVehicle = new NativeItem("Repair");
            repairVehicle.Activated += (object sender, EventArgs e) =>
            {
                player.Character.CurrentVehicle.Repair();
            };
            vehicleMenu.Add(repairVehicle);

            var enableAlwaysClean = new NativeCheckboxItem("Vehicle Always Clean");
            enableAlwaysClean.CheckboxChanged += (object sender, EventArgs e) =>
            {
                alwaysClean = enableAlwaysClean.Checked;
            };
            vehicleMenu.Add(enableAlwaysClean);


            var clearVehicle = new NativeItem("Clear vehicle");
            clearVehicle.Activated += (object sender, EventArgs e) =>
            {
                player.Character.CurrentVehicle.DirtLevel = 0;
            };
            vehicleMenu.Add(clearVehicle);

            var setMaxSpeed = new NativeItem("Change Vehicle Max Speed");
            setMaxSpeed.Activated += (object sender, EventArgs e) =>
            {
                float maxSpeed = Convert.ToInt32(Game.GetUserInput(WindowTitle.EnterCustomTeamName, null, 9));
                player.Character.CurrentVehicle.MaxSpeed = maxSpeed;
            };
            vehicleMenu.Add(setMaxSpeed);

            var setVehicleTorqueMultiplier = new NativeItem("Vehicle Torque Multiplier");
            setVehicleTorqueMultiplier.Activated += (object sender, EventArgs e) =>
            {
                float multiplier = Convert.ToInt32(Game.GetUserInput(WindowTitle.EnterCustomTeamName, null, 9));
                player.Character.CurrentVehicle.EngineTorqueMultiplier = multiplier;
            };
            vehicleMenu.Add(setVehicleTorqueMultiplier);

            var setEnginePowerMultiplier = new NativeItem("Engine Power Multiplier");
            setEnginePowerMultiplier.Activated += (object sender, EventArgs e) =>
            {
                float multiplier = Convert.ToInt32(Game.GetUserInput(WindowTitle.EnterCustomTeamName, null, 9));
                player.Character.CurrentVehicle.EnginePowerMultiplier = multiplier;
            };
            vehicleMenu.Add(setEnginePowerMultiplier);

            var setBrakesPowerMultiplier = new NativeItem("Brakes Power Multiplier");
            setBrakesPowerMultiplier.Activated += (object sender, EventArgs e) =>
            {
                float multiplier = Convert.ToInt32(Game.GetUserInput(WindowTitle.EnterCustomTeamName, null, 9));
                player.Character.CurrentVehicle.BrakePower = player.Character.CurrentVehicle.BrakePower * multiplier;
            };
            vehicleMenu.Add(setBrakesPowerMultiplier);

            vehicleItemMenu.Activated += (object sender, EventArgs e) =>
            {
                menu.Visible = false;
                this.vehicleMenu.Visible = true;
            };

            pool.Add(vehicleMenu);
            menu.Add(vehicleItemMenu);
        }

        private void InitWeaponsMenu()
        {
            var weaponsItemMenu = new NativeItem("Weapons Options");

            var weaponsMenu = new NativeMenu("Weapons");

            var getAllWeapons = new NativeItem("Get all weapons");
            getAllWeapons.Activated += (object sender, EventArgs e) =>
            {
                foreach (WeaponHash hash in weapons)
                {
                    player.Character.Weapons.Give(hash, 999, false, true);
                }
            };
            weaponsMenu.Add(getAllWeapons);

            var removeAllWeapons = new NativeItem("Remove all weapons");
            removeAllWeapons.Activated += (object sender, EventArgs e) =>
            {
                player.Character.Weapons.RemoveAll();
            };
            weaponsMenu.Add(removeAllWeapons);


            var enableUnlimitedAmmo = new NativeCheckboxItem("Unlimited ammo");
            enableUnlimitedAmmo.CheckboxChanged += (object sender, EventArgs e) =>
            {
                if (enableUnlimitedAmmo.Checked)
                {
                    foreach (WeaponHash hash in weapons)
                    {
                        var weapon = player.Character.Weapons.Give(hash, 999, true, true);
                        weapon.InfiniteAmmo = true;
                        weapon.InfiniteAmmoClip = true;
                    }
                }
                else
                {
                    player.Character.Weapons.RemoveAll();
                    foreach (WeaponHash hash in weapons)
                    {
                        player.Character.Weapons.Give(hash, 999, false, true);
                    }
                }

            };
            weaponsMenu.Add(enableUnlimitedAmmo);

            var enableExplosiveBullets = new NativeCheckboxItem("Explosive Bullets");
            enableExplosiveBullets.CheckboxChanged += (object sender, EventArgs e) =>
            {
                explosiveBullets = enableExplosiveBullets.Checked;
            };
            weaponsMenu.Add(enableExplosiveBullets);

            var enableFireBullets = new NativeCheckboxItem("Fire Bullets");
            enableFireBullets.CheckboxChanged += (object sender, EventArgs e) =>
            {
                fireBullets = enableFireBullets.Checked;
            };
            weaponsMenu.Add(enableFireBullets);

            this.weaponsMenu = weaponsMenu;
            pool.Add(weaponsMenu);

            weaponsItemMenu.Activated += (object sender, EventArgs e) =>
            {
                menu.Visible = false;
                this.weaponsMenu.Visible = true;
            };
            menu.Add(weaponsItemMenu);

        }

        private void InitPlayerMenu()
        {
            int[] tenItemsList = new int[10];
            for (int i = 1; i <= 10; i++)
            {
                tenItemsList[i - 1] = i * 10;
            }

            var playerMenuItem = new NativeItem("Player Options");
            menu.Add(playerMenuItem);

            var playerMenu = new NativeMenu("Player");

            playerMenuItem.Activated += (object sender, EventArgs e) =>
            {
                menu.Visible = false;
                playerMenu.Visible = true;
            };

            var playerGodmode = new NativeCheckboxItem("Godmode");
            playerGodmode.CheckboxChanged += (object sender, EventArgs e) =>
            {
                if (playerGodmode.Checked)
                {
                    player.IsInvincible = true;
                    player.Character.IsInvincible = true;
                    player.Character.Health = 100;
                }
                else
                {
                    player.IsInvincible = false;
                    player.Character.IsInvincible = false;
                }
            };
            playerMenu.Add(playerGodmode);

            var multiplierEnable = new NativeCheckboxItem("Enable Multiplier");
            multiplierEnable.CheckboxChanged += (object sender, EventArgs e) =>
            {
                multiplierEnabled = multiplierEnable.Checked;
                if (!multiplierEnable.Checked)
                {
                    runSpeedItem.SelectedIndex = 0;
                    swimSpeedItem.SelectedIndex = 0;
                }
            };
            playerMenu.Add(multiplierEnable);

            var runSpeedMultiplier = new NativeListItem<int>("Run Speed Multiplier", tenItemsList);
            runSpeedMultiplier.ItemChanged += (object sender, ItemChangedEventArgs<int> e) =>
            {
                runSpeedMultipler = runSpeedMultiplier.SelectedItem;
            };
            runSpeedItem = runSpeedMultiplier;
            playerMenu.Add(runSpeedMultiplier);

            var customRunSpeedMultiplier = new NativeItem("Custom Run Speed Multiplier");
            customRunSpeedMultiplier.Activated += (object sender, EventArgs e) =>
            {
                runSpeedMultipler = float.Parse(Game.GetUserInput(WindowTitle.EnterCustomTeamName, null, 5));
            };
            playerMenu.Add(customRunSpeedMultiplier);

            var swimSpeedMultiplier = new NativeListItem<int>("Swim Speed Multiplier", tenItemsList);
            swimSpeedMultiplier.ItemChanged += (object sender, ItemChangedEventArgs<int> e) =>
            {
                swimSpeedMultipler = swimSpeedMultiplier.SelectedItem;
            };
            swimSpeedItem = swimSpeedMultiplier;
            playerMenu.Add(swimSpeedMultiplier);

            var customSwimSpeedMultiplier = new NativeItem("Custom Swim Speed Multiplier");
            customSwimSpeedMultiplier.Activated += (object sender, EventArgs e) =>
            {
                swimSpeedMultipler = float.Parse(Game.GetUserInput(WindowTitle.EnterCustomTeamName, null, 5));
            };
            playerMenu.Add(customSwimSpeedMultiplier);

            var ignoredByPolice = new NativeCheckboxItem("Never Wanted");
            ignoredByPolice.CheckboxChanged += (object sender, EventArgs e) =>
            {
                player.IgnoredByPolice = ignoredByPolice.Checked;
                ignoredByPoliceEnable = ignoredByPolice.Checked;
            };
            playerMenu.Add(ignoredByPolice);

            var setWantedLevel = new NativeListItem<int>("Wanted Level", new int[] { 0, 1, 2, 3, 4, 5 });
            setWantedLevel.ItemChanged += (object sender, ItemChangedEventArgs<int> e) =>
            {
                player.WantedLevel = setWantedLevel.SelectedItem;
            };
            playerMenu.Add(setWantedLevel);

            var enableNoClip = new NativeCheckboxItem("No Clip");
            enableNoClip.CheckboxChanged += (object sender, EventArgs e) =>
            {
                noClipEnable = enableNoClip.Checked;
                player.Character.HasGravity = true;
                if (enableNoClip.Checked)
                {
                    noClipedX = player.Character.Position.X;
                    noClipedY = player.Character.Position.Y;
                    noClipedY = player.Character.Position.Z;


                    player.Character.IsVisible = false;
                    player.Character.IsInvincible = true;

                }
                else
                {
                    player.Character.IsVisible = true;
                    player.Character.IsInvincible = false;
                }
            };
            playerMenu.Add(enableNoClip);

            var enableSuperJump = new NativeCheckboxItem("Super Jump");
            enableSuperJump.CheckboxChanged += (object sender, EventArgs e) =>
            {
                superJumpEnable = enableSuperJump.Checked;
            };
            playerMenu.Add(enableSuperJump);

            this.playerMenu = playerMenu;
            pool.Add(playerMenu);

        }

        private void MoneyItemPressed(object sender, EventArgs e)
        {
            menu.Visible = false;
            moneyMenu.Visible = true;
        }

        private void AddMoney(string money)
        {
            if (money == null)
            {
                string input = Game.GetUserInput(WindowTitle.CustomTeamName, "", 9);
                player.Money += Convert.ToInt32(input);
            }
            else
            {
                player.Money += Convert.ToInt32(money);
            }

            GTA.UI.Notification.Show("Check your money");
        }

        private void RemoveMoney(string money)
        {
            if (money == null)
            {
                string input = Game.GetUserInput(WindowTitle.CustomTeamName, "", 9);
                player.Money -= Convert.ToInt32(input);
            }
            else
            {
                player.Money -= Convert.ToInt32(money);
            }
            GTA.UI.Notification.Show("Check your money");
        }
    }
}
