﻿using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Domain.SeedWork;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Domain.AggregateModels;

public class Employee : Entity<int>, IAggregateRoot
{
    private const int InactiveRcpId = 999;
    
    private Name _name;
    private Money _salary;
    private PercentageBonus _salaryBonusPercentage;
    internal int _rcpId;
    private Department _department;
    private int _departmentId;
    private JobInformation _jobInformation;
    internal bool _isActive;
    private readonly List<WorkingTimeRecord> _workingTimeRecords;
    private readonly List<WorkingTimeRecordAggregatedHistory> _workingTimeRecordAggregatedHistories;

    internal Employee()
    {
        _workingTimeRecords = new List<WorkingTimeRecord>();
        _workingTimeRecordAggregatedHistories = new List<WorkingTimeRecordAggregatedHistory>();
    }

    private Employee(Name name, Money salary, PercentageBonus salaryBonusPercentage, JobInformation jobInformation, int rcpId, int departmentId) 
        : this()
    {
        _name = name;
        _salary = salary;
        _salaryBonusPercentage = salaryBonusPercentage;
        _jobInformation = jobInformation;
        _rcpId = rcpId;
        _departmentId = departmentId;
    }
    
    public Name Name => _name;
    public Money Salary => _salary;
    public PercentageBonus SalaryBonusPercentage => _salaryBonusPercentage;
    public int RcpId => _rcpId;
    public Department Department => _department;
    public int DepartmentId => _departmentId;
    public JobInformation JobInformation => _jobInformation;
    public bool IsActive => _isActive;
    public virtual IReadOnlyCollection<WorkingTimeRecord> WorkingTimeRecords => _workingTimeRecords;
    public virtual IReadOnlyCollection<WorkingTimeRecordAggregatedHistory> WorkingTimeRecordAggregatedHistories => _workingTimeRecordAggregatedHistories;

    public static Employee Add(Name name, Money salary, PercentageBonus percentageSalaryBonus, JobInformation jobInformation, int rcpId, int departmentId)
    {
        return new Employee(name, salary, percentageSalaryBonus, jobInformation, rcpId, departmentId);
    }
    
    public void UpdateData(Name name, Money salary, PercentageBonus salaryPercentageBonus, JobInformation jobInformation, int rcpId, int departmentId)
    {
        _name = name;
        _salary = salary;
        _salaryBonusPercentage = salaryPercentageBonus;
        _jobInformation = jobInformation;
        _rcpId = rcpId;
        _departmentId = departmentId;
    }
    
    public void UpdateSummaryData(int year, int month, decimal percentageBonusSalary, Money holidaySalary, Money sicknessSalary, Money additionalSalary, ISalaryCalculationDomainService salaryCalculationDomainService)
    {
        var recordsToUpdate = WorkingTimeRecordAggregatedHistories
            .Where(x => x.Date.Month == month)
            .Where(x => x.Date.Year == year)
            .ToList();
        
        // TODO: This will not be needed when updating historical base salary will be possible (will be passed as an argument of this method)
        var baseSalary = recordsToUpdate.First().SalaryInformation.BaseSalary;
        
        var recalculatedRecord = salaryCalculationDomainService.RecalculateHistoricalSalary(
            baseSalary, percentageBonusSalary, 
            holidaySalary.Amount, sicknessSalary.Amount, additionalSalary.Amount, 
            recordsToUpdate);
        
        recordsToUpdate.ForEach(record =>
        {
            record.SalaryInformation.BaseSalary = recalculatedRecord.BaseSalary;
            record.SalaryInformation.PercentageBonusSalary = recalculatedRecord.PercentageBonusSalary;
            
            record.SalaryInformation.Base50PercentageSalary = recalculatedRecord.Base50PercentageSalary;
            record.SalaryInformation.Base100PercentageSalary = recalculatedRecord.Base100PercentageSalary;
            record.SalaryInformation.BaseSaturdaySalary = recalculatedRecord.BaseSaturdaySalary;
            
            record.SalaryInformation.GrossBaseSalary = recalculatedRecord.GrossBaseSalary;
            record.SalaryInformation.GrossBase50PercentageSalary = recalculatedRecord.GrossBase50PercentageSalary;
            record.SalaryInformation.GrossBase100PercentageSalary = recalculatedRecord.GrossBase100PercentageSalary;
            record.SalaryInformation.GrossBaseSaturdaySalary = recalculatedRecord.GrossBaseSaturdaySalary;
            
            record.SalaryInformation.BonusBaseSalary = recalculatedRecord.BonusBaseSalary;
            record.SalaryInformation.BonusBase50PercentageSalary = recalculatedRecord.BonusBase50PercentageSalary;
            record.SalaryInformation.BonusBase100PercentageSalary = recalculatedRecord.BonusBase100PercentageSalary;
            record.SalaryInformation.BonusBaseSaturdaySalary = recalculatedRecord.BonusBaseSaturdaySalary;
            
            record.SalaryInformation.GrossSumBaseSalary = recalculatedRecord.GrossSumBaseSalary;
            record.SalaryInformation.GrossSumBase50PercentageSalary = recalculatedRecord.GrossSumBase50PercentageSalary;
            record.SalaryInformation.GrossSumBase100PercentageSalary = recalculatedRecord.GrossSumBase100PercentageSalary;
            record.SalaryInformation.GrossSumBaseSaturdaySalary = recalculatedRecord.GrossSumBaseSaturdaySalary;
            
            record.SalaryInformation.BonusSumSalary = recalculatedRecord.BonusSumSalary;
            record.SalaryInformation.NightBaseSalary = recalculatedRecord.NightBaseSalary;
            record.SalaryInformation.NightWorkedHours = recalculatedRecord.NightWorkedHours;
            
            record.SalaryInformation.HolidaySalary = recalculatedRecord.HolidaySalary;
            record.SalaryInformation.SicknessSalary = recalculatedRecord.SicknessSalary;
            record.SalaryInformation.AdditionalSalary = recalculatedRecord.AdditionalSalary;
            record.SalaryInformation.FinalSumSalary = recalculatedRecord.FinalSumSalary;
        });
    }

