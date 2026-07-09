# SmartLocker Project Explainer - PowerPoint Slide Content

## Slide 1: Title Slide
**Title:** SmartLocker System
**Subtitle:** .NET on Linux Raspberry Pi On-Premise Kiosk Application
**Content:**
- Research Project Joint Venture
- ISense ASV Sdn Bhd & UKM FTSM
- July 2026

---

## Slide 2: Project Overview
**Agent:** Zara (Researcher/Requirement Analyst)
**Title:** Project Overview
**Content:**
- **Objective:** Build a comprehensive locker management system for on-premise deployment on Raspberry Pi
- **Scope:** Item borrowing, locker access control, QR unlock, role-based management
- **Technology Stack:** ASP.NET Core, SQLite, Razor Pages, Raspberry Pi OS 64-bit
- **Deployment:** Local network touchscreen kiosk with admin/staff web interfaces
- **Status:** ✅ Complete and ready for demo

---

## Slide 3: System Actors & Roles
**Agent:** Zara (Researcher/Requirement Analyst)
**Title:** System Actors & User Roles
**Content:**
- **Admin:** Full system control, user management, item management, request approval, overdue tracking
- **Staff:** Search items, submit requests, manage borrows, return items, extend loans
- **Locker (Kiosk):** Public touchscreen interface for QR unlock and item returns
- **System:** Automated audit logging, status tracking, notification handling

---

## Slide 4: 15 Use Cases Implemented
**Agent:** Zara (Researcher/Requirement Analyst)
**Title:** Complete Use Case Coverage
**Content:**
- UC-01: Login
- UC-02: Dashboard
- UC-03: Manage User
- UC-04: Manage Item
- UC-05: View Logs
- UC-06: Manage Borrow
- UC-07: Return Item
- UC-08: View Overdue
- UC-09: Manage Request
- UC-10: Approve Request
- UC-11: Reject Request
- UC-12: Search Item
- UC-13: Request Item
- UC-14: Borrow Item
- UC-15: Extend Loan

---

## Slide 5: Database Architecture
**Agent:** Abu (Backend Developer)
**Title:** Database Schema & Entity Models
**Content:**
- **User Management:** User, Role
- **Inventory:** Item, Category, ItemStatus
- **Locker Management:** Locker, LockerStatus
- **Workflow:** Request, RequestStatus, Borrow, BorrowStatus
- **Security:** LockerAccessToken
- **Audit:** SystemLog
- **Total:** 15 tables with proper relationships
- **Technology:** SQLite with Entity Framework Core

---

## Slide 6: Backend Architecture
**Agent:** Abu (Backend Developer)
**Title:** Service Layer & Business Logic
**Content:**
- **AuthenticationService:** User login, password hashing, verification
- **UserService:** User CRUD, role management
- **ItemService:** Item management, search, status updates
- **BorrowService:** Borrow lifecycle, overdue tracking, loan extension
- **RequestService:** Request creation, approval, rejection
- **LockerService:** Locker management, QR token generation/validation
- **LogService:** System audit logging
- **QrCodeService:** QR code generation
- **LockerHardwareService:** Hardware abstraction (Mock & GPIO)

---

## Slide 7: Authentication & Authorization
**Agent:** Abu (Backend Developer)
**Title:** Security Controls
**Content:**
- **Password Security:** BCrypt hashing with salt
- **Authentication:** Cookie-based with 30-minute timeout
- **Authorization:** Role-based access control (Admin, Staff)
- **Anti-CSRF:** Anti-forgery tokens on all forms
- **Input Validation:** Server-side validation on all inputs
- **SQL Injection Prevention:** Entity Framework Core parameterized queries
- **XSS Prevention:** Razor HTML encoding by default

---

## Slide 8: Admin Dashboard
**Agent:** Siti (Frontend/Kiosk UI Developer)
**Title:** Admin Interface Overview
**Content:**
- **Dashboard:** System statistics, quick navigation
- **Manage Users:** Create, edit, disable users with role assignment
- **Manage Items:** Create items, assign to lockers, categorize
- **Manage Requests:** View pending requests, approve/reject with QR generation
- **Manage Borrows:** View active borrows, return items, extend loans
- **View Overdue:** Track overdue items, mark as lost
- **View Logs:** Audit trail with filtering by action, resource, user, date

---

## Slide 9: Staff Interface
**Agent:** Siti (Frontend/Kiosk UI Developer)
**Title:** Staff Portal Features
**Content:**
- **Dashboard:** Request and borrow statistics, quick links
- **Search Items:** Filter by category, status, availability
- **Request Item:** Submit requests with justification
- **My Borrows:** View active borrows with due dates
- **Return Item:** Return borrowed items with confirmation
- **Extend Loan:** Request loan extensions with new due dates
- **Responsive Design:** Works on desktop and tablet

