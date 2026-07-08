# SmartLocker System - Final Project Package

A comprehensive .NET-based locker management system designed for on-premise deployment on Raspberry Pi with touchscreen kiosk interface.

## Quick Start

### Local Development (Windows/Linux/macOS)

```bash
cd SmartLocker.Web
dotnet run
```

Access the application at `https://localhost:5001` or `http://localhost:5000`

**Test Credentials:**
- **Admin:** `admin` / `admin123`
- **Staff:** `staff` / `staff123`

### Raspberry Pi Deployment

```bash
# On development machine
./scripts/publish-linux-arm64.sh

# Transfer publish directory to Raspberry Pi, then on the Pi:
sudo ./scripts/install-raspberrypi.sh
sudo systemctl start smartlocker
```

Access at `http://localhost:5000` or `http://smartlocker.local`

---

## Project Structure

```
SmartLocker/
├── SmartLocker.Web/                    # Main ASP.NET Core application
│   ├── Models/                         # EF Core entity models
│   │   ├── User.cs
│   │   ├── Role.cs
│   │   ├── Item.cs
│   │   ├── Locker.cs
│   │   ├── Request.cs
│   │   ├── Borrow.cs
│   │   ├── LockerAccessToken.cs
│   │   ├── SystemLog.cs
│   │   └── [Status enums]
│   ├── Data/                           # Database context and initialization
│   │   ├── SmartLockerDbContext.cs
│   │   └── DbInitializer.cs
│   ├── Services/                       # Business logic layer
│   │   ├── AuthenticationService.cs
│   │   ├── UserService.cs
│   │   ├── ItemService.cs
│   │   ├── BorrowService.cs
│   │   ├── RequestService.cs
│   │   ├── LockerService.cs
│   │   ├── LogService.cs
│   │   ├── QrCodeService.cs
│   │   └── LockerHardwareService.cs    # Hardware abstraction
│   ├── Pages/                          # Razor Pages UI
│   │   ├── Auth/
│   │   │   ├── Login.cshtml
│   │   │   └── Logout.cshtml.cs
│   │   ├── Admin/
│   │   │   ├── Dashboard.cshtml
│   │   │   ├── Users.cshtml
│   │   │   ├── Items.cshtml
│   │   │   ├── Requests.cshtml
│   │   │   ├── Borrows.cshtml
│   │   │   ├── Overdue.cshtml
│   │   │   └── Logs.cshtml
│   │   ├── Staff/
│   │   │   ├── Dashboard.cshtml
│   │   │   ├── SearchItems.cshtml
│   │   │   ├── RequestItem.cshtml
│   │   │   └── Borrows.cshtml
│   │   ├── Locker/
│   │   │   ├── Index.cshtml
│   │   │   ├── Unlock.cshtml
│   │   │   └── Return.cshtml
│   │   ├── Shared/
│   │   │   └── _Layout.cshtml
│   │   ├── Health.cshtml.cs
│   │   └── Unlock.cshtml.cs
│   ├── Migrations/                     # EF Core database migrations
│   ├── Program.cs                      # Dependency injection and configuration
│   ├── appsettings.json                # Development configuration
│   ├── appsettings.Production.json     # Production configuration
│   ├── SmartLocker.Web.csproj
│   └── smartlocker.db                  # SQLite database (development)
├── scripts/                            # Deployment scripts
│   ├── publish-linux-arm64.sh          # Publish for Raspberry Pi
│   ├── install-raspberrypi.sh          # Installation script
│   ├── smartlocker.service             # systemd service file
│   └── chromium-kiosk-autostart.sh     # Kiosk mode setup
├── docs/                               # Documentation
│   ├── deployment-raspberrypi.md       # Deployment guide
│   ├── nginx-smartlocker.conf          # Optional Nginx config
│   ├── mdns-setup.md                   # mDNS hostname setup
│   ├── architecture.md                 # System architecture
│   ├── database-schema.md              # Database schema
│   ├── test-plan.md                    # Testing strategy
│   ├── uat-checklist.md                # UAT checklist
│   └── security-review.md              # Security controls
├── PHASE1_COMPLETION_REPORT.md         # Phase 1 planning
├── PHASE2_COMPLETION_REPORT.md         # Phase 2 backend foundation
├── PHASE3_COMPLETION_REPORT.md         # Phase 3 core UI
├── PHASE4_COMPLETION_REPORT.md         # Phase 4 business logic
├── PHASE5_COMPLETION_REPORT.md         # Phase 5 QR unlock
├── PHASE6_COMPLETION_REPORT.md         # Phase 6 deployment
├── PHASE7_FINAL_REVIEW_REPORT.md       # Phase 7 final review
└── README.md                           # This file
```

