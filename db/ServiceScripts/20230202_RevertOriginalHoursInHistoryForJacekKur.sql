/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[Date]
	  ,concat('update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = ', WorkedHoursRounded, ', [BaseNormativeHours] = ', [BaseNormativeHours], ', [FiftyPercentageBonusHours] = ', [FiftyPercentageBonusHours], ', [HundredPercentageBonusHours] = ', [HundredPercentageBonusHours], ', [SaturdayHours] = ', [SaturdayHours], ', [NightHours] = ', [NightHours], ' where Id = ', [Id])
      ,[WorkedMinutes]
      ,[WorkedHoursRounded]
      ,[BaseNormativeHours]
      ,[FiftyPercentageBonusHours]
      ,[HundredPercentageBonusHours]
      ,[SaturdayHours]
      ,[NightHours]
      ,[SalaryInformation_BaseSalary]
      ,[SalaryInformation_Base50PercentageSalary]
      ,[SalaryInformation_Base100PercentageSalary]
      ,[SalaryInformation_BaseSaturdaySalary]
      ,[SalaryInformation_GrossBaseSalary]
      ,[SalaryInformation_GrossBase50PercentageSalary]
      ,[SalaryInformation_GrossBase100PercentageSalary]
      ,[SalaryInformation_GrossBaseSaturdaySalary]
      ,[SalaryInformation_BonusBaseSalary]
      ,[SalaryInformation_BonusBase50PercentageSalary]
      ,[SalaryInformation_BonusBase100PercentageSalary]
      ,[SalaryInformation_BonusBaseSaturdaySalary]
      ,[SalaryInformation_GrossSumBaseSalary]
      ,[SalaryInformation_GrossSumBase50PercentageSalary]
      ,[SalaryInformation_GrossSumBase100PercentageSalary]
      ,[SalaryInformation_GrossSumBaseSaturdaySalary]
      ,[SalaryInformation_BonusSumSalary]
      ,[SalaryInformation_NightBaseSalary]
      ,[UserId]
      ,[SalaryInformation_AdditionalSalary]
      ,[SalaryInformation_FinalSumSalary]
      ,[SalaryInformation_HolidaySalary]
      ,[SalaryInformation_NightWorkedHours]
      ,[SalaryInformation_SicknessSalary]
      ,[MissingRecordEventType]
      ,[FinishNormalizedAt]
      ,[FinishOriginalAt]
      ,[StartNormalizedAt]
      ,[StartOriginalAt]
  FROM [Boma].[dbo].[WorkingTimeRecordAggregatedHistories]
  where UserId = 52 and DATEPART(month, Date) = 1
  order by Id asc
  
 -- The above produces this scripts below
 update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 0.00, [BaseNormativeHours] = 0.00, [FiftyPercentageBonusHours] = 0.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29101
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 10.00, [BaseNormativeHours] = 8.00, [FiftyPercentageBonusHours] = 2.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29102
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 10.00, [BaseNormativeHours] = 8.00, [FiftyPercentageBonusHours] = 2.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29103
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 10.00, [BaseNormativeHours] = 8.00, [FiftyPercentageBonusHours] = 2.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29104
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 8.00, [BaseNormativeHours] = 8.00, [FiftyPercentageBonusHours] = 0.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29105
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 0.00, [BaseNormativeHours] = 0.00, [FiftyPercentageBonusHours] = 0.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29106
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 0.00, [BaseNormativeHours] = 0.00, [FiftyPercentageBonusHours] = 0.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29107
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 0.00, [BaseNormativeHours] = 0.00, [FiftyPercentageBonusHours] = 0.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29108
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 10.00, [BaseNormativeHours] = 8.00, [FiftyPercentageBonusHours] = 2.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29109
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 8.00, [BaseNormativeHours] = 8.00, [FiftyPercentageBonusHours] = 0.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29110
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 8.00, [BaseNormativeHours] = 8.00, [FiftyPercentageBonusHours] = 0.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29111
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 10.00, [BaseNormativeHours] = 8.00, [FiftyPercentageBonusHours] = 2.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29112
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 8.00, [BaseNormativeHours] = 8.00, [FiftyPercentageBonusHours] = 0.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29113
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 0.00, [BaseNormativeHours] = 0.00, [FiftyPercentageBonusHours] = 0.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 6.00, [NightHours] = 0.00 where Id = 29114
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 0.00, [BaseNormativeHours] = 0.00, [FiftyPercentageBonusHours] = 0.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29115
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 10.00, [BaseNormativeHours] = 8.00, [FiftyPercentageBonusHours] = 2.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29116
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 10.00, [BaseNormativeHours] = 8.00, [FiftyPercentageBonusHours] = 2.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29117
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 10.00, [BaseNormativeHours] = 8.00, [FiftyPercentageBonusHours] = 2.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29118
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 0.00, [BaseNormativeHours] = 0.00, [FiftyPercentageBonusHours] = 0.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29119
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 0.00, [BaseNormativeHours] = 0.00, [FiftyPercentageBonusHours] = 0.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29120
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 0.00, [BaseNormativeHours] = 0.00, [FiftyPercentageBonusHours] = 0.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29121
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 0.00, [BaseNormativeHours] = 0.00, [FiftyPercentageBonusHours] = 0.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29122
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 10.00, [BaseNormativeHours] = 8.00, [FiftyPercentageBonusHours] = 2.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29123
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 10.00, [BaseNormativeHours] = 8.00, [FiftyPercentageBonusHours] = 2.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29124
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 10.00, [BaseNormativeHours] = 8.00, [FiftyPercentageBonusHours] = 2.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29125
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 10.00, [BaseNormativeHours] = 8.00, [FiftyPercentageBonusHours] = 2.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29126
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 8.00, [BaseNormativeHours] = 8.00, [FiftyPercentageBonusHours] = 0.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29127
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 0.00, [BaseNormativeHours] = 0.00, [FiftyPercentageBonusHours] = 0.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 8.00, [NightHours] = 0.00 where Id = 29128
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 0.00, [BaseNormativeHours] = 0.00, [FiftyPercentageBonusHours] = 0.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29129
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 10.00, [BaseNormativeHours] = 8.00, [FiftyPercentageBonusHours] = 2.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29130
update dbo.WorkingTimeRecordAggregatedHistories set [WorkedHoursRounded] = 10.00, [BaseNormativeHours] = 8.00, [FiftyPercentageBonusHours] = 2.00, [HundredPercentageBonusHours] = 0.00, [SaturdayHours] = 0.00, [NightHours] = 0.00 where Id = 29131
 