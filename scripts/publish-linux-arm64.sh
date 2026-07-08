#!/bin/bash

###############################################################################
# SmartLocker Linux ARM64 Publish Script
# 
# This script publishes the SmartLocker application for Raspberry Pi (linux-arm64)
# in Release mode, ready for deployment.
#
# Usage: ./publish-linux-arm64.sh
#
# Prerequisites:
# - .NET 8.0 SDK installed
# - SmartLocker.Web.csproj in src/SmartLocker.Web/
#
###############################################################################

set -e  # Exit on error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Configuration
PROJECT_PATH="SmartLocker.Web/SmartLocker.Web.csproj"
PUBLISH_DIR="./publish"
RUNTIME="linux-arm64"
CONFIGURATION="Release"

echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}SmartLocker Linux ARM64 Publish${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""

# Check if project file exists
if [ ! -f "$PROJECT_PATH" ]; then
    echo -e "${RED}ERROR: Project file not found at $PROJECT_PATH${NC}"
    echo "Please run this script from the SmartLocker root directory."
    exit 1
fi

echo -e "${YELLOW}Configuration:${NC}"
echo "  Project: $PROJECT_PATH"
echo "  Runtime: $RUNTIME"
echo "  Configuration: $CONFIGURATION"
echo "  Output: $PUBLISH_DIR"
echo ""

# Clean previous publish (optional)
if [ -d "$PUBLISH_DIR" ]; then
    echo -e "${YELLOW}Cleaning previous publish directory...${NC}"
    rm -rf "$PUBLISH_DIR"
fi

# Restore NuGet packages
echo -e "${YELLOW}Restoring NuGet packages...${NC}"
dotnet restore "$PROJECT_PATH"

# Build the project
echo -e "${YELLOW}Building project...${NC}"
dotnet build "$PROJECT_PATH" -c "$CONFIGURATION"

# Publish for linux-arm64
echo -e "${YELLOW}Publishing for $RUNTIME...${NC}"
dotnet publish "$PROJECT_PATH" \
    -c "$CONFIGURATION" \
    -r "$RUNTIME" \
    --self-contained false \
    -o "$PUBLISH_DIR"

# Copy production configuration if it exists
if [ -f "SmartLocker.Web/appsettings.Production.json" ]; then
    echo -e "${YELLOW}Copying production configuration...${NC}"
    cp SmartLocker.Web/appsettings.Production.json "$PUBLISH_DIR/"
else
    echo -e "${YELLOW}Note: appsettings.Production.json not found. Creating default...${NC}"
    cp SmartLocker.Web/appsettings.json "$PUBLISH_DIR/appsettings.Production.json"
fi

# Copy systemd service file
if [ -f "scripts/smartlocker.service" ]; then
    echo -e "${YELLOW}Copying systemd service file...${NC}"
    cp scripts/smartlocker.service "$PUBLISH_DIR/"
fi

# Create a README for deployment
cat > "$PUBLISH_DIR/DEPLOYMENT_README.txt" << 'EOF'
SmartLocker Application - Ready for Deployment
===============================================

This directory contains the published SmartLocker application for Raspberry Pi (linux-arm64).

Contents:
- SmartLocker.Web.dll - Main application assembly
- appsettings.json - Default configuration
- appsettings.Production.json - Production configuration
- smartlocker.service - systemd service file
- Other dependencies and runtime files

Deployment Steps:
1. Copy entire contents to /opt/smartlocker on Raspberry Pi
2. Run the install script: scripts/install-raspberrypi.sh
3. Enable the systemd service: sudo systemctl enable smartlocker
4. Start the service: sudo systemctl start smartlocker
5. Access the application at http://localhost:5000

For detailed instructions, see: docs/deployment-raspberrypi.md

Database:
- SQLite database will be created at: /var/lib/smartlocker/smartlocker.db
- Run migrations if needed: dotnet ef database update

Troubleshooting:
- Check service status: sudo systemctl status smartlocker
- View logs: sudo journalctl -u smartlocker -f
- Restart service: sudo systemctl restart smartlocker

For more help, refer to the troubleshooting guide.
EOF

echo ""
echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}Publish completed successfully!${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""
echo -e "${YELLOW}Output location: $PUBLISH_DIR${NC}"
echo -e "${YELLOW}Size: $(du -sh $PUBLISH_DIR | cut -f1)${NC}"
echo ""
echo "Next steps:"
echo "1. Transfer the publish directory to your Raspberry Pi"
echo "2. Run: scripts/install-raspberrypi.sh"
echo "3. Start the service: sudo systemctl start smartlocker"
echo ""
