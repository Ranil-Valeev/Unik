-- 1. Таблица Category (Категория)
CREATE TABLE Category (
    Category_ID INT PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL,
    Salary DECIMAL(10, 2)
);

---

-- 2. Таблица Teacher (Преподаватель)
CREATE TABLE Teacher (
    Teacher_ID INT PRIMARY KEY,
    LastName NVARCHAR(100) NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    MiddleName NVARCHAR(100),
    Category_ID INT,
    Street NVARCHAR(150),
    House NVARCHAR(10),
    Apartment NVARCHAR(10),
    FOREIGN KEY (Category_ID) REFERENCES Category(Category_ID)
);

---

-- 3. Таблица Cycle (Цикл)
CREATE TABLE Cycle (
    Cycle_ID INT PRIMARY KEY,
    CycleName NVARCHAR(100) NOT NULL
);

---

-- 4. Таблица Subject (Предмет)
CREATE TABLE Subject (
    Subject_ID INT PRIMARY KEY,
    SubjectName NVARCHAR(150) NOT NULL,
    Cycle_ID INT,
    FOREIGN KEY (Cycle_ID) REFERENCES Cycle(Cycle_ID)
);

---

-- 5. Таблица Load (Нагрузка) - Связующая таблица для Teacher и Subject
CREATE TABLE Load (
    Load_ID INT PRIMARY KEY,
    Subject_ID INT NOT NULL,
    Teacher_ID INT NOT NULL,
    FOREIGN KEY (Subject_ID) REFERENCES Subject(Subject_ID),
    FOREIGN KEY (Teacher_ID) REFERENCES Teacher(Teacher_ID)
);

---

-- 6. Таблица User (Пользователь)
-- Примечание: AUTO_INCREMENT используется в MySQL. Для SQL Server это IDENTITY(1,1).
CREATE TABLE Users (
    User_ID INT PRIMARY KEY,
    Login NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL -- Для хешированных паролей
);
CREATE TABLE AuditLog (
    AuditLog_ID INT PRIMARY KEY IDENTITY(1,1),
    TableName NVARCHAR(100) NOT NULL,
    Operation NVARCHAR(10) NOT NULL, -- INSERT, UPDATE, DELETE
    Record_ID NVARCHAR(100) NOT NULL, -- ID измененной записи
    New_Data NVARCHAR(MAX), -- JSON строка, содержащая старые/новые данные
    ChangedBy NVARCHAR(100),
    ChangedAt DATETIME DEFAULT GETDATE()
);

CREATE TRIGGER TeacherAuditTrg
ON Teacher
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Один оператор INSERT для всех трех типов операций
    INSERT INTO AuditLog (TableName, Operation, Record_ID, New_Data, ChangedBy, ChangedAt)
    SELECT
        'Teacher',
        -- Определяем операцию: 'UPDATE', 'INSERT' или 'DELETE'
        CASE
            WHEN EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted) THEN 'UPDATE'
            WHEN EXISTS (SELECT 1 FROM inserted) THEN 'INSERT'
            ELSE 'DELETE'
        END AS Operation,
        
        -- Выбираем ID из inserted или deleted в зависимости от операции
        CAST(COALESCE(i.Teacher_ID, d.Teacher_ID) AS NVARCHAR(100)) AS Record_ID,
        
        -- Форматируем JSON-данные
        CASE
            -- UPDATE: {"old": {...}, "new": {...}}
            WHEN EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted) THEN 
                JSON_QUERY(N'{"old": ' + (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER) + N', "new": ' + (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER) + N'}')
            -- INSERT: {...} (только новые данные)
            WHEN EXISTS (SELECT 1 FROM inserted) THEN 
                (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
            -- DELETE: {...} (только старые данные)
            ELSE 
                (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        END AS New_Data,

        SUSER_SNAME(),
        GETDATE()
    FROM inserted i
    FULL OUTER JOIN deleted d ON i.Teacher_ID = d.Teacher_ID;
END;