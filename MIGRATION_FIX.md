# Migration Issue Fix

## Problem
The new migration `20260714145404_AddRequestDates.cs` is trying to insert seed data (BorrowStatuses, Categories, ItemStatuses, etc.) but these already exist in the database, causing UNIQUE constraint violations.

## Root Cause
- The migration was auto-generated with all seed data from OnModelCreating
- But the seed data is already in the database from previous runs
- When migration runs, it tries to insert duplicates

## Solution
Remove all InsertData calls from the migration, keeping only the AddColumn calls for RequestedStartDate and RequestedEndDate.

The migration should ONLY:
1. Add RequestedStartDate column to Requests table
2. Add RequestedEndDate column to Requests table

All seed data is already handled by OnModelCreating in SmartLockerDbContext.

## Files to Edit
- `/home/ubuntu/SmartLocker/src/SmartLocker.Web/Migrations/20260714145404_AddRequestDates.cs`

Keep only lines 14-26 in Up() method
Keep only lines 263-269 in Down() method
