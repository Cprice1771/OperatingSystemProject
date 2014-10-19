using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OperatingSystem;

namespace OperatingSystemFrontend
{
    
    public partial class OperatingSystemForm : Form
    {
        OperatingSystem.OperatingSystem os;
        public OperatingSystemForm()
        {
            InitializeComponent();
            
            fileNameTextBox.Text = Directory.GetCurrentDirectory() + "\\Input\\ugradPart1.txt";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Make sure all inputs are valid
            if (!string.IsNullOrEmpty(comboBoxLTS.Text) && !string.IsNullOrEmpty(textBoxRAMSize.Text) && !string.IsNullOrEmpty(textBoxCPUCount.Text))
            {
                LTSAlgorithm algorithm = (LTSAlgorithm)Enum.Parse(typeof(LTSAlgorithm), comboBoxLTS.Text);
                os = new OperatingSystem.OperatingSystem(algorithm, Int32.Parse(textBoxRAMSize.Text), Int32.Parse(textBoxCPUCount.Text));
                //Run the OS and show the output in the output box
                richTextBoxRAM.Text = os.Start(fileNameTextBox.Text);
            }
            else
                MessageBox.Show("You left an input empty!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        
        private void selectFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory();

            //Only update the text box if the user selected OK
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //Update the text box with the new file name
                fileNameTextBox.Text = dialog.FileName;
            }
        }
    }
}
