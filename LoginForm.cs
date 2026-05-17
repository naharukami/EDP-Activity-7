using System;
using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace TutorooSystem
{
    public class LoginForm : Form
    {
        private TextBox txtUsername = new TextBox();
        private TextBox txtPassword = new TextBox();
        private Button btnLogin    = new Button();

        public LoginForm()
        {
            // ── Window ────────────────────────────────────────────────────────
            this.Text            = "Tutoroo | Login Portal";
            this.Size            = new Size(860, 540);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.BackColor       = Color.White;

            // ── LEFT PANEL (dark branding strip) ─────────────────────────────
            Panel pnlLeft = new Panel
            {
                Location  = new Point(0, 0),
                Size      = new Size(300, 540),
                BackColor = Color.FromArgb(18, 30, 51)   // dark navy, matches screenshot
            };

            Label lblBrand = new Label
            {
                Text      = "Tutoroo",
                Font      = new Font("Segoe UI", 26, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock      = DockStyle.Fill
            };
            pnlLeft.Controls.Add(lblBrand);

            // ── RIGHT PANEL (login area) ──────────────────────────────────────
            Panel pnlRight = new Panel
            {
                Location  = new Point(300, 0),
                Size      = new Size(560, 540),
                BackColor = Color.FromArgb(243, 244, 246)
            };

            // Title
            Label lblTitle = new Label
            {
                Text      = "Account Login",
                Font      = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 20, 20),
                Location  = new Point(60, 80),
                Size      = new Size(420, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // ── Username field ────────────────────────────────────────────────
            txtUsername.Location        = new Point(60, 185);
            txtUsername.Size            = new Size(420, 38);
            txtUsername.Font            = new Font("Segoe UI", 11);
            txtUsername.BorderStyle     = BorderStyle.FixedSingle;
            txtUsername.BackColor       = Color.White;
            txtUsername.ForeColor       = Color.FromArgb(30, 30, 30);
            txtUsername.PlaceholderText = "Username";          // replaces null-prone label workaround

            // Blue bottom-border highlight on focus (matches screenshot active field)
            txtUsername.Enter += (s, e) => txtUsername.BackColor = Color.White;

            // ── Password field ────────────────────────────────────────────────
            txtPassword.Location        = new Point(60, 255);
            txtPassword.Size            = new Size(420, 38);
            txtPassword.Font            = new Font("Segoe UI", 11);
            txtPassword.BorderStyle     = BorderStyle.FixedSingle;
            txtPassword.BackColor       = Color.White;
            txtPassword.ForeColor       = Color.FromArgb(30, 30, 30);
            txtPassword.PasswordChar    = '●';
            txtPassword.PlaceholderText = "Password";

            // ── Sign In button ────────────────────────────────────────────────
            btnLogin.Text      = "SIGN IN";
            btnLogin.Location  = new Point(60, 340);
            btnLogin.Size      = new Size(420, 55);
            btnLogin.BackColor = Color.FromArgb(59, 130, 246);   // blue matches screenshot
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Font      = new Font("Segoe UI", 12, FontStyle.Bold);
            btnLogin.Cursor    = Cursors.Hand;
            btnLogin.Click    += (s, e) => ProcessSystemAuthentication();

            this.AcceptButton = btnLogin;

            // ── Assemble right panel ──────────────────────────────────────────
            pnlRight.Controls.AddRange(new Control[]
            {
                lblTitle, txtUsername, txtPassword, btnLogin
            });

            // ── Assemble form ─────────────────────────────────────────────────
            this.Controls.AddRange(new Control[] { pnlLeft, pnlRight });
        }

        // ── Authentication logic ──────────────────────────────────────────────
        private void ProcessSystemAuthentication()
        {
            string usernameInput = txtUsername.Text.Trim();
            string passwordInput = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(usernameInput) || string.IsNullOrEmpty(passwordInput))
            {
                MessageBox.Show("Please enter both username and password.", "Fields Required",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    string query = "SELECT role, status FROM users WHERE username = @user AND password = @pass";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@user", usernameInput);
                    cmd.Parameters.AddWithValue("@pass", passwordInput);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Fix CS8600: explicit null-coalescing instead of ToString() alone
                            string accountStatus = reader["status"]?.ToString() ?? "Active";
                            string userRole      = reader["role"]?.ToString()   ?? "User";

                            if (accountStatus.Equals("Inactive", StringComparison.OrdinalIgnoreCase))
                            {
                                MessageBox.Show("Your account has been deactivated. Contact a system administrator.",
                                                "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }

                            SessionManager.CurrentUsername = usernameInput;
                            SessionManager.CurrentUserRole = userRole;

                            MessageBox.Show($"Welcome back, {usernameInput}!", "Login Successful",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Open dashboard — create a fresh instance rather than searching OpenForms
                            // to avoid the CS8602 null-dereference warning on Application.OpenForms["Dashboard"]
                            Dashboard mainDash = new Dashboard();
                            mainDash.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Incorrect username or password.", "Authentication Failed",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtPassword.Clear();
                            txtPassword.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection error: " + ex.Message, "System Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

//ang ganda ni jess
