CREATE TABLE AuditLog (
    AuditLog_ID INT PRIMARY KEY IDENTITY(1,1),
    Record_ID NVARCHAR(100) NOT NULL,    -- ID измененной записи (Teacher_ID)
    Operation NVARCHAR(10) NOT NULL,     -- Тип операции (INSERT, UPDATE, DELETE)
    ChangedAt DATETIME DEFAULT GETDATE()
);

CREATE TRIGGER TeachersAuditTrg -- Изменяем имя триггера
ON Teachers -- Используем имя Teachers
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO AuditLog (Record_ID, Operation, ChangedAt)
    SELECT
        CAST(COALESCE(i.Id_teacher, d.Id_teacher) AS NVARCHAR(100)) AS Record_ID, -- Используем Id_teacher
        CASE
            WHEN EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted) THEN 'UPDATE'
            WHEN EXISTS (SELECT 1 FROM inserted) THEN 'INSERT'
            ELSE 'DELETE'
        END AS Operation,
        GETDATE() AS ChangedAt
        
    FROM inserted i
    FULL OUTER JOIN deleted d 
        ON i.Id_teacher = d.Id_teacher; -- Связываем по Id_teacher
END;