---

## System Requirements

### Local Development
- .NET 8.0 SDK
- SQLite 3.x
- Browser (Chrome, Firefox, Safari, Edge)

### Raspberry Pi Deployment
- Raspberry Pi 4 or 5 (2GB RAM minimum, 4GB+ recommended)
- Raspberry Pi OS 64-bit
- Internet connection (for initial setup)

---

## Exact Commands

### 1. Local Run Command

```bash
cd SmartLocker/SmartLocker.Web
dotnet run
```

**Output:** Application starts on `https://localhost:5001` (HTTPS) and `http://localhost:5000` (HTTP)

### 2. Raspberry Pi Publish Command

```bash
cd SmartLocker
./scripts/publish-linux-arm64.sh
```

**Output:** Creates `publish/` directory with linux-arm64 binaries ready for deployment

### 3. Raspberry Pi Installation Command

```bash
# On Raspberry Pi, after transferring publish directory:
cd /path/to/publish
sudo ../scripts/install-raspberrypi.sh
```

**Output:** Installs dependencies, creates directories, sets up systemd service

### 4. systemd Start Command

```bash
sudo systemctl start smartlocker
sudo systemctl status smartlocker
```

**Verify:** Service should show "active (running)"

---

## Demo Login Credentials

| Role | Username | Password | Access |
|------|----------|----------|--------|
| Admin | `admin` | `admin123` | All admin features, user/item management |
| Staff | `staff` | `staff123` | Search items, submit requests, view borrows |
| Locker | N/A | N/A | Public kiosk interface at `/Locker` |

**Important:** Change these credentials immediately in production.

---

## Demo Flow (10-15 minutes)

### Setup (Before Demo)
1. Ensure application is running: `dotnet run`
2. Access `https://localhost:5001`
3. Database is pre-populated with test data

### Demo Steps

#### Step 1: Admin Login & Create User (2 min)
1. Click "Login"
2. Enter: `admin` / `admin123`
3. Navigate to "Manage Users"
4. Click "Create User"
5. Fill in: Username, Email, Password, Role (Staff)
6. Click "Create"
7. **Show:** User appears in list

#### Step 2: Admin Create Item & Assign Locker (2 min)
1. Navigate to "Manage Items"
2. Click "Create Item"
3. Fill in: Name, Category, Description
4. Select a Locker from dropdown
5. Click "Create"
6. **Show:** Item appears with locker assignment

#### Step 3: Staff Login & Search Item (2 min)
1. Logout (top right)
2. Click "Login"
3. Enter: `staff` / `staff123`
4. Navigate to "Search Items"
5. Browse available items
6. **Show:** Can see items, categories, availability

#### Step 4: Staff Request Item (2 min)
1. Click on an item
2. Click "Request Item"
3. Enter justification
4. Click "Submit Request"
5. **Show:** Request submitted, confirmation message

#### Step 5: Admin Approve Request & Generate QR (2 min)
1. Logout, login as admin
2. Navigate to "Manage Requests"
3. Find the pending request
4. Click "Approve"
5. **Show:** Request approved, Borrow created, QR code generated

#### Step 6: Locker QR Unlock (2 min)
1. Navigate to `/Locker` (kiosk interface)
2. Click "Unlock with QR"
3. Paste QR token (or manually enter token)
4. Click "Unlock"
5. **Show:** Success message, locker unlocked (mock), token marked used

#### Step 7: Try Same QR Again (1 min)
1. Try to unlock with same token again
2. **Show:** Error message "Token already used"

#### Step 8: Return Item (2 min)
1. Logout, login as staff
2. Navigate to "My Borrows"
3. Find active borrow
4. Click "Return"
5. **Show:** Borrow returned, item available again, locker available

#### Step 9: View Overdue (1 min)
1. Logout, login as admin
2. Navigate to "View Overdue"
3. **Show:** List of overdue items (if any)

#### Step 10: View Logs (1 min)
1. Navigate to "View Logs"
2. **Show:** Audit trail of all actions

---

## Known Limitations

1. **Timezone:** System uses UTC for all timestamps. Configure timezone in deployment as needed.

2. **QR Distribution:** QR codes are generated but must be manually distributed or displayed. No automatic email/SMS notifications.

3. **GPIO Hardware:** Requires physical relay wiring and proper isolation (optocouplers). GPIO mode is optional; mock mode works without hardware.

4. **Network Scope:** Designed for local LAN/WiFi only. Not intended for internet-facing deployment.

5. **Single Location:** Supports one physical location. Multi-site deployments require separate instances.

