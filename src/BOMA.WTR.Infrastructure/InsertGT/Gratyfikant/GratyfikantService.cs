using BOMA.WTR.Application.InsertGT.Gratyfikant;
using BOMA.WTR.Infrastructure.InsertGT.Gratyfikant.Exceptions;
using Microsoft.Extensions.Options;

namespace BOMA.WTR.Infrastructure.InsertGT.Gratyfikant;

internal class GratyfikantService : IGratyfikantService
{
    private readonly GratyfikantOptions _gratyfikantOptions;

    public GratyfikantService(IOptions<GratyfikantOptions> gratyfikantSettings)
    {
        _gratyfikantOptions = gratyfikantSettings.Value;
    }

    public async Task OpenGratyfikant()
    {
        await RunSTATask<bool>(() =>
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
        var gt = new InsERT.GT
        {
            Produkt = InsERT.ProduktEnum.gtaProduktGratyfikant,
            Serwer = _gratyfikantOptions.Server,
            Baza = _gratyfikantOptions.Database,
            Autentykacja = InsERT.AutentykacjaEnum.gtaAutentykacjaMieszana,
            Uzytkownik = _gratyfikantOptions.User,
            Operator = _gratyfikantOptions.Operator,
            OperatorHaslo = _gratyfikantOptions.OperatorPassword
        };

        if (gt.Uruchom((int)InsERT.UruchomDopasujEnum.gtaUruchomDopasuj, (int)InsERT.UruchomEnum.gtaUruchom) is not InsERT.Gratyfikant gratyfikant)
            throw new GratyfikantOperationException("Gratyfikant cannot be opened");

        return gratyfikant;
    }
}