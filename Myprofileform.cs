using System;
using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace TutorooSystem
{
    // PURPOSE: Shows the logged-in user's own profile info (read-only).
    // Password changes are handled via the Account Security / RecoveryForm.
    public class MyProfileForm : Form
    {
        public MyProfileForm()
        {
            this.Text            = "Katuwang | My Profile";
            this.Size            = new Size(460, 620);
            this.BackColor       = Color.FromArgb(243, 244, 246);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;

            // ── Header ────────────────────────────────────────────────────────
            Panel pnlHeader = new Panel {
                Dock      = DockStyle.Top,
                Height    = 100,
                BackColor = Color.FromArgb(31, 41, 55)
            };

            Label lblTitle = new Label {
                Text      = "My Profile",
                Font      = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                Location  = new Point(30, 18),
                AutoSize  = true
            };
            Label lblSub = new Label {
                Text      = $"{SessionManager.CurrentUsername}  •  {SessionManager.CurrentUserRole}",
                Font      = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(156, 163, 175),
                Location  = new Point(30, 55),
                AutoSize  = true
            };
            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblSub });

            // ── Profile card ──────────────────────────────────────────────────
            Panel pnlCard = new Panel {
                Location  = new Point(30, 120),
                Size      = new Size(385, 370),
                BackColor = Color.White
            };
            pnlCard.Paint += (s, e) =>
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(229, 231, 235)),
                                         0, 0, pnlCard.Width - 1, pnlCard.Height - 1);

            LoadProfileRows(pnlCard);

            // ── Close button ──────────────────────────────────────────────────
            Button btnClose = new Button {
                Text      = "CLOSE",
                Location  = new Point(30, 505),
                Size      = new Size(385, 45),
                BackColor = Color.FromArgb(31, 41, 55),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { pnlHeader, pnlCard, btnClose });
        }

        private void LoadProfileRows(Panel pnlCard)
        {
            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    string query = "SELECT UserID, Username, JoinDate, Status, role FROM users WHERE Username = @u";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@u", SessionManager.CurrentUsername);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string userID   = reader["UserID"]?.ToString() ?? "—";
                            string username = reader["Username"]?.ToString() ?? "—";
                            string joinDate = reader["JoinDate"] != DBNull.Value
                                           ? Convert.ToDateTime(reader["JoinDate"]).ToString("MMMM dd, yyyy")
                                           : "—";
                            string status   = reader["Status"]?.ToString() ?? "—";
                            string role     = reader["role"]?.ToString()    ?? "—";

                            Color statusColor = status.Equals("Active", StringComparison.OrdinalIgnoreCase)
                                             ? Color.FromArgb(22, 163, 74)
                                             : Color.FromArgb(220, 38, 38);

                            AddProfileRow(pnlCard, "User ID",   userID,   0, Color.FromArgb(99, 102, 241));
                            AddProfileRow(pnlCard, "Username",  username, 1, Color.FromArgb(59, 130, 246));
                            AddProfileRow(pnlCard, "Join Date", joinDate, 2, Color.FromArgb(16, 185, 129));
                            AddProfileRow(pnlCard, "Status",    status,   3, statusColor);
                            AddProfileRow(pnlCard, "Role",      role,     4, Color.FromArgb(245, 158, 11));
                        }
                        else
                        {
                            pnlCard.Controls.Add(new Label {
                                Text      = "Profile data not found.",
                                ForeColor = Color.Red,
                                Location  = new Point(20, 20),
                                AutoSize  = true
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pnlCard.Controls.Add(new Label {
                    Text      = "Error loading profile: " + ex.Message,
                    ForeColor = Color.Red,
                    Location  = new Point(10, 10),
                    AutoSize  = true
                });
            }
        }

        private void AddProfileRow(Panel parent, string label, string value, int index, Color accentColor)
        {
            int y = index * 60 + 15;

            // Colored left accent bar
            parent.Controls.Add(new Panel {
                Location  = new Point(15, y),
                Size      = new Size(4, 40),
                BackColor = accentColor
            });

            parent.Controls.Add(new Label {
                Text      = label,
                Font      = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location  = new Point(28, y),
                Size      = new Size(340, 18)
            });

            parent.Controls.Add(new Label {
                Text      = value,
                Font      = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Location  = new Point(28, y + 19),
                Size      = new Size(340, 22)
            });
        }
    }
}