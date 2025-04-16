UPDATE wtr
SET 
    wtr.DepartmentId = e.DepartmentId,
    wtr.IsActive = e.IsActive,
    wtr.ShiftType = e.ShiftType
FROM Boma.dbo.WorkingTimeRecordAggregatedHistories wtr
JOIN Employees e ON wtr.UserId = e.Id