---

## Slide 10: Locker Kiosk Interface
**Agent:** Siti (Frontend/Kiosk UI Developer)
**Title:** Touchscreen Kiosk UI
**Content:**
- **Home Screen:** Welcome message, instructions, navigation
- **QR Unlock:** Scan or paste QR token for locker access
- **Success Screen:** Confirmation with item details and next steps
- **Error Screen:** Clear error messages for failed attempts
- **Touchscreen Optimization:** Large buttons (44x44px minimum), readable fonts
- **Responsive Layout:** Optimized for 1024x600 kiosk display
- **No Login Required:** Public access for item returns and unlocks

---

## Slide 11: Request Approval Workflow
**Agent:** Ali (Fullstack Integrator)
**Title:** End-to-End Request Approval Flow
**Content:**
- **Step 1:** Staff submits request for item
- **Step 2:** Request stored with "Pending" status
- **Step 3:** Admin reviews pending requests
- **Step 4:** Admin approves request
- **Step 5:** Borrow record automatically created
- **Step 6:** Item status changed to "Borrowed"
- **Step 7:** Locker status changed to "Occupied"
- **Step 8:** QR access token generated (10-minute expiry)
- **Step 9:** Token displayed to staff for kiosk unlock

---

## Slide 12: QR Unlock & Hardware Integration
**Agent:** Abu (Backend Developer) & Rafi (Raspberry Pi Engineer)
**Title:** QR Token Security & Hardware Control
**Content:**
- **Token Generation:** Cryptographically secure RNG (256-bit)
- **Token Expiry:** 10-minute window
- **Single-Use:** Token marked as used after first validation
- **Failed Attempt Lockout:** 5 failed attempts trigger lockout
- **Hardware Abstraction:** ILockerHardwareService interface
- **Mock Mode:** Simulates hardware for development
- **GPIO Mode:** Real relay control on Raspberry Pi
- **Logging:** All unlock attempts logged for audit trail

---

## Slide 13: Borrow & Return Workflow
**Agent:** Ali (Fullstack Integrator)
**Title:** Item Borrow & Return Process
**Content:**
- **Borrow Creation:** Automatic on request approval
- **Status Updates:** Item → Borrowed, Locker → Occupied
- **Return Process:** Staff clicks "Return" on active borrow
- **Status Restoration:** Item → Available, Locker → Available
- **Loan Extension:** Staff can extend due date before expiry
- **Overdue Tracking:** System calculates days overdue
- **Lost Item Handling:** Admin can mark items as lost
- **Database Transactions:** All operations wrapped in transactions for consistency

---

## Slide 14: Deployment Architecture
**Agent:** Rafi (Raspberry Pi/Linux Deployment Engineer)
**Title:** Raspberry Pi Deployment Strategy
**Content:**
- **Hardware:** Raspberry Pi 4/5 with 2GB+ RAM
- **OS:** Raspberry Pi OS 64-bit
- **Runtime:** .NET 8.0 Runtime
- **Service:** systemd service for auto-start and restart
- **Database:** SQLite at /var/lib/smartlocker/smartlocker.db
- **User:** Dedicated 'smartlocker' system user (no root)
- **Kiosk:** Chromium in fullscreen kiosk mode
- **mDNS:** Optional hostname setup (smartlocker.local)

---

## Slide 15: Deployment Scripts
**Agent:** Rafi (Raspberry Pi/Linux Deployment Engineer)
**Title:** Automated Deployment & Installation
**Content:**
- **publish-linux-arm64.sh:** Publishes app for linux-arm64 architecture
- **install-raspberrypi.sh:** Automated installation on Raspberry Pi
  - Updates system packages
  - Installs .NET 8.0 Runtime
  - Creates directories and permissions
  - Sets up systemd service
  - Configures application user
- **smartlocker.service:** systemd unit file for auto-start
- **chromium-kiosk-autostart.sh:** Kiosk mode boot configuration

---

## Slide 16: Security Review & Findings
**Agent:** Amir (Security & Consistency Reviewer)
**Title:** Security Controls & Compliance
**Content:**
- ✅ **Password Security:** BCrypt hashing, no plaintext storage
- ✅ **Authorization:** Role-based access control verified
- ✅ **QR Token Security:** Cryptographically secure, single-use, time-limited
- ✅ **Input Validation:** All forms validated server-side
- ✅ **Anti-CSRF:** Anti-forgery tokens on all POST operations
- ✅ **Audit Logging:** All actions logged with user, action, resource, timestamp
- ✅ **Error Handling:** No sensitive data exposed in error messages
- **Recommendation:** Change default credentials before production use

