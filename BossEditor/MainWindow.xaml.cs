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

namespace BossEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            listEnemies.Items.Add("Slime1");
            listEnemies.Items.Add("Slime2");
            listEnemies.Items.Add("Slime3");
            listEnemies.Items.Add("Slime4");
            listEnemies.Items.Add("Zombie");
            listEnemies.Items.Add("Skeleton");
            listEnemies.Items.Add("Ghost");
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            /*
             <?xml version="1.0" encoding="utf-8" ?>
<XnaContent>
  <!-- TODO: replace this Asset with your own XML asset data. -->
  <Asset Type="MageDefenderDeluxe.LevelSettings.BossBehaviour">
	  <BossName>Gooey King</BossName>
	  <MoveStyle>Sin</MoveStyle>
	  <SpitEnemies>true</SpitEnemies>
	  <SpitWhatEnemy>Slime1</SpitWhatEnemy>
	  <Animated>false</Animated>
	  <ModelID>2</ModelID>
	  <Hp>1600</Hp>
	  <Radius>100</Radius>
	  <ParticleID>0</ParticleID>
	  <Loot>1000</Loot>
	  <Xp>400</Xp>
	  <Score>2000</Score>
  </Asset>
</XnaContent>
             */


            StringBuilder sn = new StringBuilder();
            sn.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sn.AppendLine("<XnaContent>");
            sn.AppendLine("\t<Asset Type=\"MageDefenderDeluxe.LevelSettings.BossBehaviour\">");
            sn.AppendLine("\t\t<BossName>"+tbDisplayName.Text+"</BossName>");
            sn.AppendLine("\t\t<MoveStyle>Sin</MoveStyle>");
            sn.AppendLine("\t\t<SpitEnemies>"+cbSpawner.IsChecked.Value.ToString().ToLower()+"</SpitEnemies>");
            sn.AppendLine("\t\t<SpitWhatEnemy>"+listEnemies.SelectedValue.ToString()+"</SpitWhatEnemy>");
            sn.AppendLine("\t\t<Animated>"+cbAnimated.IsChecked.Value.ToString().ToLower()+"</Animated>");
            sn.AppendLine("\t\t<ModelID>"+tbModelID.Text+"</ModelID>");
            sn.AppendLine("\t\t<Hp>"+tbHp.Text+"</Hp>");
            sn.AppendLine("\t\t<Radius>"+tbSize.Text+"</Radius>");
            sn.AppendLine("\t\t<ParticleID>"+tbParticleId.Text+"</ParticleID>");
            sn.AppendLine("\t\t<Loot>"+tbLoot.Text+"</Loot>");
            sn.AppendLine("\t\t<Xp>"+tbXp.Text+"</Xp>");
            sn.AppendLine("\t\t<Score>" + tbScore.Text + "</Score>");
            sn.AppendLine("\t</Asset>");
            sn.AppendLine("</XnaContent>");

            TextWriter tw = new StreamWriter(tbCodeName.Text + ".xml");
            tw.WriteLine(sn.ToString());
            tw.Close();

            MessageBox.Show("Save completed. " + "Boss " + tbCodeName.Text + ".xml can be found in the Boss Editor directory.");
            Clipboard.SetText(sn.ToString(), TextDataFormat.Text);

        }
    }
}
