using System;
using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace TutorooSystem
{
    // PURPOSE: Forgot-password recovery — anyone can reset by verifying their username.
    // This is NOT the same as MyProfileForm (which is for a logged-in user changing their password).
    public class RecoveryForm : Form
    {
        private TextBox txtUser    = new TextBox();
        private TextBox txtNewPass = new TextBox();
        private TextBox txtConfirm = new TextBox();
        private Panel   pnlReset   = new Panel();

        public RecoveryForm()
        {
            this.Text            = "Katuwang | Account Security — Password Recovery";
            this.Size            = new Size(420, 560);
            this.BackColor       = Color.White;
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;

            // ── Header ────────────────────────────────────────────────────────
            Label lblTitle = new Label {
                Text      = "🔐  Password Recovery",
                Font      = new Font("Segoe UI", 18, FontStyle.Bold),
                Dock      = DockStyle.Top,
                Height    = 80,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblInstr = new Label {
                Text      = "Forgot your password? Enter your registered\nusername below to verify your account.",
                Font      = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Dock      = DockStyle.Top,
                Height    = 55,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // ── Username Verification ─────────────────────────────────────────
            txtUser.Location        = new Point(50, 145);
            txtUser.Width           = 310;
            txtUser.Font            = new Font("Segoe UI", 12);
            txtUser.TextAlign       = HorizontalAlignment.Center;
            txtUser.PlaceholderText = "Enter your username";

            Button btnVerify = new Button {
                Text      = "VERIFY ACCOUNT",
                Location  = new Point(50, 200),
                Size      = new Size(310, 50),
                BackColor = Color.FromArgb(31, 41, 55),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btnVerify.FlatAppearance.BorderSize = 0;
            btnVerify.Click += (s, e) => VerifyAccount();

            // ── Reset Panel (hidden until verified) ───────────────────────────
            pnlReset.Location = new Point(50, 265);
            pnlReset.Size     = new Size(310, 220);
            pnlReset.Visible  = false;

            Label lblNewPwd = new Label {
                Text     = "New Password",
                Font     = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(0, 0),
                AutoSize = true,
                ForeColor = Color.FromArgb(55, 65, 81)
            };

            txtNewPass.Location        = new Point(0, 22);
            txtNewPass.Width           = 310;
            txtNewPass.Font            = new Font("Segoe UI", 11);
            txtNewPass.PasswordChar    = '●';
            txtNewPass.PlaceholderText = "Enter new password";

            Label lblConfirm = new Label {
                Text      = "Confirm Password",
                Font      = new Font("Segoe UI", 9, FontStyle.Bold),
                Location  = new Point(0, 68),
                AutoSize  = true,
                ForeColor = Color.FromArgb(55, 65, 81)
            };

            txtConfirm.Location        = new Point(0, 90);
            txtConfirm.Width           = 310;
            txtConfirm.Font            = new Font("Segoe UI", 11);
            txtConfirm.PasswordChar    = '●';
            txtConfirm.PlaceholderText = "Re-enter new password";

            Button btnUpdate = new Button {
                Text      = "RESET PASSWORD",
                Location  = new Point(0, 155),
                Size      = new Size(310, 48),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.Click += (s, e) => UpdatePassword();

            pnlReset.Controls.AddRange(new Control[] {
                lblNewPwd, txtNewPass, lblConfirm, txtConfirm, btnUpdate
            });

            this.Controls.AddRange(new Control[] {
                lblTitle, lblInstr, txtUser, btnVerify, pnlReset
            });
        }

        private void VerifyAccount()
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text))
            {
                MessageBox.Show("Please enter a username.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = DatabaseConfig.GetConnection()) // already opened inside GetConnection()
                {
                    string query = "SELECT Status FROM users WHERE Username = @u";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@u", txtUser.Text.Trim());

                    object? result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        string status = result.ToString() ?? "";
                        if (status.Equals("Active", StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show("Account verified. Please enter your new password.", "Verified",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                            pnlReset.Visible = true;
                            txtUser.Enabled  = false; // lock username field during reset
                        }
                        else
                        {
                            MessageBox.Show("This account is inactive. Contact your administrator.",
                                            "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Username not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdatePassword()
        {
            string newPwd     = txtNewPass.Text;
            string confirmPwd = txtConfirm.Text;

            if (string.IsNullOrWhiteSpace(newPwd))
            {
                MessageBox.Show("Please enter a new password.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!newPwd.Equals(confirmPwd))
            {
                MessageBox.Show("Passwords do not match. Please try again.", "Mismatch",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirm.Clear();
                txtConfirm.Focus();
                return;
            }

            try
            {
                using (var conn = DatabaseConfig.GetConnection()) // already opened inside GetConnection()
                {
                    string query = "UPDATE users SET Password = @p WHERE Username = @u";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@p", newPwd);
                    cmd.Parameters.AddWithValue("@u", txtUser.Text.Trim());
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Password reset successfully! You may now log in with your new password.",
                                "Recovery Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}