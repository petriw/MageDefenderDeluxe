using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace LevelEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            String enemiesSelected = "";

            if (cbSlime1.IsChecked.Value)
            {
                enemiesSelected += cbSlime1.Content + ",";
            }
            if (cbSlime2.IsChecked.Value)
            {
                enemiesSelected += cbSlime2.Content + ",";
            }
            if (cbSlime3.IsChecked.Value)
            {
                enemiesSelected += cbSlime3.Content + ",";
            }
            if (cbSlime4.IsChecked.Value)
            {
                enemiesSelected += cbSlime4.Content + ",";
            }
            if (cbZombie.IsChecked.Value)
            {
                enemiesSelected += cbZombie.Content + ",";
            }
            if (cbSkeleton.IsChecked.Value)
            {
                enemiesSelected += cbSkeleton.Content + ",";
            }
            if (cbGhost.IsChecked.Value)
            {
                enemiesSelected += cbGhost.Content + ",";
            }
            if (cbBoss1.IsChecked.Value)
            {
                enemiesSelected += cbBoss1.Content + ",";
            }

            enemiesSelected = enemiesSelected.Remove(enemiesSelected.Length-1, 1);

            StringBuilder sn = new StringBuilder();
            sn.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sn.AppendLine("<XnaContent>");
            sn.AppendLine("\t<Asset Type=\"MageDefenderDeluxe.LevelSettings.Level\">");
            sn.AppendLine("\t\t<LevelID>"+tbId.Text+"</LevelID>");
            sn.AppendLine("\t\t<SpawnEnemies>" + enemiesSelected + "</SpawnEnemies>");
            sn.AppendLine("\t\t<NumEnemies>"+tbNumEnemies.Text+"</NumEnemies>");
            sn.AppendLine("\t\t<IsBoss>"+cbIsBoss.IsChecked.Value.ToString().ToLower()+"</IsBoss>");
            sn.AppendLine("\t\t<BossCodeName>" + tbBossCodeFile.Text + "</BossCodeName>");
            sn.AppendLine("\t</Asset>");
            sn.AppendLine("</XnaContent>");

            TextWriter tw = new StreamWriter("Level"+tbId.Text+".xml");
            tw.WriteLine(sn.ToString());
            tw.Close();

            MessageBox.Show("Save completed. " + "Level" + tbId.Text + ".xml can be found in the Level Editor directory. It is also placed in your clipboard");
            Clipboard.SetText(sn.ToString(), TextDataFormat.Text);
        }

        private void cbIsBoss_Checked(object sender, RoutedEventArgs e)
        {
            cbBoss1.IsChecked = true;
            cbGhost.IsChecked = false;
            cbSkeleton.IsChecked = false;
            cbZombie.IsChecked = false;
            cbSlime1.IsChecked = false;
            cbSlime2.IsChecked = false;
            cbSlime3.IsChecked = false;
            cbSlime4.IsChecked = false;

            tbNumEnemies.Text = "1";
        }
    }
}
