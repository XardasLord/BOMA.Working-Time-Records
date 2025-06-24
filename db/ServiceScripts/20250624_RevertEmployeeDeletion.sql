-- Radosław Suckiel
update [Employees]
set 
	RcpId = 167,
	IsActive = 1
where Id = (SELECT TOP 1 Id FROM Employees where LastName = 'SUCKIEL');