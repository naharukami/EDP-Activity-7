using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using MySql.Data.MySqlClient;

namespace TutorooSystem
{
    // ADMIN ONLY — full account list with active/inactive toggling
    public class UserManagementForm : Form
    {
        private DataGridView dgvUsers    = new DataGridView();
        private TextBox      txtSearch   = new TextBox();
        private TextBox      txtUsername = new TextBox();
        private ComboBox     cmbStatus   = new ComboBox();

        public UserManagementForm()
        {
            this.Text          = "Katuwang | Admin — User Management";
            this.Size          = new Size(1000, 650);
            this.BackColor     = Color.FromArgb(243, 244, 246);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Search
            txtSearch.Location        = new Point(30, 30);
            txtSearch.Width           = 350;
            txtSearch.Font            = new Font("Segoe UI", 11);
            txtSearch.PlaceholderText = "🔍 Search by Username...";
            txtSearch.TextChanged    += (s, e) => LoadUsers(txtSearch.Text);

            // Grid
            dgvUsers.Location            = new Point(30, 80);
            dgvUsers.Size                = new Size(600, 500);
            dgvUsers.BackgroundColor     = Color.White;
            dgvUsers.BorderStyle         = BorderStyle.None;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.AllowUserToAddRows  = false;
            dgvUsers.SelectionMode       = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.RowHeadersVisible   = false;
            dgvUsers.CellClick          += (s, e) => {
                if (dgvUsers.CurrentRow != null)
                {
                    txtUsername.Text = dgvUsers.CurrentRow.Cells["Username"].Value?.ToString() ?? "";
                    cmbStatus.Text   = dgvUsers.CurrentRow.Cells["Status"].Value?.ToString()   ?? "";
                }
            };

            // Right panel
            Panel pnlManage = new Panel {
                Location  = new Point(660, 80),
                Size      = new Size(300, 500),
                BackColor = Color.White
            };

            Label lblHead  = new Label { Text = "Account Details", Font = new Font("Segoe UI", 14, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true };
            Label lblUser  = new Label { Text = "Username",        Location = new Point(20, 70), AutoSize = true, ForeColor = Color.Gray };

            txtUsername.Location = new Point(20, 95);
            txtUsername.Width    = 260;
            txtUsername.Font     = new Font("Segoe UI", 11);

            Label lblStatus = new Label { Text = "Account Status", Location = new Point(20, 150), AutoSize = true, ForeColor = Color.Gray };

            cmbStatus.Location      = new Point(20, 175);
            cmbStatus.Width         = 260;
            cmbStatus.Font          = new Font("Segoe UI", 11);
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Items.AddRange(new string[] { "Active", "Inactive" });

            Button btnAdd = new Button {
                Text      = "Add Account",
                Location  = new Point(20, 250),
                Size      = new Size(260, 45),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnAdd.Click += (s, e) =>
                ExecuteCommand("INSERT INTO users (Username, Password, Status) VALUES (@u, '1', @s)");

            Button btnUpdate = new Button {
                Text      = "Update Status",
                Location  = new Point(20, 310),
                Size      = new Size(260, 45),
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnUpdate.Click += (s, e) => {
                if (txtUsername.Text.Equals(SessionManager.CurrentUsername, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("You cannot change the status of your own account.", "Action Refused",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                ExecuteCommand("UPDATE users SET Status = @s WHERE Username = @u");
            };

            pnlManage.Controls.AddRange(new Control[] {
                lblHead, lblUser, txtUsername, lblStatus, cmbStatus, btnAdd, btnUpdate
            });

            this.Controls.Add(txtSearch);
            this.Controls.Add(dgvUsers);
            this.Controls.Add(pnlManage);

            LoadUsers();
        }

        private void LoadUsers(string filter = "")
        {
            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    string query = "SELECT Username, Status FROM users WHERE Username LIKE @f";
                    MySqlDataAdapter adp = new MySqlDataAdapter(query, conn);
                    adp.SelectCommand.Parameters.AddWithValue("@f", "%" + filter + "%");
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    dgvUsers.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show("Load Error: " + ex.Message); }
        }

        private void ExecuteCommand(string query)
        {
            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@u", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@s", cmbStatus.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account updated successfully.");
                    LoadUsers();
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }
    }
}