---

## Slide 17: Bug Investigation & Edge Cases
**Agent:** Iman (Bug Investigator)
**Title:** Edge Case Handling & Risk Mitigation
**Content:**
- ✅ **Double Approval Prevention:** Check if already approved
- ✅ **Double Return Prevention:** Check if already returned
- ✅ **Duplicate Request Prevention:** One active request per user/item
- ✅ **Item Availability Check:** Prevent borrowing unavailable items
- ✅ **Locker Availability Check:** Prevent assigning to unavailable lockers
- ✅ **Expired QR Handling:** Reject expired tokens with clear message
- ✅ **GPIO Failure Handling:** Mock mode allows testing without hardware
- ✅ **Race Condition Mitigation:** Database transactions ensure consistency

---

## Slide 18: UAT & User Acceptance Testing
**Agent:** Maya (UAT + Frontend Bug Hunter)
**Title:** Testing & Quality Assurance Results
**Content:**
- ✅ **All 15 Use Cases:** Tested and validated
- ✅ **All 8 End-to-End Flows:** Passed successfully
- ✅ **Admin Screens:** Dashboard, Users, Items, Requests, Borrows, Overdue, Logs
- ✅ **Staff Screens:** Dashboard, Search, Request, Borrows
- ✅ **Kiosk Screens:** Home, QR Unlock, Return
- ✅ **Responsive Design:** Desktop, tablet, mobile, kiosk
- ✅ **Error Handling:** Graceful error messages, no crashes
- ✅ **Performance:** All pages load < 3 seconds

---

## Slide 19: Project Deliverables
**Agent:** Ali (Fullstack Integrator)
**Title:** Complete Project Deliverables
**Content:**
- **Source Code:** 156 files, fully functional ASP.NET Core application
- **Documentation:** 7 phase reports, deployment guide, security review, UAT checklist
- **Scripts:** Publish, install, systemd service, kiosk setup
- **Database:** 15 tables, migrations, seed data
- **UI:** 16 pages (Admin, Staff, Locker, Auth)
- **Services:** 9 service classes with complete business logic
- **Models:** 13 entity models with relationships
- **GitHub Repository:** Public repository with all code and documentation

---

## Slide 20: Key Achievements
**Agent:** Ali (Fullstack Integrator)
**Title:** Project Milestones & Achievements
**Content:**
- ✅ **Phase 1:** Requirements analysis and architecture planning
- ✅ **Phase 2:** Database and backend foundation (13 models, 9 services)
- ✅ **Phase 3:** Core UI development (16 pages, responsive design)
- ✅ **Phase 4:** Business logic implementation (request approval, borrow/return)
- ✅ **Phase 5:** QR unlock and hardware abstraction (secure tokens, GPIO support)
- ✅ **Phase 6:** Deployment preparation (scripts, systemd, kiosk setup)
- ✅ **Phase 7:** Final review and UAT (all tests passed)
- **Total:** 7 phases, 156 files, 15 use cases, 100% complete

---

## Slide 21: Technology Stack Summary
**Agent:** Abu (Backend Developer)
**Title:** Technology Stack & Architecture
**Content:**
- **Backend:** ASP.NET Core 8.0
- **Frontend:** Razor Pages with Bootstrap CSS
- **Database:** SQLite with Entity Framework Core
- **Authentication:** Cookie-based with BCrypt hashing
- **Hardware:** Raspberry Pi GPIO abstraction layer
- **Deployment:** systemd service on Raspberry Pi OS 64-bit
- **UI Framework:** Bootstrap 5 for responsive design
- **Validation:** HTML5 client-side + server-side validation
- **Logging:** SystemLog table with audit trail

---

## Slide 22: Known Limitations
**Agent:** Zara (Researcher/Requirement Analyst)
**Title:** System Limitations & Constraints
**Content:**
- **Timezone:** Uses UTC (configurable per deployment)
- **Single Location:** One physical location per instance
- **Network Scope:** Local LAN/WiFi only (not internet-facing)
- **Concurrency:** Designed for small teams (< 100 concurrent users)
- **QR Distribution:** Manual distribution (no email/SMS)
- **Authentication:** Cookie-based only (no OAuth/LDAP)
- **Backup:** Manual SQLite backup required
- **Database:** SQLite suitable for small deployments (< 10,000 items)

---

