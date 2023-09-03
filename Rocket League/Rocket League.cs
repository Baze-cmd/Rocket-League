
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Rocket_League
{
    public partial class RocketLeague : Form
    {
        public World world { get; set; }
        public Button freeplay { get; set; }
        public Button multiplayer { get; set; }
        public Button settings { get; set; }
        public bool isInMainMenu { get; set; }
        public Button explination { get; set; }
        public Button highFPSMode { get; set; }
        public Button music { get; set; }
        public Button goToMainMenu { get; set; }
        SoundPlayer simpleSound = new SoundPlayer(@"../../../assets/Koven - Light Up [Monstercat Release].wav");
        public RocketLeague()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            world = new World(80, 55, 820, 535, true);
            freeplay = new Button(new Vector2D(10, 10), 250, 50, false, "FREEPLAY");
            multiplayer = new Button(new Vector2D(10, 70), 250, 50, false, "MULTYPLAYER");
            settings = new Button(new Vector2D(700, 750), 200, 50, false, "SETTINGS");
            isInMainMenu = true;
            explination = new Button(new Vector2D(10, 400), 900, 300, false, "HOW TO PLAY:\nA - jump\nD - boost\nR - reset kickoff\nUse the joystink at the bottom of the screen to control the cars direction\n GL :)\nESC - settings\n(tip: turn on more FPS in settings lol)");
            highFPSMode = new Button(new Vector2D(Width / 2 - 100, Height / 2 - 200), 200, 50, false, " MORE FPS");
            music = new Button(new Vector2D(Width / 2 - 100, Height / 2 - 150), 200, 50, true, "Switch music");
            goToMainMenu = new Button(new Vector2D(Width / 2 - 100, Height / 2 - 100), 200, 50, false, "Main menu");
            simpleSound.PlayLooping();
        }
        private void RocketLeague_Paint(object sender, PaintEventArgs e)
        {
            if (settings.isOn)
            {
                highFPSMode.draw(e.Graphics);
                music.draw(e.Graphics);
                if (!isInMainMenu)
                    goToMainMenu.draw(e.Graphics);
            }
            if (!world.isPaused)
            {
                world.update();
            }
            if (isInMainMenu)
            {
                freeplay.draw(e.Graphics);
                multiplayer.draw(e.Graphics);
                explination.draw(e.Graphics);
            }
            else
            {
                world.draw(e.Graphics);
            }
            if (settings.isOn)
            {
                highFPSMode.draw(e.Graphics);
                music.draw(e.Graphics);
                if (!isInMainMenu)
                    goToMainMenu.draw(e.Graphics);
            }
            settings.draw(e.Graphics);
            Invalidate();
        }

        private void RocketLeague_MouseMove(object sender, MouseEventArgs e)
        {
            world.Input(e.Location);
        }

        private void RocketLeague_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                world.car.Jump();
            }
            if (e.KeyCode == Keys.D)
            {
                world.car.StartBoost();
            }
            if (e.KeyCode == Keys.R)
            {
                world.resetKickoff();
            }
            if (e.KeyCode == Keys.Escape)
            {
                world.switchPause();
                settings.isOn = !settings.isOn;
            }
        }

        private void RocketLeague_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D)
            {
                world.car.StopBoost();
            }
        }

        private void RocketLeague_MouseDown(object sender, MouseEventArgs e)
        {
            if (isInMainMenu && freeplay.click(e.Location))
            {
                world.switchPause();
                isInMainMenu = false;
                return;
            }
            if (settings.click(e.Location))
            {
                world.switchPause();
            }
            if (settings.isOn && music.click(e.Location))
            {
                if (!music.isOn)
                {
                    simpleSound.Stop();
                }
                if (music.isOn)
                {
                    simpleSound.PlayLooping();
                }
            }
            if (settings.isOn && highFPSMode.click(e.Location))
            {
                world.highFPSMode = !world.highFPSMode;
            }
            if (settings.isOn && goToMainMenu.click(e.Location))
            {
                isInMainMenu = true;
                world.switchPause();
            }
        }
    }
}
