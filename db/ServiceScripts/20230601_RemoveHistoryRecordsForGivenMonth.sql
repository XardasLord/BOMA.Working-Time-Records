delete from WorkingTimeRecordAggregatedHistories
where 
	DATEPART(month, [Date]) = 5 and 
	DATEPART(year, [Date]) = 2023