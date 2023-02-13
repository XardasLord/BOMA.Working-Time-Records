update [Employees]
set 
	RcpId = 250,
	IsActive = 1
where Id = 129;

delete from [WorkingTimeRecords] where UserId = 170;
delete from [Employees] where Id = 170;