6. **Authentication:** Uses cookie-based authentication. No OAuth/LDAP integration.

7. **Concurrency:** Designed for small teams (< 100 concurrent users). Not tested for high-concurrency scenarios.

8. **Backup:** No built-in backup mechanism. SQLite database must be backed up manually.

---

## Testing

### Run Tests
```bash
cd SmartLocker.Web
dotnet test
```

### Manual Testing Checklist
See `docs/uat-checklist.md` for comprehensive UAT scenarios.

### Test Data
Pre-populated on startup:
- Admin user: `admin` / `admin123`
- Staff user: `staff` / `staff123`
- 5 sample lockers
- 10 sample items
- 3 sample categories

---

## Security Notes

- **Passwords:** Hashed with BCrypt, never stored in plaintext
- **Authorization:** Role-based access control (Admin, Staff)
- **QR Tokens:** Cryptographically secure, 10-minute expiry, single-use
- **Database:** SQLite with file-level permissions (production: `/var/lib/smartlocker/smartlocker.db`)
- **Logs:** All actions logged to SystemLog table for audit trail
- **HTTPS:** Recommended for production (optional Nginx setup provided)

See `docs/security-review.md` for detailed security controls.

---

## Deployment

### Local Development
```bash
dotnet run
```

### Docker (Optional)
```bash
docker build -t smartlocker .
docker run -p 5000:5000 smartlocker
```

### Raspberry Pi
See `docs/deployment-raspberrypi.md` for step-by-step instructions.

### Nginx Reverse Proxy (Optional)
See `docs/nginx-smartlocker.conf` for configuration.

### mDNS Hostname (Optional)
See `docs/mdns-setup.md` to enable `http://smartlocker.local`

---

## Troubleshooting

### Application won't start
```bash
# Check .NET installation
dotnet --version

# Check port 5000/5001 availability
netstat -tulpn | grep 5000
```

### Database errors
```bash
# Recreate database
rm SmartLocker.Web/smartlocker.db
dotnet run  # Will recreate on startup
```

### Raspberry Pi service won't start
```bash
# Check service status
sudo systemctl status smartlocker

# View detailed logs
sudo journalctl -u smartlocker -f

# Restart service
sudo systemctl restart smartlocker
```

See `docs/deployment-raspberrypi.md` for more troubleshooting.

---

## Architecture

The system follows a clean architecture pattern:

- **Presentation Layer:** Razor Pages with responsive design
- **Business Logic Layer:** Service classes (RequestService, BorrowService, etc.)
- **Data Access Layer:** Entity Framework Core with SQLite
- **Hardware Abstraction:** ILockerHardwareService with Mock and GPIO implementations

See `docs/architecture.md` for detailed architecture documentation.

---

## Database Schema

15 tables supporting complete locker management workflow:

- **User Management:** User, Role
- **Inventory:** Item, Category, ItemStatus
- **Locker Management:** Locker, LockerStatus
- **Workflow:** Request, RequestStatus, Borrow, BorrowStatus
- **Security:** LockerAccessToken
- **Audit:** SystemLog

See `docs/database-schema.md` for detailed schema documentation.

---

## Use Cases Implemented

All 15 use cases from specification:

| UC | Name | Status |
|----|------|--------|
| UC-01 | Login | ✅ Complete |
| UC-02 | Dashboard | ✅ Complete |
| UC-03 | Manage User | ✅ Complete |
| UC-04 | Manage Item | ✅ Complete |
| UC-05 | View Logs | ✅ Complete |
| UC-06 | Manage Borrow | ✅ Complete |
| UC-07 | Return Item | ✅ Complete |
| UC-08 | View Overdue | ✅ Complete |
| UC-09 | Manage Request | ✅ Complete |
| UC-10 | Approve Request | ✅ Complete |
| UC-11 | Reject Request | ✅ Complete |
| UC-12 | Search Item | ✅ Complete |
| UC-13 | Request Item | ✅ Complete |
| UC-14 | Borrow Item | ✅ Complete |
| UC-15 | Extend Loan | ✅ Complete |

---

## Support & Documentation

- **Deployment Guide:** `docs/deployment-raspberrypi.md`
- **Architecture:** `docs/architecture.md`
- **Database Schema:** `docs/database-schema.md`
- **Security Review:** `docs/security-review.md`
- **UAT Checklist:** `docs/uat-checklist.md`
- **Test Plan:** `docs/test-plan.md`

---

## License

This project is provided as-is for educational and demonstration purposes.

---

## Version

**SmartLocker v1.0.0** - Final Release

**Build Date:** July 2026

**Status:** ✅ Ready for Demo and Deployment
