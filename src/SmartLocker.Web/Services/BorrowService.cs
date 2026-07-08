using Microsoft.EntityFrameworkCore;
using SmartLocker.Web.Data;
using SmartLocker.Web.Models;

namespace SmartLocker.Web.Services
{
    public class BorrowService
    {
        private readonly SmartLockerDbContext _context;

        public BorrowService(SmartLockerDbContext context)
        {
            _context = context;
        }

        public List<Borrow> GetAllBorrows()
        {
            return _context.Borrows
                .Include(b => b.User)
                .Include(b => b.Item)
                .Include(b => b.Locker)
                .Include(b => b.BorrowStatus)
                .OrderByDescending(b => b.BorrowStartDate)
                .ToList();
        }

        public Borrow GetBorrowById(int borrowId)
        {
            return _context.Borrows
                .Include(b => b.User)
                .Include(b => b.Item)
                .Include(b => b.Locker)
                .Include(b => b.BorrowStatus)
                .FirstOrDefault(b => b.BorrowId == borrowId);
        }

        public List<Borrow> GetActiveBorrows()
        {
            var activeStatus = _context.BorrowStatuses.FirstOrDefault(s => s.BorrowStatusName == "Active");
            if (activeStatus == null)
            {
                return new List<Borrow>();
            }

            return _context.Borrows
                .Include(b => b.User)
                .Include(b => b.Item)
                .Include(b => b.Locker)
                .Include(b => b.BorrowStatus)
                .Where(b => b.BorrowStatusId == activeStatus.BorrowStatusId)
                .OrderByDescending(b => b.BorrowStartDate)
                .ToList();
        }

        public List<Borrow> GetOverdueBorrows()
        {
            var activeStatus = _context.BorrowStatuses.FirstOrDefault(s => s.BorrowStatusName == "Active");
            if (activeStatus == null)
            {
                return new List<Borrow>();
            }

            return _context.Borrows
                .Include(b => b.User)
                .Include(b => b.Item)
                .Include(b => b.Locker)
                .Include(b => b.BorrowStatus)
                .Where(b => b.BorrowStatusId == activeStatus.BorrowStatusId && 
                           b.BorrowEndDate < DateTime.UtcNow)
                .OrderBy(b => b.BorrowEndDate)
                .ToList();
        }

        public List<Borrow> GetUserBorrows(int userId)
        {
            return _context.Borrows
                .Include(b => b.User)
                .Include(b => b.Item)
                .Include(b => b.Locker)
                .Include(b => b.BorrowStatus)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BorrowStartDate)
                .ToList();
        }

        public List<Borrow> GetUserActiveBorrows(int userId)
        {
            var activeStatus = _context.BorrowStatuses.FirstOrDefault(s => s.BorrowStatusName == "Active");
            if (activeStatus == null)
            {
                return new List<Borrow>();
            }

            return _context.Borrows
                .Include(b => b.User)
                .Include(b => b.Item)
                .Include(b => b.Locker)
                .Include(b => b.BorrowStatus)
                .Where(b => b.UserId == userId && b.BorrowStatusId == activeStatus.BorrowStatusId)
                .OrderByDescending(b => b.BorrowStartDate)
                .ToList();
        }

        public Borrow CreateBorrow(int userId, int itemId, int lockerId, int requestId, DateTime dueDate)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Validate item and locker
                    var item = _context.Items.Include(i => i.ItemStatus).FirstOrDefault(i => i.ItemId == itemId);
                    var locker = _context.Lockers.Include(l => l.LockerStatus).FirstOrDefault(l => l.LockerId == lockerId);
                    
                    if (item == null)
                        throw new Exception("Item not found");
                    if (locker == null)
                        throw new Exception("Locker not found");

                    var activeStatus = _context.BorrowStatuses.FirstOrDefault(s => s.BorrowStatusName == "Active");
                    if (activeStatus == null)
                        throw new Exception("Active status not found");

                    // Create borrow record
                    var borrow = new Borrow
                    {
                        UserId = userId,
                        ItemId = itemId,
                        LockerId = lockerId,
                        BorrowStatusId = activeStatus.BorrowStatusId,
                        RequestId = requestId,
                        BorrowStartDate = DateTime.UtcNow,
                        BorrowEndDate = dueDate,
                        IsOverdue = false
                    };

                    _context.Borrows.Add(borrow);
                    _context.SaveChanges();

                    // Update item status to Borrowed
                    var borrowedStatus = _context.ItemStatuses.FirstOrDefault(s => s.ItemStatusName == "Borrowed");
                    if (borrowedStatus != null)
                    {
                        item.ItemStatusId = borrowedStatus.ItemStatusId;
                        item.UpdatedAt = DateTime.UtcNow;
                    }

                    // Update locker status to Occupied
                    var occupiedStatus = _context.LockerStatuses.FirstOrDefault(s => s.LockerStatusName == "Occupied");
                    if (occupiedStatus != null)
                    {
                        locker.LockerStatusId = occupiedStatus.LockerStatusId;
                        locker.UpdatedAt = DateTime.UtcNow;
                    }