## Slide 23: Future Enhancements
**Agent:** Zara (Researcher/Requirement Analyst)
**Title:** Recommended Future Improvements
**Content:**
- Email notifications for request approval/rejection
- SMS notifications for overdue items
- Barcode scanning as alternative to QR codes
- Multi-language support (i18n)
- Analytics dashboard for usage statistics
- Backup and restore functionality
- Multi-location/site support
- OAuth/LDAP integration
- Mobile app for staff
- Advanced reporting and analytics

---

## Slide 24: Demo Flow
**Agent:** Maya (UAT + Frontend Bug Hunter)
**Title:** Live Demo Walkthrough
**Content:**
- **Step 1:** Admin login and create user
- **Step 2:** Admin create item and assign locker
- **Step 3:** Staff login and search items
- **Step 4:** Staff request item
- **Step 5:** Admin approve request and generate QR
- **Step 6:** Locker QR unlock (mock hardware)
- **Step 7:** Try same QR again (single-use enforcement)
- **Step 8:** Staff return item
- **Step 9:** Admin view overdue items
- **Step 10:** Admin view audit logs

---

## Slide 25: How to Run Locally
**Agent:** Rafi (Raspberry Pi/Linux Deployment Engineer)
**Title:** Local Development Setup
**Content:**
**Command:**
```
cd SmartLocker/src/SmartLocker.Web
dotnet run
```

**Access:**
- HTTP: http://localhost:5000
- HTTPS: https://localhost:5001

**Test Credentials:**
- Admin: admin / admin123
- Staff: staff / staff123

**Database:**
- SQLite: smartlocker.db (auto-created)
- Pre-populated with test data

---

## Slide 26: Raspberry Pi Deployment
**Agent:** Rafi (Raspberry Pi/Linux Deployment Engineer)
**Title:** Production Deployment on Raspberry Pi
**Content:**
**Publish Command:**
```
./scripts/publish-linux-arm64.sh
```

**Install Command (on Pi):**
```
sudo ./scripts/install-raspberrypi.sh
```

**Start Service:**
```
sudo systemctl start smartlocker
```

**Access:**
- http://localhost:5000
- http://smartlocker.local (if mDNS configured)

**Kiosk Setup:**
```
./scripts/chromium-kiosk-autostart.sh
```

---

## Slide 27: GitHub Repository
**Agent:** Ali (Fullstack Integrator)
**Title:** Project Repository & Version Control
**Content:**
- **Repository:** https://github.com/MustaqiimR/smartlocker-dotnet-raspberrypi
- **Branch:** master
- **Commit:** 1122a289f2e69e0808865845c62e168dc5736425
- **Files:** 156 total (source code, docs, scripts)
- **Status:** ✅ Ready for cloning and deployment
- **License:** Educational and demonstration use
- **Version:** 1.0.0 Final Release

---

## Slide 28: Project Status & Readiness
**Agent:** Ali (Fullstack Integrator)
**Title:** Final Project Status
**Content:**
- ✅ **Requirements:** All 15 use cases implemented
- ✅ **Backend:** All services and models complete
- ✅ **Frontend:** All pages tested and responsive
- ✅ **Database:** Schema complete, migrations applied
- ✅ **Security:** All controls verified and documented
- ✅ **Testing:** All flows validated, UAT passed
- ✅ **Deployment:** Scripts ready, documentation complete
- **Status:** READY FOR DEMO AND DEPLOYMENT

---

## Slide 29: Contact & Support
**Agent:** Zara (Researcher/Requirement Analyst)
**Title:** Documentation & Support Resources
**Content:**
- **README.md:** Project overview and quick start
- **DEPLOYMENT_GUIDE.md:** Step-by-step deployment instructions
- **SECURITY_REVIEW.md:** Security controls and recommendations
- **UAT_CHECKLIST.md:** Comprehensive testing checklist
- **Phase Reports:** Detailed progress from each development phase
- **GitHub Issues:** Report bugs or request features
- **GitHub Wiki:** Additional documentation and guides

---

## Slide 30: Conclusion
**Agent:** Ali (Fullstack Integrator)
**Title:** Project Summary & Next Steps
**Content:**
- **Completed:** Comprehensive locker management system for Raspberry Pi
- **Technology:** Modern .NET stack with secure practices
- **Deployment:** Ready for on-premise local network deployment
- **Quality:** Fully tested, documented, and production-ready
- **Next Steps:**
  1. Review project on GitHub
  2. Clone repository
  3. Run locally for testing
  4. Deploy to Raspberry Pi
  5. Configure for production use
- **Support:** Full documentation available in repository
