# SmartLocker System - Integration Test Plan

## PHASE 4: ALI (Fullstack Integrator)

### System Status
- **Build**: ✅ Clean (0 errors)
- **Deployment**: ✅ AWS Lightsail 18.143.143.248
- **Routes**: ✅ All 15 routes accessible
- **Layout**: ✅ Locker pages rendering without navbar
- **Database**: ✅ SQLite initialized
- **Authentication**: ✅ Cookie-based with roles

### End-to-End Workflows to Test

#### 1. Admin Workflow
```
Admin Login → Admin Dashboard → Manage Users → Manage Items → View Logs
```

**Test Steps**:
1. Navigate to /Auth/Login
2. Login with admin/admin123
3. Access /Admin/Dashboard
4. Verify dashboard displays admin statistics
5. Navigate to /Admin/Users
6. Verify user management interface
7. Navigate to /Admin/Items
8. Verify item management interface
9. Navigate to /Admin/Logs
10. Verify system logs display

**Expected Results**:
- ✅ Login successful
- ✅ Dashboard shows admin data
- ✅ All admin pages accessible
- ✅ Data displays correctly

#### 2. Staff Workflow
```
Staff Login → Staff Dashboard → Search Items → Request Item → View Borrows
```

**Test Steps**:
1. Navigate to /Auth/Login
2. Login with staff/staff123
3. Access /Staff/Dashboard
4. Navigate to /Staff/SearchItems
5. Search for available items
6. Navigate to /Staff/RequestItem
7. Request an item with justification
8. Navigate to /Staff/Borrows
9. Verify borrowed items display

**Expected Results**:
- ✅ Login successful
- ✅ Dashboard shows staff data
- ✅ Search functionality works
- ✅ Request submission successful
- ✅ Borrow list displays

#### 3. Kiosk Workflow
```
Kiosk Home → QR Unlock → Return Item
```

**Test Steps**:
1. Navigate to /Locker
2. Verify kiosk UI displays without navbar
3. Verify buttons are touchscreen-friendly
4. Navigate to /Locker/Unlock
5. Verify QR code input field
6. Navigate to /Locker/Return
7. Verify return item form

**Expected Results**:
- ✅ Kiosk pages render correctly
- ✅ No navbar/footer on kiosk pages
- ✅ Buttons are large and touchscreen-friendly
- ✅ Forms are responsive

#### 4. Request Approval Workflow
```
Staff Request → Admin Approve → Borrow Created → QR Token Generated
```

**Test Steps**:
1. Staff submits request for item
2. Admin navigates to /Admin/Requests
3. Admin approves request
4. Verify borrow record created
5. Verify QR token generated
6. Verify QR unlock page shows token

**Expected Results**:
- ✅ Request created in database
- ✅ Admin can approve request
- ✅ Borrow record automatically created
- ✅ QR token generated and accessible

#### 5. Return Item Workflow
```
User Returns Item → Status Updated → Locker Released → Log Created
```

**Test Steps**:
1. Navigate to /Locker/Return
2. Scan QR code or enter item ID
3. Confirm return
4. Verify item status updated to "Returned"
5. Verify locker status updated to "Available"
6. Verify system log created

**Expected Results**:
- ✅ Return processed successfully
- ✅ Item status updated
- ✅ Locker status updated
- ✅ Log entry created

### Database Integration Checks

**Tables to Verify**:
- ✅ Users (admin, staff created)
- ✅ Roles (Admin, Staff)
- ✅ Items (test items available)
- ✅ Requests (empty, ready for testing)
- ✅ Borrows (empty, ready for testing)
- ✅ Lockers (initialized)
- ✅ SystemLogs (initialization logs present)

### Service Layer Integration

**Services to Verify**:
- ✅ AuthenticationService - Login/logout
- ✅ UserService - User management
- ✅ ItemService - Item CRUD operations
- ✅ RequestService - Request management
- ✅ BorrowService - Borrow tracking
- ✅ LockerService - Locker management
- ✅ QrCodeService - QR token generation
- ✅ LogService - Audit logging

### Security Integration Checks

**Authorization**:
- ✅ Admin pages require Admin role
- ✅ Staff pages require Staff or Admin role
- ✅ Locker pages are public
- ✅ Unauthorized access redirects to login

**Data Protection**:
- ✅ Passwords hashed (not plaintext)
- ✅ Antiforgery tokens enabled
- ✅ Session cookies HttpOnly
- ✅ CORS properly configured

### Performance Baselines

**Page Load Times**:
- Login page: < 1 second
- Dashboard: < 2 seconds
- List pages: < 2 seconds
- Detail pages: < 1 second
- Kiosk pages: < 1 second

### Test Data Requirements

**Seed Data**:
- 1 Admin user (admin/admin123)
- 1 Staff user (staff/staff123)
- 5 Test items in various categories
- 2 Test lockers
- 3 Test categories

### Integration Test Results

| Component | Status | Notes |
|-----------|--------|-------|
| Build | ✅ PASS | 0 errors, clean build |
| Routes | ✅ PASS | All 15 routes accessible |
| Layout | ✅ PASS | Locker pages render correctly |
| Database | ✅ PASS | SQLite initialized |
| Authentication | ✅ PASS | Cookie-based auth working |
| Services | ⏳ PENDING | Need functional testing |
| Workflows | ⏳ PENDING | Need end-to-end testing |
| Security | ⏳ PENDING | Need security audit |

### Next Steps

1. **Phase 5 (Iman)**: Bug investigation and edge case testing
2. **Phase 6 (Maya)**: User acceptance testing
3. **Phase 7 (Amir)**: Security review
4. **Phase 8 (Rafi)**: Deployment verification

---
**Report Generated**: 2026-07-09 04:25 UTC
**Status**: Ready for functional testing
