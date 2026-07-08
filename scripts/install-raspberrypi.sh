#!/bin/bash

###############################################################################
# SmartLocker Raspberry Pi Installation Script
#
# This script installs SmartLocker on Raspberry Pi OS 64-bit with all
# required dependencies, systemd service, and configuration.
#
# Usage: sudo ./install-raspberrypi.sh
#
# Prerequisites:
# - Raspberry Pi OS 64-bit
# - Internet connection
# - sudo privileges
#
###############################################################################

set -e  # Exit on error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
APP_DIR="/opt/smartlocker"
DATA_DIR="/var/lib/smartlocker"
LOG_DIR="/var/log/smartlocker"
SERVICE_USER="smartlocker"
SERVICE_NAME="smartlocker"
DOTNET_VERSION="8.0"

# Check if running as root
if [ "$EUID" -ne 0 ]; then 
    echo -e "${RED}ERROR: This script must be run as root (use sudo)${NC}"
    exit 1
fi

echo -e "${BLUE}========================================${NC}"
echo -e "${BLUE}SmartLocker Raspberry Pi Installation${NC}"
echo -e "${BLUE}========================================${NC}"
echo ""

# Step 1: Update system packages
echo -e "${YELLOW}[1/8] Updating system packages...${NC}"
apt-get update
apt-get upgrade -y

# Step 2: Install required dependencies
echo -e "${YELLOW}[2/8] Installing dependencies...${NC}"
apt-get install -y \
    curl \
    wget \
    git \
    sqlite3 \
    libsqlite3-dev \
    chromium-browser \
    x11-xserver-utils \
    unclutter \
    supervisor

# Step 3: Install .NET Runtime
echo -e "${YELLOW}[3/8] Installing .NET ${DOTNET_VERSION} Runtime...${NC}"

# Check if .NET is already installed
if ! command -v dotnet &> /dev/null; then
    echo "  Installing .NET from Microsoft repository..."
    wget -q https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    dpkg -i packages-microsoft-prod.deb
    rm packages-microsoft-prod.deb
    
    apt-get update
    apt-get install -y dotnet-runtime-${DOTNET_VERSION}
    
    echo -e "${GREEN}  .NET ${DOTNET_VERSION} Runtime installed${NC}"
else
    echo -e "${GREEN}  .NET is already installed$(dotnet --version)${NC}"
fi

# Step 4: Create application directories
echo -e "${YELLOW}[4/8] Creating application directories...${NC}"
mkdir -p "$APP_DIR"
mkdir -p "$DATA_DIR"
mkdir -p "$LOG_DIR"

# Step 5: Create service user
echo -e "${YELLOW}[5/8] Setting up service user...${NC}"
if ! id "$SERVICE_USER" &>/dev/null; then
    useradd -r -s /bin/bash -d "$APP_DIR" "$SERVICE_USER"
    echo -e "${GREEN}  Created user: $SERVICE_USER${NC}"
else
    echo -e "${GREEN}  User $SERVICE_USER already exists${NC}"
fi

# Step 6: Set permissions
echo -e "${YELLOW}[6/8] Setting permissions...${NC}"
chown -R "$SERVICE_USER:$SERVICE_USER" "$APP_DIR"
chown -R "$SERVICE_USER:$SERVICE_USER" "$DATA_DIR"
chown -R "$SERVICE_USER:$SERVICE_USER" "$LOG_DIR"
chmod 755 "$APP_DIR"
chmod 755 "$DATA_DIR"
chmod 755 "$LOG_DIR"

# Step 7: Copy application files (if in publish directory)
echo -e "${YELLOW}[7/8] Copying application files...${NC}"
if [ -d "./publish" ]; then
    echo "  Copying from ./publish..."
    cp -r ./publish/* "$APP_DIR/"
    chown -R "$SERVICE_USER:$SERVICE_USER" "$APP_DIR"
    echo -e "${GREEN}  Application files copied${NC}"
else
    echo -e "${YELLOW}  Note: ./publish directory not found${NC}"
    echo -e "${YELLOW}  Please copy published files manually to: $APP_DIR${NC}"
fi

# Step 8: Install systemd service
echo -e "${YELLOW}[8/8] Installing systemd service...${NC}"
if [ -f "./scripts/smartlocker.service" ]; then
    cp ./scripts/smartlocker.service /etc/systemd/system/
    systemctl daemon-reload
    systemctl enable smartlocker
    echo -e "${GREEN}  Service installed and enabled${NC}"
else
    echo -e "${RED}  ERROR: smartlocker.service not found${NC}"
    echo "  Please ensure scripts/smartlocker.service exists"
fi

echo ""
echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}Installation completed!${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""

echo -e "${BLUE}Important Information:${NC}"
echo "  Application Directory: $APP_DIR"
echo "  Data Directory: $DATA_DIR"
echo "  Log Directory: $LOG_DIR"
echo "  Service User: $SERVICE_USER"
echo "  Service Name: $SERVICE_NAME"
echo ""

echo -e "${BLUE}Next Steps:${NC}"
echo "1. Configure production settings:"
echo "   sudo nano $APP_DIR/appsettings.Production.json"
echo ""
echo "2. Create/update database:"
echo "   cd $APP_DIR"
echo "   sudo -u $SERVICE_USER dotnet SmartLocker.Web.dll --migrate"
echo ""
echo "3. Start the service:"
echo "   sudo systemctl start $SERVICE_NAME"
echo ""
echo "4. Check service status:"
echo "   sudo systemctl status $SERVICE_NAME"
echo ""
echo "5. View logs:"
echo "   sudo journalctl -u $SERVICE_NAME -f"
echo ""
echo "6. Access the application:"
echo "   http://localhost:5000"
echo ""

echo -e "${YELLOW}Optional: Setup Chromium Kiosk${NC}"
echo "  Run: scripts/chromium-kiosk-autostart.sh"
echo ""

echo -e "${YELLOW}Optional: Setup Nginx Reverse Proxy${NC}"
echo "  See: docs/nginx-smartlocker.conf"
echo ""

echo -e "${YELLOW}Optional: Setup mDNS Hostname${NC}"
echo "  See: docs/mdns-setup.md"
echo ""
