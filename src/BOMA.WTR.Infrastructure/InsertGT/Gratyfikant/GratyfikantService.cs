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

    public async Task SetWorkingHours(IEnumerable<EmployeeWorkingTimeRecordViewModel> records)
    {
        await RunSTATask<bool>(() =>
        {
            var gratyfikant = RunGratyfikant();

            if (!gratyfikant.PracownicyManager.Istnieje("86100917703"))
                return false;
            
            var employee = gratyfikant.PracownicyManager.WczytajPracownika("86100917703");

            var rcp = gratyfikant.ECP.RCP(employee.Identyfikator, "2023-10-30") as RCP;

            var employeeWorkingRecordDetails = records
                .Single(x => x.Employee.LastName == "Bondarieva")
                .WorkingTimeRecordsAggregated;

            // var recordDetails = employeeWorkingRecordDetails.Single(x => x.Date.Day == 27);
            // var startTime = recordDetails.StartNormalizedAt;
            // var endTime = recordDetails.FinishNormalizedAt;
            //
            // if (!endTime.HasValue)
            //     return false;
            //
            // rcp.DodajOkresPracy(TotalMinutesSinceMidnight(startTime), TotalMinutesSinceMidnight(endTime.Value), GodzinyEnum.gtaGodzinyDzienne);
            //
            // rcp.PrzeliczycGodzinyPracy = true;
            // rcp.Zapisz();

            return true;
        });
    }

    private long TotalMinutesSinceMidnight(DateTime startDate)
    {
        var timeSinceMidnight = startDate - startDate.Date;
        
        var minutes = (long)timeSinceMidnight.TotalMinutes;

        return minutes;
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
        var gt = new InsERT.GT
        {
            Produkt = InsERT.ProduktEnum.gtaProduktGratyfikant,
            Serwer = _gratyfikantOptions.Server,
            Baza = _gratyfikantOptions.Database,
            Autentykacja = InsERT.AutentykacjaEnum.gtaAutentykacjaWindows,
            Uzytkownik = _gratyfikantOptions.User,
            Operator = _gratyfikantOptions.Operator,
            OperatorHaslo = _gratyfikantOptions.OperatorPassword
        };

        if (gt.Uruchom((int)InsERT.UruchomDopasujEnum.gtaUruchomDopasuj, (int)InsERT.UruchomEnum.gtaUruchom) is not InsERT.Gratyfikant gratyfikant)
            throw new GratyfikantOperationException("Gratyfikant cannot be opened");

        return gratyfikant;
    }
}