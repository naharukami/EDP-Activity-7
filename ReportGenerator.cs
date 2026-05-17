using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.IO;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;

namespace TutorooSystem
{
    public class ReportGenerator : Form
    {
        private DataGridView dgvReports  = new DataGridView();
        private DataGridView dgvAccounts = new DataGridView();
        private Button btnExportTrans    = new Button();
        private Button btnExportAccounts = new Button();

        private string currentSessionUser => $"{SessionManager.CurrentUsername} (Authorized Administrator)";

        public ReportGenerator()
        {
            this.Text          = "Katuwang | Report Generator";
            this.Size          = new Size(980, 700);
            this.BackColor     = Color.FromArgb(243, 244, 246);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblTitle = new Label {
                Text      = "TRIS Core Operations & Transaction Analytics",
                Font      = new Font("Segoe UI", 16, FontStyle.Bold),
                Dock      = DockStyle.Top,
                Height    = 70,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(31, 41, 55)
            };

            // ── Tab Control ───────────────────────────────────────────────────
            TabControl tabs = new TabControl {
                Location = new Point(20, 80),
                Size     = new Size(930, 560),
                Font     = new Font("Segoe UI", 10)
            };

            // ── TAB 1: Transaction Logs (existing feature) ────────────────────
            TabPage tabTrans = new TabPage("📋  Transaction Logs");

            dgvReports.Dock                  = DockStyle.Top;
            dgvReports.Height                = 400;
            dgvReports.BackgroundColor       = Color.White;
            dgvReports.BorderStyle           = BorderStyle.None;
            dgvReports.AutoSizeColumnsMode   = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReports.RowHeadersVisible     = false;
            dgvReports.AllowUserToAddRows    = false;

            btnExportTrans.Text      = "EXPORT TRANSACTION REPORT TO EXCEL";
            btnExportTrans.Dock      = DockStyle.Bottom;
            btnExportTrans.Height    = 50;
            btnExportTrans.BackColor = Color.FromArgb(16, 185, 129);
            btnExportTrans.ForeColor = Color.White;
            btnExportTrans.FlatStyle = FlatStyle.Flat;
            btnExportTrans.Font      = new Font("Segoe UI", 11, FontStyle.Bold);
            btnExportTrans.FlatAppearance.BorderSize = 0;
            btnExportTrans.Click    += (s, e) => ExportTransactionsToExcel();

            tabTrans.Controls.Add(btnExportTrans);
            tabTrans.Controls.Add(dgvReports);

            // ── TAB 2: Account List Report (admin only) ───────────────────────
            TabPage tabAccounts = new TabPage("👥  Account Status Report");

            dgvAccounts.Dock                = DockStyle.Top;
            dgvAccounts.Height              = 400;
            dgvAccounts.BackgroundColor     = Color.White;
            dgvAccounts.BorderStyle         = BorderStyle.None;
            dgvAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAccounts.RowHeadersVisible   = false;
            dgvAccounts.AllowUserToAddRows  = false;

            // Summary label — updated after data loads
            Label lblSummary = new Label {
                Name      = "lblSummary",
                Text      = "",
                Font      = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.FromArgb(75, 85, 99),
                Dock      = DockStyle.Bottom,
                Height    = 28,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding   = new Padding(5, 0, 0, 0)
            };

            btnExportAccounts.Text      = "EXPORT ACCOUNT STATUS REPORT TO EXCEL";
            btnExportAccounts.Dock      = DockStyle.Bottom;
            btnExportAccounts.Height    = 50;
            btnExportAccounts.BackColor = Color.FromArgb(29, 78, 216);
            btnExportAccounts.ForeColor = Color.White;
            btnExportAccounts.FlatStyle = FlatStyle.Flat;
            btnExportAccounts.Font      = new Font("Segoe UI", 11, FontStyle.Bold);
            btnExportAccounts.FlatAppearance.BorderSize = 0;
            btnExportAccounts.Click    += (s, e) => ExportAccountsToExcel();

            tabAccounts.Controls.Add(btnExportAccounts);
            tabAccounts.Controls.Add(lblSummary);
            tabAccounts.Controls.Add(dgvAccounts);

            // Only show account tab to admins
            tabs.TabPages.Add(tabTrans);
            if (SessionManager.CurrentUserRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                tabs.TabPages.Add(tabAccounts);

            this.Controls.AddRange(new Control[] { lblTitle, tabs });

            LoadTransactionData();
            if (SessionManager.CurrentUserRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                LoadAccountData(lblSummary);
        }

        // ── Load existing transaction data ────────────────────────────────────
        private void LoadTransactionData()
        {
            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    string query = @"SELECT tutor_name AS 'Volunteer Tutor', 
                                            student_name AS 'TRIS Beneficiary', 
                                            subject_area AS 'Subject Module', 
                                            hours_rendered AS 'Hours Contributed', 
                                            rating_score AS 'Performance Rating', 
                                            logged_by AS 'Logged By' 
                                     FROM system_transactions";
                    MySqlDataAdapter adp = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    dgvReports.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Transaction Load Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Load account list with active/inactive status ─────────────────────
        private void LoadAccountData(Label lblSummary)
        {
            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    string query = @"SELECT UserID      AS 'User ID', 
                                            Username    AS 'Username', 
                                            role        AS 'Role', 
                                            Status      AS 'Account Status', 
                                            JoinDate    AS 'Date Joined' 
                                     FROM users 
                                     ORDER BY Status ASC, Username ASC";
                    MySqlDataAdapter adp = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    dgvAccounts.DataSource = dt;

                    // Count active vs inactive for summary bar
                    int active   = 0;
                    int inactive = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["Account Status"]?.ToString()
                                .Equals("Active", StringComparison.OrdinalIgnoreCase) == true)
                            active++;
                        else
                            inactive++;
                    }
                    lblSummary.Text = $"  Total Accounts: {dt.Rows.Count}     ✅ Active: {active}     ❌ Inactive: {inactive}";

                    // Color-code the status column rows
                    dgvAccounts.DataBindingComplete += (s, e) => {
                        foreach (DataGridViewRow row in dgvAccounts.Rows)
                        {
                            string? status = row.Cells["Account Status"].Value?.ToString();
                            row.DefaultCellStyle.ForeColor = status?.Equals("Active", StringComparison.OrdinalIgnoreCase) == true
                                ? Color.FromArgb(22, 163, 74)
                                : Color.FromArgb(220, 38, 38);
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Account Load Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Export transaction log (original feature) ─────────────────────────
        private void ExportTransactionsToExcel()
        {
            try
            {
                int totalRows = dgvReports.Rows.Count;
                if (totalRows == 0)
                {
                    MessageBox.Show("No transaction logs available to export.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                               "Katuwang_TRIS_Operational_Report.xlsx");

                using (var workbook = new XLWorkbook())
                {
                    var ws1 = workbook.Worksheets.Add("Operational Records");

                    ws1.Cell(1, 1).Value = "KATUWANG INFORMATION SYSTEM";
                    ws1.Cell(1, 1).Style.Font.Bold = true;
                    ws1.Cell(1, 1).Style.Font.FontSize = 14;
                    ws1.Cell(1, 1).Style.Font.FontColor = XLColor.FromHtml("#065F46");
                    ws1.Cell(2, 1).Value = "Official Transaction Audit Log — Taysan Resettlement Integrated School (TRIS)";
                    ws1.Cell(2, 1).Style.Font.Italic = true;

                    for (int col = 0; col < dgvReports.Columns.Count; col++)
                    {
                        var cell = ws1.Cell(4, col + 1);
                        cell.Value = dgvReports.Columns[col].HeaderText;
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#10B981");
                        cell.Style.Font.FontColor = XLColor.White;
                    }

                    for (int row = 0; row < totalRows; row++)
                        for (int col = 0; col < dgvReports.Columns.Count; col++)
                        {
                            string cellValue = dgvReports[col, row].FormattedValue?.ToString() ?? "";
                            if (col == 3 || col == 4) { double.TryParse(cellValue, out double n); ws1.Cell(row + 5, col + 1).Value = n; }
                            else ws1.Cell(row + 5, col + 1).Value = cellValue;
                        }

                    int signRow = totalRows + 8;
                    ws1.Cell(signRow,     1).Value = "Certified Correct and Audited By:";
                    ws1.Cell(signRow,     1).Style.Font.Italic = true;
                    ws1.Cell(signRow + 2, 1).Value = currentSessionUser;
                    ws1.Cell(signRow + 2, 1).Style.Font.Bold = true;
                    ws1.Cell(signRow + 2, 1).Style.Font.Underline = XLFontUnderlineValues.Single;
                    ws1.Cell(signRow + 3, 1).Value = "Katuwang System Administrator Account";

                    var ws2 = workbook.Worksheets.Add("Visual Analytics");
                    ws2.Cell(1, 1).Value = "SYSTEM OPERATIONAL AUDIT METRICS";
                    ws2.Cell(1, 1).Style.Font.Bold = true;

                    ws1.Columns().AdjustToContents();
                    ws2.Columns().AdjustToContents();
                    workbook.SaveAs(filePath);
                }

                Process.Start(new ProcessStartInfo { FileName = filePath, UseShellExecute = true });
                MessageBox.Show("Transaction report saved to Desktop!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Export account status report (admin only) ─────────────────────────
        private void ExportAccountsToExcel()
        {
            try
            {
                int totalRows = dgvAccounts.Rows.Count;
                if (totalRows == 0)
                {
                    MessageBox.Show("No account records to export.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                               "Katuwang_Account_Status_Report.xlsx");

                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("Account Status Report");

                    // ── Report header ─────────────────────────────────────────
                    ws.Cell(1, 1).Value = "KATUWANG INFORMATION SYSTEM";
                    ws.Cell(1, 1).Style.Font.Bold = true;
                    ws.Cell(1, 1).Style.Font.FontSize = 14;
                    ws.Cell(1, 1).Style.Font.FontColor = XLColor.FromHtml("#1D4ED8");

                    ws.Cell(2, 1).Value = "User Account Status Report — Generated by Administrator";
                    ws.Cell(2, 1).Style.Font.Italic = true;

                    ws.Cell(3, 1).Value = $"Date Generated: {DateTime.Now:MMMM dd, yyyy  hh:mm tt}";
                    ws.Cell(3, 1).Style.Font.FontColor = XLColor.Gray;

                    // ── Column headers ────────────────────────────────────────
                    string[] headers = { "User ID", "Username", "Role", "Account Status", "Date Joined" };
                    for (int col = 0; col < headers.Length; col++)
                    {
                        var cell = ws.Cell(5, col + 1);
                        cell.Value = headers[col];
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1D4ED8");
                        cell.Style.Font.FontColor = XLColor.White;
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    }

                    // ── Data rows with color-coded status ─────────────────────
                    int activeCount = 0, inactiveCount = 0;
                    for (int row = 0; row < totalRows; row++)
                    {
                        for (int col = 0; col < dgvAccounts.Columns.Count; col++)
                        {
                            string val = dgvAccounts[col, row].FormattedValue?.ToString() ?? "";
                            var cell = ws.Cell(row + 6, col + 1);
                            cell.Value = val;

                            // Color status cell
                            if (dgvAccounts.Columns[col].HeaderText == "Account Status")
                            {
                                if (val.Equals("Active", StringComparison.OrdinalIgnoreCase))
                                {
                                    cell.Style.Font.FontColor = XLColor.FromHtml("#166534");
                                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#DCFCE7");
                                    activeCount++;
                                }
                                else
                                {
                                    cell.Style.Font.FontColor = XLColor.FromHtml("#991B1B");
                                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#FEE2E2");
                                    inactiveCount++;
                                }
                                cell.Style.Font.Bold = true;
                                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            }
                        }
                    }

                    // ── Summary block ─────────────────────────────────────────
                    int sumRow = totalRows + 8;
                    ws.Cell(sumRow,     1).Value = "ACCOUNT SUMMARY";
                    ws.Cell(sumRow,     1).Style.Font.Bold = true;
                    ws.Cell(sumRow + 1, 1).Value = $"Total Accounts : {totalRows}";
                    ws.Cell(sumRow + 2, 1).Value = $"Active          : {activeCount}";
                    ws.Cell(sumRow + 2, 1).Style.Font.FontColor = XLColor.FromHtml("#166534");
                    ws.Cell(sumRow + 3, 1).Value = $"Inactive        : {inactiveCount}";
                    ws.Cell(sumRow + 3, 1).Style.Font.FontColor = XLColor.FromHtml("#991B1B");

                    // ── Signature ─────────────────────────────────────────────
                    ws.Cell(sumRow + 6, 1).Value = "Report Generated By:";
                    ws.Cell(sumRow + 6, 1).Style.Font.Italic = true;
                    ws.Cell(sumRow + 7, 1).Value = currentSessionUser;
                    ws.Cell(sumRow + 7, 1).Style.Font.Bold = true;
                    ws.Cell(sumRow + 7, 1).Style.Font.Underline = XLFontUnderlineValues.Single;
                    ws.Cell(sumRow + 8, 1).Value = "Katuwang System Administrator Account";

                    ws.Columns().AdjustToContents();
                    workbook.SaveAs(filePath);
                }

                Process.Start(new ProcessStartInfo { FileName = filePath, UseShellExecute = true });
                MessageBox.Show("Account status report saved to Desktop!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}