using System;
using System.Windows.Forms;
using System.Drawing;

namespace TutorooSystem
{
    public class AboutForm : Form
    {
        public AboutForm()
        {
            this.Text = "System | About Tutoroo";
            this.Size = new Size(420, 520);
            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            Label lblTitle = new Label { 
                Text = "System Information", 
                Font = new Font("Segoe UI", 16, FontStyle.Bold), 
                Dock = DockStyle.Top, Height = 80, 
                TextAlign = ContentAlignment.MiddleCenter 
            };

            Label lblIcon = new Label { 
                Text = "🎓", 
                Font = new Font("Segoe UI", 60), 
                Dock = DockStyle.Top, Height = 140, 
                TextAlign = ContentAlignment.MiddleCenter 
            };

            Label lblInfo = new Label {
                Text = "PROJECT: Tutoroo System\nVERSION: 1.0.0 (Stable)\nCOLLEGE: Bicol University\nCOURSE: BS Information Systems\n\nThis system utilizes Excel Automation\nto manage high-quality anime records.",
                Font = new Font("Segoe UI", 10), 
                ForeColor = Color.FromArgb(75, 85, 99), 
                Dock = DockStyle.Fill, 
                TextAlign = ContentAlignment.TopCenter, 
                Padding = new Padding(30)
            };

            this.Controls.AddRange(new Control[] { lblInfo, lblIcon, lblTitle });
        }
    }
}