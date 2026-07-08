# SmartLocker UAT Checklist

Comprehensive User Acceptance Testing checklist for all system features.

## Pre-Test Setup

- [ ] Application running locally: `dotnet run`
- [ ] Database initialized with seed data
- [ ] Browser opened to `https://localhost:5001`
- [ ] Test credentials available: admin/admin123, staff/staff123

---

## Authentication & Authorization

### Login Functionality
- [ ] Admin can login with admin/admin123
- [ ] Staff can login with staff/staff123
- [ ] Invalid credentials show error message
- [ ] Session timeout after 30 minutes
- [ ] Logout clears session
- [ ] Cannot access admin pages without admin role
- [ ] Cannot access staff pages without staff role

---

## Admin Dashboard

### Dashboard Display
- [ ] Dashboard loads without errors
- [ ] Shows user statistics
- [ ] Shows item statistics
- [ ] Shows active borrow count
- [ ] Shows overdue count
- [ ] Navigation menu visible and functional

### Navigation
- [ ] "Manage Users" link works
- [ ] "Manage Items" link works
- [ ] "Manage Requests" link works
- [ ] "Manage Borrows" link works
- [ ] "View Overdue" link works
- [ ] "View Logs" link works
- [ ] Logout link works

---

## Manage Users (UC-03)

### Create User
- [ ] "Create User" button visible
- [ ] Form displays all required fields
- [ ] Username field validates (required, unique)
- [ ] Email field validates (required, format)
- [ ] Password field validates (required, min length)
- [ ] Role dropdown shows Admin and Staff options
- [ ] Submit creates user successfully
- [ ] New user appears in list
- [ ] Success message displayed
- [ ] Can login with new credentials

### View Users
- [ ] User list displays all users
- [ ] List shows username, email, role
- [ ] List is sortable
- [ ] List is paginated if > 10 users

### Edit User
- [ ] Edit button visible for each user
- [ ] Can change email
- [ ] Can change role
- [ ] Changes save successfully
- [ ] Cannot edit own role to non-admin

### Disable User
- [ ] Disable button visible
- [ ] Disabled user cannot login
- [ ] Disabled user appears as inactive in list

---

## Manage Items (UC-04)

### Create Item
- [ ] "Create Item" button visible
- [ ] Form displays all required fields
- [ ] Item name field validates (required)
- [ ] Category dropdown shows all categories
- [ ] Description field accepts text
- [ ] Locker dropdown shows available lockers
- [ ] Submit creates item successfully
- [ ] New item appears in list
- [ ] Item status defaults to "Available"

### View Items
- [ ] Item list displays all items
- [ ] List shows name, category, status, locker
- [ ] List is sortable by column
- [ ] List is paginated if > 10 items

### Edit Item
- [ ] Edit button visible for each item
- [ ] Can change category
- [ ] Can change locker assignment
- [ ] Changes save successfully
- [ ] Cannot change item name (read-only)

### Delete Item
- [ ] Delete button visible
- [ ] Confirmation dialog appears
- [ ] Item removed from list after deletion
- [ ] Cannot delete borrowed items

---

## Manage Requests (UC-09, UC-10, UC-11)

### View Requests
- [ ] Request list displays all requests
- [ ] Shows username, item name, request date, status
- [ ] Filters available: Pending, Approved, Rejected
- [ ] List is paginated if > 10 requests

### Approve Request
- [ ] "Approve" button visible for pending requests
- [ ] Approval creates Borrow record
- [ ] Borrow status set to "Active"
- [ ] Item status changed to "Borrowed"
- [ ] Locker status changed to "Occupied"
- [ ] QR token generated
- [ ] Request status changed to "Approved"
- [ ] Success message displayed

### Reject Request
- [ ] "Reject" button visible for pending requests
- [ ] Rejection reason can be entered
- [ ] Request status changed to "Rejected"
- [ ] No Borrow record created
- [ ] Item remains "Available"
- [ ] Success message displayed

### Prevent Double Approval
- [ ] Cannot approve already approved request
- [ ] Error message shown if attempted

---

## Manage Borrows (UC-06)

### View Borrows
- [ ] Borrow list displays all active borrows
- [ ] Shows username, item name, borrow date, due date
- [ ] Can filter by status (Active, Returned, Overdue, Lost)
- [ ] List is paginated if > 10 borrows

