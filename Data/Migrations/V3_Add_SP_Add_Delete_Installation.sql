use PrinterAccountingDb;
GO

CREATE PROCEDURE AddInstallation
    @Name NVARCHAR(50),
    @BranchId INT,
    @DeviceId INT,
    @SerialNumber INT = NULL,
    @IsDefault BIT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF @SerialNumber IS NULL
        BEGIN
            SET @SerialNumber = (
                SELECT ISNULL(MAX(SerialNumber), 0) + 1
                FROM Installations
                WHERE BranchId = @BranchId
            );
        END

        IF @IsDefault = 1
        BEGIN
            -- Make isDefault = false for other installations in current branch
            UPDATE Installations
            SET IsDefault = 0
            WHERE BranchId = @BranchId AND IsDefault = 1;
        END

        INSERT INTO Installations (Name, BranchId, DeviceId, SerialNumber, IsDefault)
        VALUES (@Name, @BranchId, @DeviceId, @SerialNumber, @IsDefault);

		DECLARE @NewInstallationId INT;
        SET @NewInstallationId = SCOPE_IDENTITY();

        SELECT Id, Name, BranchId, DeviceId, SerialNumber, IsDefault
        FROM Installations
        WHERE Id = @NewInstallationId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END

GO
CREATE PROCEDURE DeleteInstallation
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @IsDefault BIT;
        SET @IsDefault = (SELECT IsDefault FROM Installations WHERE Id = @Id);

        IF @IsDefault = 1
        BEGIN
            DECLARE @BranchId INT;
            SET @BranchId = (SELECT BranchId FROM Installations WHERE Id = @Id);

            IF EXISTS (
                SELECT 1
                FROM Installations
                WHERE Id <> @Id AND BranchId = @BranchId
            )
            BEGIN
                DECLARE @NewDefaultInstallationId INT;
                SET @NewDefaultInstallationId = (
                    SELECT TOP 1 Id
                    FROM Installations
                    WHERE Id <> @Id AND BranchId = @BranchId
                    ORDER BY Id
                );

                UPDATE Installations
                SET IsDefault = 1
                WHERE Id = @NewDefaultInstallationId;
            END
            ELSE
                THROW 50000, 'Cannot delete the last installation in the branch.', 1;
        END


        DELETE FROM Installations
        WHERE Id = @Id;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END