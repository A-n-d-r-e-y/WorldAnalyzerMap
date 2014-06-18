use master;
go

if (select is_broker_enabled from sys.databases where name = 'test') = 0
begin
	alter database test set enable_broker;
end

go

use test
go

create queue WorldAnalyzerQueue;

create service WorldDataChangeNotifications
  on queue WorldAnalyzerQueue
([http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification]);

go