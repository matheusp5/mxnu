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

            pool.Add(menu);
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

        }
    }
}
