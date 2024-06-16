use PrinterAccountingDb;

GO
CREATE TRIGGER trg_CheckDefaultInstallation
ON Installations
AFTER INSERT
AS
BEGIN
    -- Case 1: There is more than one entry with IsDefault = 1 for one branch in the request to add
    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE i.IsDefault = 1
        GROUP BY i.BranchId
        HAVING COUNT(*) > 1
    )
    BEGIN
        RAISERROR('Only one default installation is allowed per branch in a single insert operation.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Case 2: There is at least one record in the request with IsDefault = 1, but there is already such an entry in the table
    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE i.IsDefault = 1
        AND EXISTS (
            SELECT 1
            FROM Installations ins
            WHERE ins.Id NOT IN (SELECT Id FROM inserted) AND ins.BranchId = i.BranchId AND ins.IsDefault = 1
        )
    )
    BEGIN
        RAISERROR('An installation with IsDefault = 1 already exists for this branch.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Case 3: Only records with IsDefault = 0 are added to the request, but there are no installations for the branch yet
    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE NOT EXISTS (
            SELECT 1
            FROM Installations ins
            WHERE ins.Id NOT IN (SELECT Id FROM inserted) AND ins.BranchId = i.BranchId
        )
        AND NOT EXISTS (
            SELECT 1
            FROM inserted i2
            WHERE i2.BranchId = i.BranchId AND i2.IsDefault = 1
        )
    )
    BEGIN
        RAISERROR('The first installation for a branch must have IsDefault = 1.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END;
