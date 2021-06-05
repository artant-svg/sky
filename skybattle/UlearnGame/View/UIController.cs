using System.Drawing;
using System.Windows.Forms;
using UlearnGame.Models;

namespace UlearnGame.Controllers
{
    public class UIController
    {
        private readonly Player player;
        private readonly EnemyController enemyController;
        private readonly Label healthLabel;
        private readonly Label armorLabel;
        private readonly Label waveLabel;
        private readonly Label countofEnemy;

        public int Wave { get; set; }

        public int WaveTime { get; set; }

        public UIController(Form form, WaveController waveController, EnemyController enemyController, Player player)
        {
            this.player = player;
            this.enemyController = enemyController;
            Wave = waveController.Wave;

            healthLabel = new Label
            {
                Text = $"Health:{player.Health}",
                ForeColor = Color.Purple,
                Font = new Font(FontFamily.GenericSansSerif, 12),
                Dock = DockStyle.Fill
            };
            armorLabel = new Label
            {
                Text = $"Armor:{player.Armor}",
                ForeColor = Color.Purple,
                Dock = DockStyle.Fill,
                Font = new Font(FontFamily.GenericSansSerif, 12)
            };
            waveLabel = new Label
            {
                Text = $"Wave:{waveController.Wave}",
                ForeColor = Color.Purple,
                Dock = DockStyle.Fill,
                Font = new Font(FontFamily.GenericSansSerif, 12)
            };
            countofEnemy = new Label
            {
                Text = $"Count of enemies:{ enemyController.CountOfEnemies - enemyController.DeadCount}",
                ForeColor = Color.Purple,
                Dock = DockStyle.Fill,
                Font = new Font(FontFamily.GenericSansSerif, 12)
            };
            var tableLayout = new TableLayoutPanel
            {
                Size = new Size(form.ClientSize.Width, 40),
            };
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tableLayout.Controls.Add(healthLabel, 0, 0);
            tableLayout.Controls.Add(armorLabel, 1, 0);
            tableLayout.Controls.Add(waveLabel, 2, 0);
            tableLayout.Controls.Add(countofEnemy, 3, 0);

            form.Controls.Add(tableLayout);
        }

        public void Update()
        {
            healthLabel.Text = $"Health:{player.Health}";
            armorLabel.Text = $"Armor:{player.Armor}";
            waveLabel.Text = $"Wave:{Wave}";
            countofEnemy.Text = $"Count of enemies:{enemyController.CountOfEnemies - enemyController.DeadCount}";
        }

    }

}
