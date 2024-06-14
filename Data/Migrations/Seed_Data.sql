USE PrinterAccountingDb;

-- Заполнение таблицы Branches
INSERT INTO Branches (Name, Location) 
VALUES 
    (N'Тридевятое царство', N'Тридевятое царство'),
    (N'Дремучий Лес', N'Дремучий Лес'),
    (N'Луна', N'Луна');

-- Заполнение таблицы Devices
INSERT INTO Devices (Name, ConnectionType) 
VALUES 
    (N'Папирус', N'Локальное'),
    (N'Бумага', N'Локальное'),
    (N'Камень', N'Сетевое');

-- Получение ID для филиалов
DECLARE @BranchId_1 INT, @BranchId_2 INT, @BranchId_3 INT;
SELECT @BranchId_1 = Id FROM Branches WHERE Name = N'Тридевятое царство';
SELECT @BranchId_2 = Id FROM Branches WHERE Name = N'Дремучий Лес';
SELECT @BranchId_3 = Id FROM Branches WHERE Name = N'Луна';

-- Получение ID для устройств
DECLARE @DeviceId_1 INT, @DeviceId_2 INT, @DeviceId_3 INT;
SELECT @DeviceId_1 = Id FROM Devices WHERE Name = N'Папирус';
SELECT @DeviceId_2 = Id FROM Devices WHERE Name = N'Бумага';
SELECT @DeviceId_3 = Id FROM Devices WHERE Name = N'Камень';

-- Заполнение таблицы Installations
INSERT INTO Installations (Name, BranchId, SerialNumber, IsDefault, DeviceId) 
VALUES 
    (N'Дворец', @BranchId_1, 1, 1, @DeviceId_1),
    (N'Конюшни', @BranchId_1, 2, 0, @DeviceId_2),
    (N'Оружейная', @BranchId_1, 3, 0, @DeviceId_2),
    (N'Кратер', @BranchId_3, 4, 0, @DeviceId_3),
    (N'Избушка', @BranchId_2, 3, 0, @DeviceId_2),
    (N'Топи', @BranchId_2, 2, 1, @DeviceId_1);

-- Заполнение таблицы Employees
INSERT INTO Employees (BranchId, Name) 
VALUES 
    (@BranchId_1, N'Царь'),
    (@BranchId_1, N'Добрыня'),
    (@BranchId_2, N'Яга'),
    (@BranchId_2, N'Кощей'),
    (@BranchId_3, N'Копатыч'),
    (@BranchId_3, N'Лосяш');
