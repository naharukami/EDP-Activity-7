using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using MySql.Data.MySqlClient;

namespace TutorooSystem
{
    public class AccountManagement : Form
    {
        private DataGridView dgvUsers = new DataGridView();
        private Button btnToggleStatus = new Button();

        public AccountManagement()
        {
            this.Text = "Katuwang | Administrative Account Management Control";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblTitle = new Label {
                Text = "User Status Control Center",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Dock = DockStyle.Top, Height = 60, TextAlign = ContentAlignment.MiddleCenter
            };

            dgvUsers.Location = new Point(30, 80);
            dgvUsers.Size = new Size(720, 280);
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.AllowUserToAddRows = false;

            btnToggleStatus.Text = "TOGGLE STATUS (ACTIVATE / DEACTIVATE)";
            btnToggleStatus.Location = new Point(250, 380);
            btnToggleStatus.Size = new Size(300, 45);
            btnToggleStatus.BackColor = Color.FromArgb(29, 78, 216); // Administrative Blue
            btnToggleStatus.ForeColor = Color.White;
            btnToggleStatus.FlatStyle = FlatStyle.Flat;
            btnToggleStatus.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnToggleStatus.Click += (s, e) => ToggleAccountStatus();

            this.Controls.AddRange(new Control[] { lblTitle, dgvUsers, btnToggleStatus });

            // SECURITY CHECK: Evaluate permission level before running queries
            VerifyAdministrativePrivileges();
        }

        private void VerifyAdministrativePrivileges()
        {
            // If the user role saved during login isn't 'Admin', lock them out immediately
            if (!SessionManager.CurrentUserRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Access Denied: You do not possess administrative privileges to modify account properties.",
                                "Security Violation", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                btnToggleStatus.Enabled = false;
                dgvUsers.Enabled = false;
                this.BeginInvoke(new Action(() => this.Close())); // Safely kill the window execution loop
                return;
            }
            LoadUserRecords();
        }

        private void LoadUserRecords()
        {
            using (var conn = DatabaseConfig.GetConnection())
            {
                // Pull username, role, and current status safely
                string query = "SELECT username AS 'Username', role AS 'System Role', status AS 'Account Status' FROM users";
                MySqlDataAdapter adp = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                dgvUsers.DataSource = dt;
            }
        }

        private void ToggleAccountStatus()
        {
            // Guard: ensure at least one row is actually selected before proceeding
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user account to toggle.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 1. Index into [0] to get a single DataGridViewRow, not the whole collection
            DataGridViewRow selectedRow = dgvUsers.SelectedRows[0];

            // 2. Now .Cells works correctly on the single row instance
            string targetUser = selectedRow.Cells["Username"].Value?.ToString() ?? "";
            string currentStatus = selectedRow.Cells["Account Status"].Value?.ToString() ?? "Active";

            // Prevent the active admin from accidentally locking themselves out of the system
            if (targetUser.Equals(SessionManager.CurrentUsername, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("You cannot deactivate your own active session account.", "Action Refused", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Determine the inverse state swap
            string newStatus = currentStatus.Equals("Active", StringComparison.OrdinalIgnoreCase) ? "Inactive" : "Active";

            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    string updateQuery = "UPDATE users SET status = @status WHERE username = @user";
                    MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@status", newStatus);
                    cmd.Parameters.AddWithValue("@user", targetUser);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show($"Account access status for '{targetUser}' has been successfully set to {newStatus}.", "Status Updated");
                LoadUserRecords(); // Refresh the grid view data elements
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to change account state: " + ex.Message, "Error");
            }
        }
    }
}