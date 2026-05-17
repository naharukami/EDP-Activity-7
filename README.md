# Katuwang Information System
### EDP Activity 7 — Bicol University

A role-based desktop information system built with C# Windows Forms and MySQL for the Taysan Resettlement Integrated School (TRIS).

---

## ⚙️ Prerequisites

Before running this project, make sure you have the following installed on your computer:

| Tool | Download Link |
|---|---|
| .NET SDK (version 6.0 or higher) | https://dotnet.microsoft.com/en-us/download |
| MySQL Server (version 8.0 recommended) | https://dev.mysql.com/downloads/mysql/ |
| MySQL Workbench (optional, for DB setup) | https://dev.mysql.com/downloads/workbench/ |
| Git | https://git-scm.com/downloads |

> ✅ To check if .NET is already installed, open a terminal and run: `dotnet --version`  
> ✅ To check if MySQL is installed, run: `mysql --version`

---

## 📥 Step 1 — Clone the Repository

Open a terminal (Command Prompt, PowerShell, or Git Bash) and run:

```bash
git clone https://github.com/<your-username>/EDP-Activity-7.git
```

Then navigate into the project folder:

```bash
cd EDP-Activity-7
```

---

## 🗄️ Step 2 — Set Up the Database

The repository includes a ready-made SQL dump file (`act2.sql`) that creates all tables and loads all sample data automatically.

### Option A — Using MySQL Workbench (Recommended for beginners)

1. Open **MySQL Workbench** and connect to your local MySQL server
2. In the top menu, click **Server → Data Import**
3. Select **Import from Self-Contained File**
4. Click the `...` button and locate the `act2.sql` file inside the project folder
5. Under **Default Target Schema**, click **New** and type `act2`
6. Click **Start Import**
7. Wait for the import to complete — you should see a success message

### Option B — Using the Terminal

1. Open a terminal and log in to MySQL:

```bash
mysql -u root -p
```

2. Create the database:

```sql
CREATE DATABASE act2;
EXIT;
```

3. Import the SQL file (replace the path with where your project folder is):

```bash
mysql -u root -p act2 < act2.sql
```

---

## 📊 What the Database Contains

After importing `act2.sql`, your database will have the following tables and data:

### `users` — Login accounts
| Username | Password | Role | Status |
|---|---|---|---|
| Jessica | Dannica2 | User | Active |
| nori | 2 | Admin | Active |
| GokuFan | password123 | User | Active |
| ZoroLost | password123 | User | Inactive |
| *(and more...)* | | | |

> 💡 Use **`nori`** with password **`2`** to log in as Admin.  
> 💡 Use **`Jessica`** with password **`Dannica2`** to log in as a regular User.

### `system_transactions` — Tutor session logs
Contains 10 sample tutoring records used by the Report Generator.

### `animes` & `studios`
Sample reference tables included in the database.

### `watchlists`
User anime watchlist table (empty by default).

---

## 🔌 Step 3 — Configure the Database Connection

Open `DatabaseConfig.cs` in the project folder and find this line:

```csharp
private static string connString = "server=127.0.0.1;port=3306;user=root;password=;database=act2;";
```

Update it to match your MySQL credentials:

| Field | What to change |
|---|---|
| `server` | Leave as `127.0.0.1` if MySQL is on your local machine |
| `port` | Leave as `3306` unless you changed it during MySQL install |
| `user` | Your MySQL username (usually `root`) |
| `password` | Your MySQL password (leave empty `password=;` if none) |
| `database` | Leave as `act2` |

**Example** — if your MySQL password is `12345`:
```csharp
private static string connString = "server=127.0.0.1;port=3306;user=root;password=12345;database=act2;";
```

---

## 📦 Step 4 — Restore NuGet Packages

Run this command inside the project folder to install the required libraries:

```bash
dotnet restore
```

This installs:
- `MySql.Data` — MySQL database connection
- `ClosedXML` — Excel report export

---

## ▶️ Step 5 — Run the Application

```bash
dotnet run
```

The login window will appear. Use one of the accounts below to get started.

---

## 🔑 Login Accounts

| Username | Password | Role | Access |
|---|---|---|---|
| `nori` | `2` | Admin | Full access — User Management, Reports, Account control |
| `Jessica` | `Dannica2` | User | My Profile, Account Security |
| `GokuFan` | `password123` | User | My Profile, Account Security |

> ⚠️ Accounts marked **Inactive** (e.g. ZoroLost, SaitamaOne) cannot log in until an Admin reactivates them.

---

## ❗ Common Errors & Fixes

| Error | Cause | Fix |
|---|---|---|
| `Connection must be valid and open` | MySQL service is not running | Start MySQL: run `net start mysql` in terminal, or start it from Windows Services |
| `Unable to connect to any MySQL hosts` | Wrong credentials in `DatabaseConfig.cs` | Double-check your username and password |
| `Unknown database 'act2'` | SQL file was not imported | Re-run Step 2 |
| `Table doesn't exist` | Incomplete import | Re-import `act2.sql` fully |
| `dotnet: command not found` | .NET SDK not installed | Download from https://dotnet.microsoft.com/en-us/download |
| `Access denied for user 'root'` | Wrong MySQL password | Update the `password=` field in `DatabaseConfig.cs` |

---

## 📁 Project File Overview

| File | Purpose |
|---|---|
| `act2.sql` | **Database dump — import this first** |
| `DatabaseConfig.cs` | MySQL connection — **edit this for your setup** |
| `LoginForm.cs` | Login screen |
| `Dashboard.cs` | Main menu after login |
| `SessionManager.cs` | Stores logged-in user session |
| `MyProfileForm.cs` | Profile viewer for regular users |
| `UserManagementForm.cs` | Account management for admins |
| `RecoveryForm.cs` | Password reset / recovery |
| `ReportGenerator.cs` | Excel report export |
| `AccountManagement.cs` | Admin account toggle control |
| `AboutForm.cs` | System info page |

---

## 🤝 Collaboration (Activity 7 Steps)

| Step | Action |
|---|---|
| 1 | Created repo under BU email account |
| 2 | Pushed Activity 6 source files including `act2.sql` |
| 3 | Added collaborator via second email |
| 4 | Collaborator cloned repo and created a branch |
| 5 | Collaborator pushed changes on branch |
| 6 | Reviewed and merged branch into main via BU email |

---

## 👨‍💻 Author

**Noriel** — Bicol University  
*EDP (Event-Driven Programming) — Activity 7*
