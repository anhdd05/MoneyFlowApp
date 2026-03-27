-- Migration script for Admin features
-- Run this AFTER expense.sql on existing ExpenseManagement database
USE ExpenseManagement;
GO

-- 1. Users: Add is_banned, ensure last_active exists for LastLoginAt
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'is_banned')
BEGIN
    ALTER TABLE Users ADD is_banned BIT DEFAULT 0;
END
GO

-- 2. Budgets: Add allocated if not exists
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Budgets') AND name = 'allocated')
BEGIN
    ALTER TABLE Budgets ADD allocated DECIMAL(18,2) DEFAULT 0;
END
GO

-- 3. TransactionCategories: Add color, ensure icon exists
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('TransactionCategories') AND name = 'color')
BEGIN
    ALTER TABLE TransactionCategories ADD color VARCHAR(20) NULL;
END
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('TransactionCategories') AND name = 'icon')
BEGIN
    ALTER TABLE TransactionCategories ADD icon VARCHAR(255) NULL;
END
GO

-- 4. BudgetCategories: Add color
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('BudgetCategories') AND name = 'color')
BEGIN
    ALTER TABLE BudgetCategories ADD color VARCHAR(20) NULL;
END
GO

-- Fix: expense.sql uses user_id, category_id (snake_case) but EF uses different convention
-- Check actual column names in DB - EF Core By default maps to PascalCase
-- Our models use [Column("user_id")] so we need snake_case columns
-- The original expense.sql has: user_id, user_name, etc. - so we're good.
-- allocated: Budgets table - add if missing. Expense.sql has: amount, month, year, description, auto_renew, created_at
-- No allocated in original - our migration adds it.
GO
