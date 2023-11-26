using BOMA.WTR.Application.InsertGT.Gratyfikant;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;
using BOMA.WTR.Infrastructure.InsertGT.Gratyfikant.Exceptions;
using InsERT;
using Microsoft.Extensions.Options;

namespace BOMA.WTR.Infrastructure.InsertGT.Gratyfikant;

internal class GratyfikantService : IGratyfikantService
{
    private readonly GratyfikantOptions _gratyfikantOptions;

    public GratyfikantService(IOptions<GratyfikantOptions> gratyfikantSettings)
    {
        _gratyfikantOptions = gratyfikantSettings.Value;
    }

    public async Task SetWorkingHours(IEnumerable<EmployeeWorkingTimeRecordViewModel> employeesRecords)
    {
        await RunSTATask(() =>
        {
            var gratyfikant = RunGratyfikant();

            var employeesWorkingTimeRecordViewModels = employeesRecords.ToList();
            
            foreach (var employeeRecord in employeesWorkingTimeRecordViewModels)
            {
                if (!gratyfikant.PracownicyManager.Istnieje(employeeRecord.Employee.PersonalIdentityNumber))
                    return false;
            
                var employee = gratyfikant.PracownicyManager.WczytajPracownika(employeeRecord.Employee.PersonalIdentityNumber);

                foreach (var workingTimeData in employeeRecord.WorkingTimeRecordsAggregated)
                {
                    var rcp = gratyfikant.ECP.RCP(employee.Identyfikator, workingTimeData.Date) as RCP 
                        ?? throw new GratyfikantOperationException($"Cannot instantiate RCP Gratyfikant object for {workingTimeData.Date} date for {employee.Imie} {employee.Nazwisko} employee");
                    
                    if (workingTimeData.DayWorkTimePeriodNormalized?.To is not null)
                    {
                        var startTime = workingTimeData.DayWorkTimePeriodNormalized.From;
                        var endTime = workingTimeData.DayWorkTimePeriodNormalized.To;

                        rcp.UsunZapisyZDnia();
                        rcp.DodajOkresPracy(TotalMinutesSinceMidnight(startTime), TotalMinutesSinceMidnight(endTime.Value), GodzinyEnum.gtaGodzinyDzienne);
                        rcp.PrzeliczycGodzinyPracy = true;

                        try
                        {
                            rcp.Zapisz();
                        }
                        catch (Exception ex)
                        {
                            throw new GratyfikantOperationException($"Error for date - {workingTimeData.Date} - for {employee.Imie} {employee.Nazwisko} employee - {ex.Message}");
                        }
                    }
                    
                    if (workingTimeData.NightWorkTimePeriodNormalized?.To is not null)
                    {
                        var startTime = workingTimeData.NightWorkTimePeriodNormalized.From;
                        var endTime = workingTimeData.NightWorkTimePeriodNormalized.To;

                        rcp.UsunZapisyZDnia();
                        rcp.DodajOkresPracy(TotalMinutesSinceMidnight(startTime), TotalMinutesSinceMidnight(endTime.Value), GodzinyEnum.gtaGodzinyNocne);
                        rcp.PrzeliczycGodzinyPracy = true;

                        try
                        {
                            rcp.Zapisz();
                        }
                        catch (Exception ex)
                        {
                            throw new GratyfikantOperationException($"Error for date - {workingTimeData.Date} - for {employee.Imie} {employee.Nazwisko} employee - {ex.Message}");
                        }
                    }
                }
            }

            return true;
        });
    }

    public async Task OpenGratyfikant()
    {
        await RunSTATask(() =>
        {
            var gratyfikant = RunGratyfikant();

            return true;
        });
    }

    private static Task<T> RunSTATask<T>(Func<T> function)
    {
        var task = new Task<T>(function, TaskCreationOptions.DenyChildAttach);
        var thread = new Thread(task.RunSynchronously)
        {
            IsBackground = true
        };
        
#pragma warning disable CA1416
        thread.SetApartmentState(ApartmentState.STA);
#pragma warning restore CA1416
        
        thread.Start();
        
        return task;
    }

    private InsERT.Gratyfikant RunGratyfikant()
    {
        var gt = new GT
        {
            Produkt = ProduktEnum.gtaProduktGratyfikant,
            Serwer = _gratyfikantOptions.Server,
            Baza = _gratyfikantOptions.Database,
            Autentykacja = AutentykacjaEnum.gtaAutentykacjaWindows,
            Uzytkownik = _gratyfikantOptions.User,
            Operator = _gratyfikantOptions.Operator,
            OperatorHaslo = _gratyfikantOptions.OperatorPassword
        };

        if (gt.Uruchom((int)UruchomDopasujEnum.gtaUruchomDopasuj, (int)UruchomEnum.gtaUruchom) is not InsERT.Gratyfikant gratyfikant)
            throw new GratyfikantOperationException("Gratyfikant cannot be opened");

        return gratyfikant;
    }

    private static long TotalMinutesSinceMidnight(DateTime startDate)
    {
        var timeSinceMidnight = startDate - startDate.Date;
        
        var minutes = (long)timeSinceMidnight.TotalMinutes;

        return minutes;
    }
}