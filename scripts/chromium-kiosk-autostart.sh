#!/bin/bash

###############################################################################
# SmartLocker Chromium Kiosk Autostart Script
#
# This script sets up Chromium to automatically start in kiosk mode on boot,
# displaying the SmartLocker locker interface.
#
# Usage: ./chromium-kiosk-autostart.sh
#
# Prerequisites:
# - Raspberry Pi OS with desktop environment
# - Chromium browser installed
# - SmartLocker application running on localhost:5000
#
###############################################################################

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

# Configuration
KIOSK_URL="http://localhost:5000/Locker"
KIOSK_USER="pi"
LXDE_AUTOSTART_DIR="/home/$KIOSK_USER/.config/lxsession/LXDE-pi"
LXDE_AUTOSTART_FILE="$LXDE_AUTOSTART_DIR/autostart"

echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}SmartLocker Chromium Kiosk Setup${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""

# Check if running as root
if [ "$EUID" -eq 0 ]; then 
    echo -e "${RED}ERROR: Do not run this script as root${NC}"
    echo "Run as the pi user: ./chromium-kiosk-autostart.sh"
    exit 1
fi

# Create LXDE autostart directory if it doesn't exist
echo -e "${YELLOW}Creating LXDE autostart directory...${NC}"
mkdir -p "$LXDE_AUTOSTART_DIR"

# Backup existing autostart file
if [ -f "$LXDE_AUTOSTART_FILE" ]; then
    echo -e "${YELLOW}Backing up existing autostart file...${NC}"
    cp "$LXDE_AUTOSTART_FILE" "$LXDE_AUTOSTART_FILE.bak.$(date +%s)"
fi

# Create new autostart configuration
echo -e "${YELLOW}Creating Chromium kiosk autostart configuration...${NC}"
cat > "$LXDE_AUTOSTART_FILE" << 'EOF'
@lxpanel --profile LXDE-pi
@pcmanfm --desktop --profile LXDE-pi
@xscreensaver -no-splash

# SmartLocker Chromium Kiosk
@unclutter -idle 3
@chromium-browser --kiosk --no-first-run --no-default-browser-check --disable-translate --disable-infobars --disable-suggestions-ui --disable-save-password-bubble http://localhost:5000/Locker
EOF

echo -e "${GREEN}Autostart configuration created${NC}"
echo ""

# Alternative: Create a dedicated kiosk startup script
echo -e "${YELLOW}Creating dedicated kiosk startup script...${NC}"
cat > "$HOME/.smartlocker-kiosk.sh" << 'EOF'
#!/bin/bash
# SmartLocker Chromium Kiosk Startup Script

# Wait for X server to start
sleep 3

# Hide mouse cursor after 3 seconds of inactivity
unclutter -idle 3 -root &

# Launch Chromium in kiosk mode
chromium-browser \
    --kiosk \
    --no-first-run \
    --no-default-browser-check \
    --disable-translate \
    --disable-infobars \
    --disable-suggestions-ui \
    --disable-save-password-bubble \
    --disable-session-crashed-bubble \
    --disable-component-update \
    --disable-default-apps \
    --disable-preconnect \
    http://localhost:5000/Locker
EOF

chmod +x "$HOME/.smartlocker-kiosk.sh"
echo -e "${GREEN}Kiosk startup script created: $HOME/.smartlocker-kiosk.sh${NC}"
echo ""

# Create systemd user service for kiosk (alternative method)
echo -e "${YELLOW}Creating systemd user service for kiosk...${NC}"
mkdir -p "$HOME/.config/systemd/user"
cat > "$HOME/.config/systemd/user/smartlocker-kiosk.service" << 'EOF'
[Unit]
Description=SmartLocker Chromium Kiosk
After=graphical-session.target

[Service]
Type=simple
ExecStart=%h/.smartlocker-kiosk.sh
Restart=on-failure
RestartSec=5

[Install]
WantedBy=graphical-session.target
EOF

echo -e "${GREEN}Systemd user service created${NC}"
echo ""

echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}Chromium Kiosk Setup Complete!${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""

echo -e "${YELLOW}Configuration Methods:${NC}"
echo ""
echo "Method 1: LXDE Autostart (Recommended for Raspberry Pi OS)"
echo "  - Autostart file: $LXDE_AUTOSTART_FILE"
echo "  - Chromium will launch automatically on desktop login"
echo ""
echo "Method 2: Dedicated Kiosk Script"
echo "  - Script location: $HOME/.smartlocker-kiosk.sh"
echo "  - Add to LXDE autostart or cron"
echo ""
echo "Method 3: Systemd User Service"
echo "  - Service file: $HOME/.config/systemd/user/smartlocker-kiosk.service"
echo "  - Enable: systemctl --user enable smartlocker-kiosk"
echo "  - Start: systemctl --user start smartlocker-kiosk"
echo ""

echo -e "${YELLOW}Next Steps:${NC}"
echo "1. Reboot the Raspberry Pi to test autostart:"
echo "   sudo reboot"
echo ""
echo "2. Or manually test the kiosk script:"
echo "   $HOME/.smartlocker-kiosk.sh"
echo ""
echo "3. If using systemd service, enable it:"
echo "   systemctl --user enable smartlocker-kiosk"
echo ""

echo -e "${YELLOW}Troubleshooting:${NC}"
echo "- If Chromium doesn't start, check that SmartLocker service is running:"
echo "  sudo systemctl status smartlocker"
echo ""
echo "- To disable kiosk mode, comment out the Chromium line in autostart"
echo ""
echo "- To exit kiosk mode manually, press Alt+F4"
echo ""