### Borrow Details
- [ ] Can view borrow details
- [ ] Shows item information
- [ ] Shows locker assignment
- [ ] Shows QR token (if available)

### Return Item (UC-07)
- [ ] "Return" button visible for active borrows
- [ ] Return action updates Borrow status to "Returned"
- [ ] Item status changed to "Available"
- [ ] Locker status changed to "Available"
- [ ] Return date recorded
- [ ] Success message displayed

### Extend Loan (UC-15)
- [ ] "Extend" button visible for active borrows
- [ ] Extension dialog appears
- [ ] Can select extension period (days)
- [ ] BorrowEndDate updated correctly
- [ ] Success message displayed

### Prevent Double Return
- [ ] Cannot return already returned borrow
- [ ] Error message shown if attempted

---

## View Overdue (UC-08)

### Overdue List
- [ ] Overdue list displays only expired active borrows
- [ ] Shows username, item name, days overdue
- [ ] List sorted by days overdue (descending)
- [ ] Can filter by user

### Overdue Actions
- [ ] Can mark item as "Lost"
- [ ] Can send reminder notification
- [ ] Can force return

---

## View Logs (UC-05)

### Log Display
- [ ] Log list displays all system actions
- [ ] Shows action, resource type, user, timestamp
- [ ] List is paginated if > 50 logs
- [ ] Logs sorted by date (newest first)

### Log Filtering
- [ ] Can filter by action (Create, Update, Delete, Approve, etc.)
- [ ] Can filter by resource type (User, Item, Request, Borrow, etc.)
- [ ] Can filter by user
- [ ] Can filter by date range

### Log Details
- [ ] Can view full log details
- [ ] Shows all relevant information

---

## Staff Dashboard

### Dashboard Display
- [ ] Dashboard loads without errors
- [ ] Shows quick stats (requests, active borrows)
- [ ] Shows navigation links

### Navigation
- [ ] "Search Items" link works
- [ ] "Request Item" link works
- [ ] "My Borrows" link works
- [ ] Logout link works

---

## Search Items (UC-12)

### Search Functionality
- [ ] Search page loads
- [ ] Can filter by category
- [ ] Can filter by status (Available, Borrowed, Maintenance, Lost)
- [ ] Search results display correctly
- [ ] Shows item name, category, status, locker

### Item Details
- [ ] Can view item details
- [ ] Shows full description
- [ ] Shows locker location
- [ ] Shows current borrower (if borrowed)

---

## Request Item (UC-13)

### Request Form
- [ ] Request form displays
- [ ] Item selection dropdown shows available items
- [ ] Justification field accepts text
- [ ] Submit button visible

### Submit Request
- [ ] Can submit request for available item
- [ ] Request created with "Pending" status
- [ ] Success message displayed
- [ ] Cannot request already borrowed item
- [ ] Cannot submit duplicate request for same item

---

## My Borrows (Staff)

### View Borrows
- [ ] Shows only current user's active borrows
- [ ] Displays item name, borrow date, due date
- [ ] Shows days remaining until due

### Return Item
- [ ] "Return" button visible for each borrow
- [ ] Return action successful
- [ ] Item marked as "Available"
- [ ] Borrow status changed to "Returned"

### Extend Loan
- [ ] "Extend" button visible for each borrow
- [ ] Can extend loan
- [ ] Due date updated correctly

---

## Locker Kiosk Interface

### Locker Home
- [ ] Kiosk home page accessible at `/Locker`
- [ ] Welcome message displayed
- [ ] Instructions clear
- [ ] "Unlock with QR" button visible
- [ ] Large touchscreen-friendly buttons

### QR Unlock
- [ ] QR code input field visible
- [ ] Can paste QR token
- [ ] Can manually enter token
- [ ] "Unlock" button visible

### Unlock Success
- [ ] Valid QR token unlocks successfully
- [ ] Success message displayed
- [ ] Shows item name and locker number
- [ ] Shows next steps
- [ ] Token marked as used

### Unlock Failure
- [ ] Invalid token shows error
- [ ] Expired token shows error
- [ ] Used token shows error
- [ ] Error messages are clear

### Prevent Double Unlock
- [ ] Cannot use same token twice
- [ ] Second attempt shows "already used" error

