use PrinterAccountingDb;

CREATE UNIQUE INDEX IX_Installations_BranchId_IsDefault
ON Installations(BranchId)
WHERE IsDefault = 1;

GO

CREATE TRIGGER trg_CheckDefaultInstallation
ON Installations
AFTER INSERT
AS
BEGIN
    DECLARE @BranchId INT;
    
    -- Получить BranchId добавленной записи
    SELECT @BranchId = i.BranchId FROM inserted i;
    
    -- Проверить, если уже есть хотя бы одна инсталляция в филиале
    IF NOT EXISTS (SELECT 1 FROM Installations WHERE BranchId = @BranchId)
    BEGIN
        -- Если вставлена запись с IsDefault = 0, а филиал ещё не имеет инсталляций
        IF EXISTS (
            SELECT 1
            FROM inserted i
            WHERE i.IsDefault = 0
        )
        BEGIN
            -- Если вставлена запись с IsDefault = 0, вызываем ошибку и откатываем транзакцию
            RAISERROR('При добавлении первой инсталляции в филиале её необходимо установить как по умолчанию.', 16, 1);
            ROLLBACK TRANSACTION;
        END
    END
END;