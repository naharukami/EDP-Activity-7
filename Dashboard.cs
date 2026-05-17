using System;
using System.Windows.Forms;
using System.Drawing;

namespace TutorooSystem
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            this.Text = "Katuwang | Dashboard";
            this.Size = new Size(1100, 700);
            this.BackColor = Color.FromArgb(243, 244, 246);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Sidebar Panel (Dark Navy)
            Panel sidebar = new Panel { Dock = DockStyle.Left, Width = 230, BackColor = Color.FromArgb(31, 41, 55) };
            Label logo = new Label {
                Text = "KATUWANG",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Dock = DockStyle.Top, Height = 100,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Button btnLogout = new Button {
                Text = "Log Out",
                Dock = DockStyle.Bottom,
                Height = 60,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10)
            };
            btnLogout.Click += (s, e) => Application.Exit();
            sidebar.Controls.AddRange(new Control[] { logo, btnLogout });

            // Top Header Panel
            Panel header = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.White };
            Label welcome = new Label {
                Text = $"Welcome back, {SessionManager.CurrentUsername}!",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(30, 30),
                AutoSize = true
            };
            // Show current role as a subtle badge next to the welcome label
            Label roleBadge = new Label {
                Text = SessionManager.CurrentUserRole.ToUpper(),
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = SessionManager.CurrentUserRole.Equals("Admin", StringComparison.OrdinalIgnoreCase)
                            ? Color.FromArgb(220, 38, 38)   // red for admin
                            : Color.FromArgb(59, 130, 246), // blue for user
                Location = new Point(30, 65),
                AutoSize = true,
                Padding = new Padding(6, 2, 6, 2)
            };
            header.Controls.AddRange(new Control[] { welcome, roleBadge });

            // Centered Tiles Navigation
            FlowLayoutPanel cardArea = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                Padding = new Padding(40),
                AutoScroll = true
            };

            bool isAdmin = SessionManager.CurrentUserRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);

            // Tile 1: Role-based User Management
            if (isAdmin)
            {
                // Admin sees full account management (active / inactive control)
                cardArea.Controls.Add(CreateActionCard("User Management", "👥", "Manage All Accounts & Status", () => {
                    new UserManagementForm().Show();
                }));
            }
            else
            {
                // Regular user sees only their own profile
                cardArea.Controls.Add(CreateActionCard("My Profile", "👤", "View & Update Your Account", () => {
                    new MyProfileForm().Show();
                }));
            }

            // Tile 2: Password Recovery
            cardArea.Controls.Add(CreateActionCard("Account Security", "🔐", "Verify & Reset Passwords", () => {
                new RecoveryForm().Show();
            }));

            // Tile 3: Report Generator
            cardArea.Controls.Add(CreateActionCard("Generate Reports", "📊", "Export Data to Excel", () => {
                new ReportGenerator().Show();
            }));

            // Tile 4: About System
            cardArea.Controls.Add(CreateActionCard("About System", "ℹ️", "Bicol University Project Info", () => {
                new AboutForm().Show();
            }));

            this.Controls.AddRange(new Control[] { cardArea, header, sidebar });
        }

        private Button CreateActionCard(string title, string icon, string subtext, Action onClick)
        {
            Button btn = new Button {
                Size = new Size(280, 180),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 0, 30, 30),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Color.FromArgb(229, 231, 235);

            Label lblIcon = new Label {
                Text = icon,
                Font = new Font("Segoe UI", 32),
                Dock = DockStyle.Top, Height = 70,
                TextAlign = ContentAlignment.BottomCenter,
                Enabled = false
            };
            Label lblTitle = new Label {
                Text = title,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Dock = DockStyle.Top, Height = 35,
                TextAlign = ContentAlignment.MiddleCenter,
                Enabled = false
            };
            Label lblSub = new Label {
                Text = subtext,
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopCenter,
                Enabled = false
            };

            btn.Controls.AddRange(new Control[] { lblSub, lblTitle, lblIcon });
            btn.Click += (s, e) => onClick();
            return btn;
        }
    }
}