### Touchscreen Usability
- [ ] Buttons are large (44x44px minimum)
- [ ] Text is readable on small screen
- [ ] No hover effects (touchscreen doesn't hover)
- [ ] Form inputs are accessible

---

## Responsive Design

### Desktop (1920x1080)
- [ ] All pages render correctly
- [ ] No horizontal scrolling
- [ ] All buttons clickable
- [ ] Tables readable

### Tablet (768x1024)
- [ ] All pages responsive
- [ ] Navigation adapts
- [ ] Tables stack appropriately
- [ ] Forms readable

### Mobile (375x667)
- [ ] All pages responsive
- [ ] Navigation collapses to menu
- [ ] Buttons remain clickable
- [ ] Text readable without zoom

### Kiosk (1024x600)
- [ ] Kiosk pages display correctly
- [ ] Buttons large and accessible
- [ ] No unnecessary scrolling
- [ ] Touchscreen-optimized

---

## Error Handling

### Form Validation
- [ ] Required fields show error if empty
- [ ] Email format validated
- [ ] Password strength enforced
- [ ] Duplicate usernames rejected
- [ ] Error messages clear and helpful

### Database Errors
- [ ] Graceful error handling for DB failures
- [ ] User-friendly error messages
- [ ] No technical details exposed

### Network Errors
- [ ] Timeout errors handled gracefully
- [ ] Retry options provided
- [ ] No data loss on error

---

## Security

### Password Security
- [ ] Passwords never displayed in plain text
- [ ] Password fields masked
- [ ] Passwords hashed in database

### Authorization
- [ ] Admin-only pages require admin role
- [ ] Staff-only pages require staff role
- [ ] Cannot access other users' data
- [ ] Cannot modify other users' records

### Input Validation
- [ ] All form inputs validated
- [ ] No SQL injection possible
- [ ] No XSS vulnerabilities
- [ ] No CSRF vulnerabilities (anti-forgery tokens present)

### Audit Trail
- [ ] All actions logged
- [ ] Logs show who did what when
- [ ] Cannot modify logs
- [ ] Cannot delete logs

---

## Performance

### Page Load Times
- [ ] Dashboard loads < 2 seconds
- [ ] List pages load < 3 seconds
- [ ] Form pages load < 1 second
- [ ] No noticeable lag

### Database Performance
- [ ] Queries execute quickly
- [ ] No N+1 query problems
- [ ] Pagination works efficiently
- [ ] Filtering performs well

---

## Browser Compatibility

- [ ] Chrome/Chromium
- [ ] Firefox
- [ ] Safari
- [ ] Edge

---

## Final Sign-Off

### Tester Information
- [ ] Tester Name: ___________________
- [ ] Test Date: ___________________
- [ ] Environment: Local / Raspberry Pi

### Overall Assessment
- [ ] All critical features working
- [ ] No blocking bugs
- [ ] UI acceptable
- [ ] Performance acceptable

### Sign-Off
- [ ] **PASS** - Ready for production
- [ ] **PASS WITH NOTES** - Ready with known limitations
- [ ] **FAIL** - Not ready, issues must be fixed

### Notes
```
[Space for tester notes and comments]
```

---

## Test Execution Summary

| Feature | Status | Notes |
|---------|--------|-------|
| Authentication | ✅ / ⚠️ / ❌ | |
| Admin Dashboard | ✅ / ⚠️ / ❌ | |
| Manage Users | ✅ / ⚠️ / ❌ | |
| Manage Items | ✅ / ⚠️ / ❌ | |
| Manage Requests | ✅ / ⚠️ / ❌ | |
| Manage Borrows | ✅ / ⚠️ / ❌ | |
| View Overdue | ✅ / ⚠️ / ❌ | |
| View Logs | ✅ / ⚠️ / ❌ | |
| Staff Dashboard | ✅ / ⚠️ / ❌ | |
| Search Items | ✅ / ⚠️ / ❌ | |
| Request Item | ✅ / ⚠️ / ❌ | |
| My Borrows | ✅ / ⚠️ / ❌ | |
| Locker Kiosk | ✅ / ⚠️ / ❌ | |
| QR Unlock | ✅ / ⚠️ / ❌ | |
| Responsive Design | ✅ / ⚠️ / ❌ | |
| Security | ✅ / ⚠️ / ❌ | |
| Performance | ✅ / ⚠️ / ❌ | |

**Overall Result:** ✅ PASS / ⚠️ PASS WITH NOTES / ❌ FAIL
