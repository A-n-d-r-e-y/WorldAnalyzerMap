:SETVAR db "SQLNotificationTest"
--:SETVAR qu "TestChangeMessages"
--:SETVAR sc "TestChangeNotifications"
:SETVAR tb "dbo.TableToTrank"

CREATE DATABASE $(db);
GO

ALTER DATABASE $(db) SET ENABLE_BROKER;
GO

USE $(db)
GO

/*
CREATE QUEUE $(qu);
GO

CREATE SERVICE $(sc) ON QUEUE $(qu) ([http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification]);
GO
*/

CREATE TABLE $(tb) (id int IDENTITY(1, 1) PRIMARY KEY, msg NVARCHAR(100));

--DROP DATABASE $(db)

/*
select * from sys.dm_qn_subscriptions

insert into dbo.TableToTrank (msg) values ('hello')
update dbo.TableToTrank set msg = msg+'_'
*/