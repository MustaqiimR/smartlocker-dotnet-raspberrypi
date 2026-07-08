# SmartLocker Deployment Guide

Complete step-by-step instructions for deploying SmartLocker on Raspberry Pi.

## Prerequisites

- Raspberry Pi 4 or 5 (2GB RAM minimum, 4GB+ recommended)
- Raspberry Pi OS 64-bit installed
- Internet connection
- SSH access or direct terminal access

## Local Publish (Development Machine)

### Step 1: Publish for Linux ARM64

```bash
cd /path/to/SmartLocker
./scripts/publish-linux-arm64.sh
```

This creates a `publish/` directory containing all binaries and configuration files.

### Step 2: Transfer to Raspberry Pi

**Option A: SCP (Secure Copy)**
```bash
scp -r publish/ pi@raspberrypi.local:/home/pi/smartlocker-publish/
```

**Option B: USB Drive**
- Copy `publish/` directory to USB drive
- Insert USB into Raspberry Pi
- Mount and copy to `/home/pi/smartlocker-publish/`

## Raspberry Pi Installation

### Step 1: SSH into Raspberry Pi

```bash
ssh pi@raspberrypi.local
# or
ssh pi@<raspberry-pi-ip>
```

### Step 2: Navigate to Published Directory

```bash
cd /home/pi/smartlocker-publish
```

### Step 3: Run Installation Script

```bash
sudo bash scripts/install-raspberrypi.sh
```

The script will:
- Update system packages
- Install .NET 8.0 Runtime
- Install dependencies (SQLite, Chromium, etc.)
- Create application directories
- Create `smartlocker` system user
- Set up systemd service

### Step 4: Verify Installation

```bash
sudo systemctl status smartlocker
```

Should show: `active (running)`

## Database Setup

### Step 1: Create Database

```bash
cd /opt/smartlocker
sudo -u smartlocker dotnet SmartLocker.Web.dll --migrate
```

This creates `/var/lib/smartlocker/smartlocker.db` with all tables and seed data.

### Step 2: Verify Database

```bash
ls -lh /var/lib/smartlocker/smartlocker.db
```

Should show the database file with appropriate permissions.

## Configuration

### Edit Production Settings

```bash
sudo nano /opt/smartlocker/appsettings.Production.json
```

Key settings:

- **Database Path:** `/var/lib/smartlocker/smartlocker.db` (default)
- **Hardware Mode:** `Mock` (default) or `Gpio` (for actual hardware)
- **Base URL:** `http://smartlocker.local` or IP address
- **QR Expiry:** 10 minutes (default)

### GPIO Configuration (Optional)

If using actual locker hardware, update the `LockerHardware` section:

```json
"LockerHardware": {
  "Mode": "Gpio",
  "Lockers": [
    {
      "LockerId": 1,
      "RelayPin": 17,
      "DoorSensorPin": 27
    }
  ]
}
```

## Starting the Service

### Start Service

```bash
sudo systemctl start smartlocker
```

### Check Status

```bash
sudo systemctl status smartlocker
```

### View Logs

```bash
sudo journalctl -u smartlocker -f
```

### Enable Auto-Start on Boot

```bash
sudo systemctl enable smartlocker
```

## Accessing the Application

### Local Access

```
http://localhost:5000
```

### Network Access

```
http://<raspberry-pi-ip>:5000
http://smartlocker.local:5000  (if mDNS configured)
```

### Test Credentials

- **Admin:** `admin` / `admin123`
- **Staff:** `staff` / `staff123`

## Optional: Chromium Kiosk Mode

To automatically boot into the locker touchscreen interface:

### Step 1: Run Kiosk Setup Script

```bash
chmod +x scripts/chromium-kiosk-autostart.sh
./scripts/chromium-kiosk-autostart.sh
```

### Step 2: Reboot

```bash
sudo reboot
```

Chromium will launch automatically in fullscreen kiosk mode.

## Optional: mDNS Hostname

To enable `http://smartlocker.local`:

```bash
sudo hostnamectl set-hostname smartlocker
sudo systemctl restart avahi-daemon
```

Verify:
```bash
ping smartlocker.local
```

## Optional: Nginx Reverse Proxy

To proxy port 80 to 5000:

### Step 1: Install Nginx

```bash
sudo apt-get install -y nginx
```

### Step 2: Copy Configuration

```bash
sudo cp docs/nginx-smartlocker.conf /etc/nginx/sites-available/smartlocker
sudo ln -s /etc/nginx/sites-available/smartlocker /etc/nginx/sites-enabled/
```

### Step 3: Test and Enable

```bash
sudo nginx -t
sudo systemctl enable nginx
sudo systemctl start nginx
```

Access at `http://smartlocker.local` (port 80)

## Troubleshooting

### Service Won't Start

```bash
# Check detailed logs
sudo journalctl -u smartlocker -n 50

# Check port availability
sudo netstat -tulpn | grep 5000

# Restart service
sudo systemctl restart smartlocker
```

### Database Permission Error

```bash
# Fix permissions
sudo chown -R smartlocker:smartlocker /var/lib/smartlocker
sudo chmod 755 /var/lib/smartlocker
sudo chmod 644 /var/lib/smartlocker/smartlocker.db
```

### Kiosk Not Starting

```bash
# Check if service is running
sudo systemctl status smartlocker

# Increase sleep delay in chromium-kiosk-autostart.sh
# (if service takes longer to start)
```

### GPIO Not Working

```bash
# Add smartlocker user to gpio group
sudo usermod -a -G gpio smartlocker

# Reboot to apply
sudo reboot
```

## Maintenance

### View Application Logs

```bash
sudo journalctl -u smartlocker -f
```

### Restart Service

```bash
sudo systemctl restart smartlocker
```

### Stop Service

```bash
sudo systemctl stop smartlocker
```

### Backup Database

```bash
sudo cp /var/lib/smartlocker/smartlocker.db /var/lib/smartlocker/smartlocker.db.backup
```

### Health Check

```bash
curl http://localhost:5000/health
```

Should return JSON with app status and database connectivity.

## Security Notes

1. **Change Default Passwords:** Log in immediately and change admin/staff passwords
2. **Firewall:** Restrict port 5000 to local network only
3. **HTTPS:** Use Nginx with SSL certificate for production
4. **Database:** Ensure `/var/lib/smartlocker/` is only readable by `smartlocker` user
5. **SSH:** Disable root login, use key-based authentication

## Performance Tuning

### Increase Memory Limit (Optional)

Edit `/etc/systemd/system/smartlocker.service.d/override.conf`:

```ini
[Service]
MemoryLimit=512M
```

### CPU Quota (Optional)

```ini
[Service]
CPUQuota=75%
```

## Uninstallation

To remove SmartLocker:

```bash
sudo systemctl stop smartlocker
sudo systemctl disable smartlocker
sudo rm -rf /opt/smartlocker
sudo rm -rf /var/lib/smartlocker
sudo rm -rf /var/log/smartlocker
sudo userdel smartlocker
sudo rm /etc/systemd/system/smartlocker.service
sudo systemctl daemon-reload
```

## Support

For issues or questions, refer to:
- `docs/deployment-raspberrypi.md` - Detailed deployment guide
- `docs/security-review.md` - Security controls
- `docs/architecture.md` - System architecture
- `PHASE7_FINAL_REVIEW_REPORT.md` - Comprehensive review findings
