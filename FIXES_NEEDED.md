# SmartLocker Critical Issues - Fixes in Progress

## ADMIN SECTION

### Issue 1: Header Layering
- **Problem**: Staff dashboard header overlaps with another header
- **File**: Pages/Shared/_Layout.cshtml
- **Fix**: Add z-index to navbar and ensure proper spacing

### Issue 2: Password Edit
- **Problem**: Admin cannot edit password
- **File**: Pages/Admin/Users.cshtml.cs
- **Fix**: Add OnPostChangePassword handler with password hashing

### Issue 3: User Deletion
- **Problem**: Admin cannot delete users
- **File**: Pages/Admin/Users.cshtml.cs
- **Fix**: Add OnPostDelete handler with FK constraint checks

### Issue 4: Manage Locker Page
- **Problem**: Page doesn't exist
- **Files to Create**: 
  - Pages/Admin/Lockers.cshtml
  - Pages/Admin/Lockers.cshtml.cs
- **Fix**: Create CRUD page for lockers

### Issue 5: Item-Locker Assignment
- **Problem**: Items not assigned to locker until request is made
- **File**: Pages/Admin/Items.cshtml.cs
- **Fix**: Add locker dropdown to item creation/edit form

## STAFF SECTION

### Issue 6: Request Date Justification
- **Problem**: Cannot specify borrow start/end dates
- **File**: Pages/Staff/RequestItem.cshtml
- **Fix**: Add date pickers for desired borrow period

### Issue 7: Borrow History
- **Problem**: No view of past borrows
- **Files to Create**:
  - Pages/Staff/BorrowHistory.cshtml
  - Pages/Staff/BorrowHistory.cshtml.cs
- **Fix**: Create page showing returned/overdue/lost items

### Issue 8: Return Item Page
- **Problem**: Redundant page (already in Manage Borrow)
- **File**: Pages/Staff/ReturnItem.cshtml
- **Fix**: Redirect to ManageBorrows or remove

### Issue 9: Request Cancellation
- **Problem**: Blank page after cancelling request
- **File**: Pages/Staff/RequestItem.cshtml.cs
- **Fix**: Add proper redirect to SearchItems on cancel

### Issue 10: Request Deletion
- **Problem**: Cannot delete rejected requests
- **File**: Pages/Staff/ManageRequests.cshtml.cs
- **Fix**: Add OnPostDelete handler for rejected requests

### Issue 11: Request ID Security
- **Problem**: Actual request ID visible (security concern)
- **Files**: Pages/Staff/ManageRequests.cshtml, RequestItem.cshtml
- **Fix**: Show sequential number (Request #1, #2) instead of actual ID

## IMPLEMENTATION ORDER
1. Fix header layering (quick)
2. Add password/delete to Users (medium)
3. Create Lockers page (medium)
4. Fix item-locker assignment (quick)
5. Add date justification to requests (medium)
6. Create borrow history page (medium)
7. Fix request cancellation flow (quick)
8. Add request deletion (quick)
9. Implement request ID security (medium)
10. Build, test, deploy