                    _context.SaveChanges();
                    transaction.Commit();

                    return borrow;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void ReturnBorrow(int borrowId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var borrow = GetBorrowById(borrowId);
                    if (borrow == null)
                        throw new Exception("Borrow not found");

                    // Validate borrow is active
                    var activeStatus = _context.BorrowStatuses.FirstOrDefault(s => s.BorrowStatusName == "Active");
                    if (borrow.BorrowStatusId != activeStatus?.BorrowStatusId)
                        throw new Exception("Borrow is not active");

                    var returnedStatus = _context.BorrowStatuses.FirstOrDefault(s => s.BorrowStatusName == "Returned");
                    if (returnedStatus == null)
                        throw new Exception("Returned status not found");

                    // Update borrow status
                    borrow.BorrowStatusId = returnedStatus.BorrowStatusId;
                    borrow.BorrowReturnDate = DateTime.UtcNow;
                    borrow.UpdatedAt = DateTime.UtcNow;

                    // Update item status to Available
                    var item = _context.Items.FirstOrDefault(i => i.ItemId == borrow.ItemId);
                    var availableStatus = _context.ItemStatuses.FirstOrDefault(s => s.ItemStatusName == "Available");
                    if (item != null && availableStatus != null)
                    {
                        item.ItemStatusId = availableStatus.ItemStatusId;
                        item.UpdatedAt = DateTime.UtcNow;
                    }

                    // Update locker status to Available
                    var locker = _context.Lockers.FirstOrDefault(l => l.LockerId == borrow.LockerId);
                    var lockerAvailableStatus = _context.LockerStatuses.FirstOrDefault(s => s.LockerStatusName == "Available");
                    if (locker != null && lockerAvailableStatus != null)
                    {
                        locker.LockerStatusId = lockerAvailableStatus.LockerStatusId;
                        locker.UpdatedAt = DateTime.UtcNow;
                    }

                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void ExtendLoan(int borrowId, DateTime newDueDate)
        {
            var borrow = GetBorrowById(borrowId);
            if (borrow == null)
            {
                throw new Exception("Borrow not found");
            }

            // Validate borrow is active
            var activeStatus = _context.BorrowStatuses.FirstOrDefault(s => s.BorrowStatusName == "Active");
            if (borrow.BorrowStatusId != activeStatus?.BorrowStatusId)
            {
                throw new Exception("Borrow is not active");
            }

            // Validate new date is in the future
            if (newDueDate <= DateTime.UtcNow)
            {
                throw new Exception("New due date must be in the future");
            }

            borrow.BorrowEndDate = newDueDate;
            borrow.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
        }

        public void MarkAsLost(int borrowId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var borrow = GetBorrowById(borrowId);
                    if (borrow == null)
                        throw new Exception("Borrow not found");

                    var lostStatus = _context.BorrowStatuses.FirstOrDefault(s => s.BorrowStatusName == "Lost");
                    if (lostStatus == null)
                        throw new Exception("Lost status not found");

                    // Update borrow status
                    borrow.BorrowStatusId = lostStatus.BorrowStatusId;
                    borrow.UpdatedAt = DateTime.UtcNow;

                    // Update item status to Lost
                    var item = _context.Items.FirstOrDefault(i => i.ItemId == borrow.ItemId);
                    var itemLostStatus = _context.ItemStatuses.FirstOrDefault(s => s.ItemStatusName == "Lost");
                    if (item != null && itemLostStatus != null)
                    {
                        item.ItemStatusId = itemLostStatus.ItemStatusId;
                        item.UpdatedAt = DateTime.UtcNow;
                    }

                    // Update locker status to Available (item is lost, locker is free)
                    var locker = _context.Lockers.FirstOrDefault(l => l.LockerId == borrow.LockerId);
                    var availableStatus = _context.LockerStatuses.FirstOrDefault(s => s.LockerStatusName == "Available");
                    if (locker != null && availableStatus != null)
                    {
                        locker.LockerStatusId = availableStatus.LockerStatusId;
                        locker.UpdatedAt = DateTime.UtcNow;
                    }

                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void UpdateOverdueStatus()
        {
            var activeStatus = _context.BorrowStatuses.FirstOrDefault(s => s.BorrowStatusName == "Active");
            var overdueStatus = _context.BorrowStatuses.FirstOrDefault(s => s.BorrowStatusName == "Overdue");

            if (activeStatus == null || overdueStatus == null)
            {
                return;
            }

            var overdueBorrows = _context.Borrows
                .Where(b => b.BorrowStatusId == activeStatus.BorrowStatusId && 
                           b.BorrowEndDate < DateTime.UtcNow)
                .ToList();

            foreach (var borrow in overdueBorrows)
            {
                borrow.IsOverdue = true;
                borrow.BorrowStatusId = overdueStatus.BorrowStatusId;
                borrow.UpdatedAt = DateTime.UtcNow;
            }

            _context.SaveChanges();
        }
    }
}
