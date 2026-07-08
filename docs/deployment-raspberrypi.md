# SmartLocker Raspberry Pi Deployment Guide

This guide provides comprehensive instructions for deploying the SmartLocker system on a Raspberry Pi running Raspberry Pi OS (64-bit).

## 1. Prerequisites

- Raspberry Pi 4 or 5 (2GB RAM minimum, 4GB+ recommended)
- Raspberry Pi OS (64-bit) with Desktop environment installed
- Internet connection (for initial setup)
- Access to the SmartLocker source code or published binaries

## 2. Preparation (On Development Machine)

Before deploying to the Raspberry Pi, you need to publish the application for the `linux-arm64` architecture.

1. Navigate to the SmartLocker project root.
2. Run the publish script:
   ```bash
   ./scripts/publish-linux-arm64.sh
   ```
3. This will create a `publish` directory containing all necessary files.
4. Transfer the `publish` directory and the `scripts` directory to your Raspberry Pi (e.g., via SCP, SFTP, or a USB drive).

## 3. Installation (On Raspberry Pi)

1. Open a terminal on the Raspberry Pi.
2. Navigate to the directory where you copied the files.
3. Make the installation script executable:
   ```bash
   chmod +x scripts/install-raspberrypi.sh
   ```
4. Run the installation script as root:
   ```bash
   sudo ./scripts/install-raspberrypi.sh
   ```

The script will:
- Install required dependencies (.NET runtime, SQLite, Chromium, etc.)
- Create application directories (`/opt/smartlocker`, `/var/lib/smartlocker`, `/var/log/smartlocker`)
- Create a dedicated `smartlocker` system user
- Copy the published files to `/opt/smartlocker`
- Install and enable the systemd service

## 4. Database Setup

The database needs to be created and migrated on the Raspberry Pi before starting the service.

1. Switch to the `smartlocker` user:
   ```bash
   sudo -u smartlocker -s
   ```
2. Navigate to the application directory:
   ```bash
   cd /opt/smartlocker
   ```
3. Run the Entity Framework migrations to create the SQLite database:
   ```bash
   dotnet SmartLocker.Web.dll --migrate
   ```
   *(Note: The `--migrate` flag is a conceptual command. If your application applies migrations automatically on startup in Production, you can skip this step. Otherwise, you must run the EF Core CLI tools or ensure your `Program.cs` calls `context.Database.Migrate()`.)*

4. Exit the `smartlocker` user session:
   ```bash
   exit
   ```

## 5. Configuration

Review the production configuration file to ensure it matches your environment.

```bash
sudo nano /opt/smartlocker/appsettings.Production.json
```

Key settings to verify:
- `Hardware:Mode`: Set to `"Mock"` for testing without physical lockers, or `"Gpio"` for actual hardware integration.
- `LockerHardware:Lockers`: Verify the GPIO pin mappings match your physical wiring.
- `SmartLocker:BaseUrl`: Set to your mDNS hostname (e.g., `http://smartlocker.local`) or IP address.

## 6. Starting the Service

1. Start the SmartLocker service:
   ```bash
   sudo systemctl start smartlocker
   ```
2. Check the status to ensure it's running:
   ```bash
   sudo systemctl status smartlocker
   ```
3. Verify the application is responding by checking the health endpoint:
   ```bash
   curl http://localhost:5000/health
   ```

## 7. Kiosk Mode Setup

To make the Raspberry Pi boot directly into the SmartLocker touchscreen interface:

1. Ensure you are logged in as the default desktop user (usually `pi`).
2. Make the kiosk script executable:
   ```bash
   chmod +x scripts/chromium-kiosk-autostart.sh
   ```
3. Run the script:
   ```bash
   ./scripts/chromium-kiosk-autostart.sh
   ```
4. Reboot the Raspberry Pi to test the autostart:
   ```bash
   sudo reboot
   ```

## 8. Hardware GPIO Integration

When you are ready to connect physical lockers:

1. **Safety Warning:** Ensure the Raspberry Pi is powered off before wiring relays or solenoids. Use appropriate isolation (e.g., optocouplers) to protect the Pi's GPIO pins from voltage spikes caused by solenoids.
2. Edit `/opt/smartlocker/appsettings.Production.json`.
3. Change `"Mode": "Mock"` to `"Mode": "Gpio"` in both the `SmartLocker:Hardware` and `LockerHardware` sections.
4. Update the `Lockers` array with the correct `RelayPin` and `DoorSensorPin` (BCM pin numbers) for each locker.
5. Restart the service:
   ```bash
   sudo systemctl restart smartlocker
   ```

## 9. Local Network Security

- **Isolation:** The SmartLocker application is designed to run on a local network (LAN/WiFi). Do not expose port 5000 directly to the public internet.
- **Passwords:** Immediately log in as the default Admin and change the password.
- **Firewall:** Consider configuring `ufw` (Uncomplicated Firewall) on the Raspberry Pi to only allow traffic on necessary ports (e.g., 22 for SSH, 5000 for the app, 80/443 if using Nginx).

## 10. Troubleshooting Guide

### Service won't start or keeps restarting
Check the detailed logs:
```bash
sudo journalctl -u smartlocker -f
```
Look for exceptions related to database access, port binding (is port 5000 already in use?), or missing configuration files.

### Database access errors (SQLite Error 14: unable to open database file)
Ensure the `smartlocker` user has read/write permissions to both the database file AND the directory containing it.
```bash
sudo chown -R smartlocker:smartlocker /var/lib/smartlocker
sudo chmod 755 /var/lib/smartlocker
sudo chmod 644 /var/lib/smartlocker/smartlocker.db
```

### Kiosk mode shows a blank screen or error
Verify the backend service is running (`curl http://localhost:5000/health`). If the service takes longer to start than Chromium, Chromium might show a "Connection Refused" error. The kiosk script includes a `sleep 3` delay, but you may need to increase this if your Pi boots slowly.

### GPIO pins not responding
- Verify you are using BCM pin numbering, not physical board pin numbers.
- Check the application logs for GPIO initialization errors.
- Ensure the `smartlocker` user is in the `gpio` group:
  ```bash
  sudo usermod -a -G gpio smartlocker
  ```
  *(Requires a reboot or session restart to take effect).*

### Useful Commands Reference
- Restart application: `sudo systemctl restart smartlocker`
- View recent logs: `sudo journalctl -u smartlocker -n 100`
- Check .NET version: `dotnet --info`
- Check listening ports: `sudo netstat -tulpn | grep LISTEN`