    public void UpdateDetailsData(
        int year, 
        int month, 
        IDictionary<int, double?> dayHoursDictionary,
        IDictionary<int, double?> saturdayHoursDictionary,
        IDictionary<int, double?> nightHoursDictionary,
        IEmployeeWorkingTimeRecordCalculationDomainService calculationDomainService)
    {
        var recordsToUpdate = WorkingTimeRecordAggregatedHistories
            .Where(x => x.Date.Month == month)
            .Where(x => x.Date.Year == year)
            .ToList();
        
        recordsToUpdate.ForEach(record =>
        {
            var workedHours = dayHoursDictionary[record.Date.Day] ?? 0;
            var saturdayHours = saturdayHoursDictionary[record.Date.Day] ?? 0;
            var nightHours = nightHoursDictionary[record.Date.Day] ?? 0;
            
            if (Math.Abs(record.WorkedHoursRounded - workedHours) < double.Epsilon &&
                Math.Abs(record.SaturdayHours - saturdayHours) < double.Epsilon &&
                Math.Abs(record.NightHours - nightHours) < double.Epsilon)
                return; // Nothing has changed

            record.WorkedHoursRounded = workedHours;
            
            // We zeros the start and finish times because we don't know that after edit - this is temporarily disabled due to business needs
            // record.StartOriginalAt = record.Date;
            // record.FinishOriginalAt = record.Date;

            record.BaseNormativeHours = calculationDomainService.GetBaseNormativeHours(record.Date, record.Date, workedHours);
            record.FiftyPercentageBonusHours = calculationDomainService.GetFiftyPercentageBonusHours(record.Date, record.Date, workedHours);
            record.HundredPercentageBonusHours = calculationDomainService.GetHundredPercentageBonusHours(record.Date, record.Date, workedHours);
            record.SaturdayHours = saturdayHours;
            record.NightHours = nightHours;
        });
    }
    
    public void AddWorkingTimeRecord(WorkingTimeRecord record)
    {
        if (WorkingTimeRecords
            .Where(x => x.EventType == record.EventType)
            .Where(x => x.OccuredAt == record.OccuredAt)
            .Any(x => x.DepartmentId == record.DepartmentId))
        {
            return;
        }
        
        _workingTimeRecords.Add(record);
    }
    
    public void AddWorkingTimeRecordAggregatedHistory(WorkingTimeRecordAggregatedHistory record)
    {
        if (WorkingTimeRecordAggregatedHistories.Any(x => x.Date == record.Date))
            return;
        
        _workingTimeRecordAggregatedHistories.Add(record);
    }

    public void ClearAllWorkingTimeRecords()
    {
        _workingTimeRecords.Clear();
        _workingTimeRecordAggregatedHistories.Clear();
    }

    public void Deactivate()
    {
        if (!_isActive)
            throw new InvalidOperationException("Pracownik został już zdeaktywowany.");
        
        _isActive = false;
        _rcpId = InactiveRcpId;
    }
}