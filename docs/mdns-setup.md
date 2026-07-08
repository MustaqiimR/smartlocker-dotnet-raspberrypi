# SmartLocker mDNS Hostname Setup

This guide explains how to set up the `smartlocker.local` hostname on Raspberry Pi OS using mDNS (Multicast DNS).

## What is mDNS?

mDNS allows devices on a local network to be accessed by hostname without a traditional DNS server. With mDNS, you can access your Raspberry Pi as `smartlocker.local` instead of using its IP address.

## Prerequisites

- Raspberry Pi OS 64-bit
- Network connectivity
- sudo privileges

## Setup Steps

### 1. Install Avahi Daemon

Avahi is the mDNS service that handles hostname resolution on the local network.

```bash
sudo apt-get update
sudo apt-get install -y avahi-daemon avahi-utils
```

### 2. Set Raspberry Pi Hostname

Set the hostname to `smartlocker`:

```bash
sudo hostnamectl set-hostname smartlocker
```

Verify the hostname was set:

```bash
hostnamectl
```

### 3. Update /etc/hosts

Edit the hosts file to include the new hostname:

```bash
sudo nano /etc/hosts
```

Add or update the line:

```
127.0.0.1       localhost
::1             localhost
127.0.1.1       smartlocker
```

### 4. Enable Avahi Service

Ensure Avahi daemon is running and enabled on boot:

```bash
sudo systemctl enable avahi-daemon
sudo systemctl start avahi-daemon
```

Check the status:

```bash
sudo systemctl status avahi-daemon
```

### 5. Verify mDNS Resolution

Test that the hostname resolves correctly:

```bash
# From the Raspberry Pi itself
ping smartlocker.local

# From another device on the network
ping smartlocker.local
```

You should see responses from the Raspberry Pi.

### 6. Update SmartLocker Configuration (Optional)

Update the `appsettings.Production.json` to use the mDNS hostname:

```json
"SmartLocker": {
  "BaseUrl": "http://smartlocker.local"
}
```

### 7. Access SmartLocker via mDNS

Once mDNS is configured, you can access the application from any device on the network:

```
http://smartlocker.local
http://smartlocker.local:5000
http://smartlocker.local/Locker
```

## Troubleshooting

### mDNS not resolving

1. Check Avahi daemon is running:
   ```bash
   sudo systemctl status avahi-daemon
   ```

2. Check firewall rules (if using ufw):
   ```bash
   sudo ufw allow 5353/udp
   sudo ufw reload
   ```

3. Restart Avahi:
   ```bash
   sudo systemctl restart avahi-daemon
   ```

### Hostname not set correctly

Verify with:

```bash
hostname
cat /etc/hostname
hostnamectl
```

### Can't ping from other devices

1. Ensure devices are on the same network (same WiFi or LAN)
2. Check that Avahi daemon is running on the Raspberry Pi
3. Try pinging the IP address first to verify network connectivity
4. Some networks block mDNS - check firewall settings

### Windows Clients

Windows 10/11 may not support mDNS by default. Options:

1. Install Bonjour for Windows: https://support.apple.com/downloads/bonjour_for_windows
2. Use the IP address directly
3. Add an entry to the Windows hosts file

## Additional Notes

- mDNS only works on the local network (not over the internet)
- The `.local` domain is reserved for mDNS
- Avahi is pre-installed on most Raspberry Pi OS distributions
- mDNS is optional - you can always use the IP address directly

## References

- Avahi Documentation: https://avahi.org/
- Raspberry Pi Hostname Documentation: https://www.raspberrypi.com/documentation/computers/remote-access.html
