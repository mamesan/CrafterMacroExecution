using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Advanced_Combat_Tracker;

namespace CrafterMacroExecution
{
    public partial class CrafterMacroExecution : UserControl, IActPluginV1
    {
        public CrafterMacroExecution()
        {
            InitializeComponent();
        }

        public void DeInitPlugin()
        {
        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            //lbStatus = pluginStatusText;   // Hand the status label's reference to our local var
            pluginScreenSpace.Controls.Add(this);   // Add this UserControl to the tab ACT provides
            this.Dock = DockStyle.Fill; // Expand the UserControl to fill the tab's client space
                                        // MultiProject.BasePlugin.xmlSettings = new SettingsSerializer(this); // Create a new settings serializer and pass it this instance

            pluginScreenSpace.Text = "テスト用";
            pluginStatusText.Text = "テスト用Starts";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
