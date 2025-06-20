-- Góryl
update [Employees]
set 
	RcpId = 598,
	IsActive = 1
where Id = (SELECT TOP 1 Id FROM Employees where LastName = 'GÓRYL');

-- Łysko
update [Employees]
set 
	RcpId = 606,
	IsActive = 1
where Id = (SELECT TOP 1 Id FROM Employees where LastName like '%YSKO%');