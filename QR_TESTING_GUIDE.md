# SmartLocker QR Code Testing Guide

## Overview

The QR unlock feature allows staff and users to unlock lockers by scanning a QR code generated after approving a borrow request. This guide explains how to test the complete QR workflow.

## QR Unlock Workflow

```
1. Staff/Admin approves a borrow request
   ↓
2. System generates access token
   ↓
3. QR code is displayed (or can be generated)
   ↓
4. User scans QR code with phone/scanner
   ↓
5. QR URL is accessed: /Locker/Unlock/{token}
   ↓
6. System validates token and unlocks locker
   ↓
7. Token is marked as used (single-use)
```

## Step-by-Step Testing

### Step 1: Login as Admin

```
URL: http://18.143.143.248/Auth/Login
Username: admin
Password: admin123
```

### Step 2: Create a Test Request (if needed)

1. Go to **Admin → Requests** or **Staff → Manage Requests**
2. View pending requests
3. If no requests exist, create one:
   - Go to **Staff → Request Item**
   - Select an available item
   - Submit the request

### Step 3: Approve the Request and Get QR Token

1. Go to **Admin → Requests**
2. Find a pending request
3. Click **Approve**
4. Select a locker from the dropdown
5. Click **Approve Request**
6. **Important**: The success message will display the access token

Example success message:
```
Request approved successfully! 
Access Token: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Step 4: Generate QR Code URL

The QR code URL format is:
```
http://18.143.143.248/Locker/Unlock/{token}
```

Replace `{token}` with the token from Step 3.

Example:
```
http://18.143.143.248/Locker/Unlock/eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Step 5: Test QR Unlock (Method 1: Direct URL)

1. Copy the QR unlock URL from Step 4
2. Open it in a browser
3. Expected response:
   - **Success**: Locker unlocked, page shows success message
   - **Error**: Token expired, already used, or invalid

### Step 6: Test QR Unlock (Method 2: Generate QR Code)

To generate a visual QR code:

```bash
# Using qrencode (if installed)
qrencode -o qr.png "http://18.143.143.248/Locker/Unlock/{token}"

# Or use an online QR generator
# Visit: https://www.qr-code-generator.com/
# Enter the URL from Step 4
```

Then scan the QR code with a phone camera or QR scanner app.

### Step 7: Test Token Expiration

1. Get a token from Step 3
2. Wait for the token to expire (default: 5 minutes)
3. Try to access the unlock URL
4. Expected: "Token expired" error message

### Step 8: Test Single-Use Token

1. Get a token from Step 3
2. Access the unlock URL once (successful unlock)
3. Try to access the same URL again
4. Expected: "Token already used" error message

## Testing Scenarios

### Scenario 1: Normal Unlock Flow
```
✅ Request item
✅ Approve request
✅ Get token
✅ Access unlock URL
✅ Locker unlocks
✅ Token marked as used
```

### Scenario 2: Expired Token
```
✅ Request item
✅ Approve request
✅ Get token
⏳ Wait 5+ minutes
❌ Access unlock URL → "Token expired"
```

### Scenario 3: Already Used Token
```
✅ Request item
✅ Approve request
✅ Get token
✅ Access unlock URL (first time) → Success
❌ Access unlock URL (second time) → "Token already used"
```

### Scenario 4: Invalid Token
```
❌ Access unlock URL with random token
❌ Expected: "Invalid token" error
```

## QR Code Service Details

The system uses the `QrCodeService` to:

1. **Generate QR URLs**:
   ```csharp
   var qrUrl = _qrCodeService.GenerateQrCodeUrl(token);
   // Returns: http://18.143.143.248/Locker/Unlock/{token}
   ```

2. **Generate QR Code SVG** (placeholder):
   ```csharp
   var qrSvg = _qrCodeService.GenerateQrCodeSvg(token);
   // Returns: Simple SVG placeholder (not production-ready)
   ```

3. **Generate QR Code Data URI**:
   ```csharp
   var qrDataUri = _qrCodeService.GenerateQrCodeDataUri(token);
   // Returns: data:image/svg+xml;base64,...
   ```

## Current Limitations

1. **QR Code Generation**: Currently generates a placeholder SVG, not a real scannable QR code
   - **Fix**: Integrate a real QR library like `QRCoder` or `ZXing.Net`

2. **Base URL Configuration**: 
   - Development: `http://localhost:5000`
   - Production: Must be configured in `appsettings.json` under `SmartLocker:BaseUrl`
   - **Current issue**: Production URLs may point to localhost

3. **Token Display**: Token is shown in success message, not as a visual QR code
   - **Fix**: Display generated QR code image on approval page

## Configuration

Edit `appsettings.json` to configure QR settings:

```json
{
  "SmartLocker": {
    "BaseUrl": "http://18.143.143.248",
    "QrTokenExpirationMinutes": 5,
    "QrTokenLength": 32
  }
}
```

## API Endpoints for Testing

### Generate QR Token
```
POST /api/qr/generate
Body: { borrowId: 123, lockerId: 1 }
Response: { token: "...", expiresAt: "2026-07-09T05:00:00Z" }
```

### Validate QR Token
```
GET /api/qr/validate/{token}
Response: { valid: true, borrowId: 123, lockerId: 1 }
```

### Unlock Locker
```
GET /Locker/Unlock/{token}
Response: Redirect to success page or error page
```

## Troubleshooting

| Issue | Cause | Solution |
|-------|-------|----------|
| Token not generated | Request not approved | Approve request first |
| Token expired | Waited too long | Generate new token by re-approving |
| Invalid token | Wrong token format | Copy token correctly from success message |
| Locker not unlocking | GPIO not configured | Check GPIO configuration or use mock mode |
| QR code not scannable | Placeholder SVG used | Integrate real QR library |
| Wrong base URL | Config not updated | Update `appsettings.json` BaseUrl |

## Next Steps

1. **Integrate Real QR Library**:
   ```bash
   dotnet add package QRCoder
   ```

2. **Display QR Code on Approval Page**:
   - Generate QR image
   - Display in modal/page
   - Allow download/print

3. **Add QR Code Display to Borrow Page**:
   - Show QR code after approval
   - Allow re-generation if expired

4. **Mobile App Integration**:
   - Create mobile app to scan QR codes
   - Integrate with unlock API

## Testing Checklist

- [ ] Login as Admin
- [ ] Create/find a pending request
- [ ] Approve request and get token
- [ ] Generate QR URL
- [ ] Test direct URL access
- [ ] Test token expiration
- [ ] Test single-use token
- [ ] Test invalid token
- [ ] Generate visual QR code
- [ ] Scan QR code with phone
- [ ] Verify locker unlock
- [ ] Check audit logs

---

**Last Updated**: 2026-07-09  
**Status**: Testing Guide Complete
