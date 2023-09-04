update [dbo].[WorkingTimeRecordAggregatedHistories]
set 
	WorkedMinutes = 480,
	WorkedHoursRounded = 8,
	BaseNormativeHours = 8
where [Date] = '2023-05-17' and WorkedHoursRounded = 6