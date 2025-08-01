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
    private PersonalIdentityNumber _personalIdentityNumber;
    internal bool _isActive;
    private readonly List<WorkingTimeRecord> _workingTimeRecords;
    private readonly List<WorkingTimeRecordAggregatedHistory> _workingTimeRecordAggregatedHistories;

    internal Employee()
    {
        _workingTimeRecords = [];
        _workingTimeRecordAggregatedHistories = [];
    }

    private Employee(Name name, Money salary, PercentageBonus salaryBonusPercentage, JobInformation jobInformation, PersonalIdentityNumber personalIdentityNumber, int rcpId, int departmentId) 
        : this()
    {
        _name = name;
        _salary = salary;
        _salaryBonusPercentage = salaryBonusPercentage;
        _jobInformation = jobInformation;
        _personalIdentityNumber = personalIdentityNumber;
        _rcpId = rcpId;
        _departmentId = departmentId;
        _isActive = true;
    }
    
    public Name Name => _name;
    public Money Salary => _salary;
    public PercentageBonus SalaryBonusPercentage => _salaryBonusPercentage;
    public int RcpId => _rcpId;
    public Department Department => _department;
    public int DepartmentId => _departmentId;
    public JobInformation JobInformation => _jobInformation;
    public PersonalIdentityNumber PersonalIdentityNumber => _personalIdentityNumber;
    public bool IsActive => _isActive;
    public virtual IReadOnlyCollection<WorkingTimeRecord> WorkingTimeRecords => _workingTimeRecords;
    public virtual IReadOnlyCollection<WorkingTimeRecordAggregatedHistory> WorkingTimeRecordAggregatedHistories => _workingTimeRecordAggregatedHistories;

    public static Employee Add(Name name, Money salary, PercentageBonus percentageSalaryBonus, JobInformation jobInformation, PersonalIdentityNumber personalIdentityNumber, int rcpId, int departmentId)
    {
        return new Employee(name, salary, percentageSalaryBonus, jobInformation, personalIdentityNumber, rcpId, departmentId);
    }
    
    public void UpdateData(Name name, Money salary, PercentageBonus salaryPercentageBonus, JobInformation jobInformation, PersonalIdentityNumber personalIdentityNumber, int rcpId, int departmentId)
    {
        _name = name;
        _salary = salary;
        _salaryBonusPercentage = salaryPercentageBonus;
        _jobInformation = jobInformation;
        _personalIdentityNumber = personalIdentityNumber;
        _rcpId = rcpId;
        _departmentId = departmentId;
    }
    
    public async Task UpdateSummaryData(
        int year, int month,
        decimal baseSalary, decimal percentageBonusSalary,
        Money holidaySalary, Money sicknessSalary, Money additionalSalary, Money minSalaryCompensationAmount,
        ISalaryCalculationDomainService salaryCalculationDomainService)
    {
        var recordsToUpdate = WorkingTimeRecordAggregatedHistories
            .Where(x => x.Date.Month == month)
            .Where(x => x.Date.Year == year)
            .ToList();
        
        var recalculatedRecord = await salaryCalculationDomainService.RecalculateHistoricalSalary(
            baseSalary, percentageBonusSalary, 
            holidaySalary.Amount, sicknessSalary.Amount, additionalSalary.Amount,  minSalaryCompensationAmount.Amount,
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
            
            if (record.SalaryInformation.MinSalaryCompensationAmount != recalculatedRecord.MinSalaryCompensationAmount)
            {
                record.SalaryInformation.MinSalaryCompensationFactor = 0;
            }
            
            record.SalaryInformation.MinSalaryCompensationAmount = recalculatedRecord.MinSalaryCompensationAmount;
            record.SalaryInformation.FinalSumSalary = recalculatedRecord.FinalSumSalary;
        });
    }

    public async Task UpdateDetailsData(
        int year, 
        int month, 
        IDictionary<int, double?> dayHoursDictionary,
        IDictionary<int, double?> saturdayHoursDictionary,
        IDictionary<int, double?> nightHoursDictionary,
        IEmployeeWorkingTimeRecordCalculationDomainService calculationDomainService,
        ISalaryCalculationDomainService salaryCalculationDomainService)
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

            record.IsEditedManually = true;
        });

        var first = recordsToUpdate.First();
        var recalculatedRecord = await salaryCalculationDomainService.RecalculateHistoricalSalary(
            first.SalaryInformation.BaseSalary, first.SalaryInformation.PercentageBonusSalary, 
            first.SalaryInformation.HolidaySalary, first.SalaryInformation.SicknessSalary, first.SalaryInformation.AdditionalSalary, first.SalaryInformation.MinSalaryCompensationAmount, 
            recordsToUpdate);
        
        UpdateAllHistorySalaryInformation(recordsToUpdate, recalculatedRecord);
    }

    private static void UpdateAllHistorySalaryInformation(List<WorkingTimeRecordAggregatedHistory> recordsToUpdate, EmployeeSalaryAggregatedHistory recalculatedRecord)
    {
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

    public bool AddWorkingTimeRecord(WorkingTimeRecord record)
    {
        if (WorkingTimeRecords
            .Where(x => x.EventType == record.EventType)
            .Where(x => x.OccuredAt == record.OccuredAt)
            .Any(x => x.DepartmentId == record.DepartmentId))
        {
            return false;
        }
        
        _workingTimeRecords.Add(record);

        return true;
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
            throw new InvalidOperationException("Pracownik został już zdezaktywowany.");
        
        _isActive = false;
        _rcpId = InactiveRcpId;
    }

    public void UpdateWorkingHours(int year, int month, IDictionary<int, string> reportEntryHours, IDictionary<int, string> reportExitHours)
    {
        if (WorkingTimeRecordAggregatedHistories.Any(x => x.Date.Month == month && x.Date.Year == year))
            throw new InvalidOperationException("Nie można edytować godzin pracy za zakończony już miesiąc");

        UpdateEntryRecords(year, month, reportEntryHours);
        UpdateExitRecords(year, month, reportEntryHours, reportExitHours);
    }

    private void UpdateEntryRecords(int year, int month, IDictionary<int, string> reportHours)
    {
        foreach (var reportHourDay in reportHours)
        {
            if (string.IsNullOrWhiteSpace(reportHourDay.Value))
                continue;
            
            var timeSpan = TimeSpan.Parse(reportHourDay.Value);
            var hours = timeSpan.Hours;
            var minutes = timeSpan.Minutes;
            var day = reportHourDay.Key;
            
            if (!IsDateValid(year, month, day))
                continue;
            
            var newOccuredAt = new DateTime(year, month, reportHourDay.Key, hours, minutes, 0);

            var entryRecords = WorkingTimeRecords
                .Where(x => x.OccuredAt.Month == month)
                .Where(x => x.OccuredAt.Year == year)
                .Where(x => x.OccuredAt.Day == reportHourDay.Key)
                .Where(x => x.EventType == RecordEventType.Entry)
                .ToList();
            
            if (entryRecords.Count == 0)
            {
                var entry = WorkingTimeRecord.Create(RecordEventType.Entry, newOccuredAt, _departmentId);
                entry.MarkAsEditedManually();

                AddWorkingTimeRecord(entry);
            }
            else
            {
                if (entryRecords.Count > 1)
                {
                    _workingTimeRecords.RemoveAll(x => 
                        x.OccuredAt.Month == month &&
                        x.OccuredAt.Year == year &&
                        x.OccuredAt.Day == reportHourDay.Key &&
                        x.EventType == RecordEventType.Entry);
                    
                    var entry = WorkingTimeRecord.Create(RecordEventType.Entry, newOccuredAt, _departmentId);
                    entry.MarkAsEditedManually();
                    
                    AddWorkingTimeRecord(entry);
                }
                else
                {
                    if (entryRecords.First().OccuredAt.Hour != newOccuredAt.Hour || entryRecords.First().OccuredAt.Minute != newOccuredAt.Minute)
                    {
                        entryRecords.First().OccuredAt = newOccuredAt;
                        entryRecords.First().MarkAsEditedManually();
                    }
                }
            }
        }
    }

    private void UpdateExitRecords(int year, int month, IDictionary<int, string> reportEntryHours, IDictionary<int, string> reportExitHours)
    {
        foreach (var reportHourDay in reportExitHours)
        {
            if (string.IsNullOrWhiteSpace(reportHourDay.Value))
                continue;
            
            var timeSpan = TimeSpan.Parse(reportHourDay.Value);
            var hours = timeSpan.Hours;
            var minutes = timeSpan.Minutes;
            var day = timeSpan.Hours is >= 0 and < 4 ? reportHourDay.Key + 1 : reportHourDay.Key;
            
            if (!IsDateValid(year, month, day))
                continue;
                
            var newOccuredAt = new DateTime(year, month, day, hours, minutes, 0);
            
            var exitRecords = WorkingTimeRecords
                .Where(x => x.OccuredAt.Month == month)
                .Where(x => x.OccuredAt.Year == year)
                .Where(x => x.OccuredAt.Day == newOccuredAt.Day)
                .Where(x => x.EventType == RecordEventType.Exit)
                .ToList();
            
            if (exitRecords.Count == 0)
            {
                var exit = WorkingTimeRecord.Create(RecordEventType.Exit, newOccuredAt, _departmentId);
                exit.MarkAsEditedManually();
                
                AddWorkingTimeRecord(exit);
            }
            else
            {
                if (exitRecords.Count > 1)
                {
                    if (exitRecords.Any(x => x.OccuredAt.Hour == newOccuredAt.Hour && x.OccuredAt.Minute == newOccuredAt.Minute))
                        continue;
                    
                    _workingTimeRecords.RemoveAll(x => 
                        x.OccuredAt.Month == month &&
                        x.OccuredAt.Year == year &&
                        x.OccuredAt.Day == newOccuredAt.Day &&
                        x.EventType == RecordEventType.Exit);
                    
                    var exit = WorkingTimeRecord.Create(RecordEventType.Exit, newOccuredAt, _departmentId);
                    exit.MarkAsEditedManually();
                    
                    AddWorkingTimeRecord(exit);
                }
                else
                {
                    if (exitRecords.Any(x => x.OccuredAt.Hour == newOccuredAt.Hour && x.OccuredAt.Minute == newOccuredAt.Minute))
                        continue;
                    
                    var entryTimeSpanForThisDay = TimeSpan.Parse(reportEntryHours[reportHourDay.Key]);

                    if (entryTimeSpanForThisDay.Hours < 15 && newOccuredAt.Hour > 18 && exitRecords.First().OccuredAt.Hour < 4)
                    {
                        // Szczególny przypadek, gdy pracownik wychodzi z pracy np o 22:00, a miał już wyjście tego samego dnia za dzień poprzedni o 02:00, więc traktujemy to jako zupełnie nowy wpis wyjścia na ten sam dzień
                        var exit = WorkingTimeRecord.Create(RecordEventType.Exit, newOccuredAt, _departmentId);
                        exit.MarkAsEditedManually();
                        
                        AddWorkingTimeRecord(exit);
                    }
                    else if (exitRecords.First().OccuredAt.Hour >= 18 && entryTimeSpanForThisDay.Hours >= 14 && newOccuredAt.Hour < 4)
                    {
                        // Jest już wpis tego dnia z wyjściem po godzinie 18 (wieczornym), a teraz wyjście jest o godzinie w nocy (00:00 - 04:00), czyli jest to niezależne wyjście tego dnia i należy dodać jako nowy wpis
                        var exit = WorkingTimeRecord.Create(RecordEventType.Exit, newOccuredAt, _departmentId);
                        exit.MarkAsEditedManually();
                        
                        AddWorkingTimeRecord(exit);
                    }
                    else
                    { 
                        exitRecords.First().OccuredAt = newOccuredAt;
                        exitRecords.First().MarkAsEditedManually();
                    }
                }
            }
        }
    }
    
    private bool IsDateValid(int year, int month, int day) {
        return day <= DateTime.DaysInMonth(year, month);
    }
}