using BOMA.WTR.Infrastructure.InsertGT.Gratyfikant.Exceptions;

namespace BOMA.WTR.Infrastructure.InsertGT.Gratyfikant
{
    internal class GratyfikantService
    {
        public GratyfikantService()
        {
            
        }

        public void OpenGratyfikant()
        {
            var gratyfikant = RunGratyfikant();
        }

        private InsERT.Gratyfikant RunGratyfikant()
        {
            var gt = new InsERT.GT
            {
                Produkt = InsERT.ProduktEnum.gtaProduktGratyfikant,
                Serwer = "localhost\\SQLEXPRESS",
                Baza = "TEST_GRATYFIKANT",
                Autentykacja = InsERT.AutentykacjaEnum.gtaAutentykacjaMieszana,
                Uzytkownik = "sa",
                Operator = "szef",
                OperatorHaslo = ""
            };

            if (gt.Uruchom((int)InsERT.UruchomDopasujEnum.gtaUruchomDopasuj, (int)InsERT.UruchomEnum.gtaUruchom) is not InsERT.Gratyfikant gratyfikant)
                throw new GratyfikantOperationException("Gratyfikant cannot be opened");
            
            gratyfikant.Okno.Widoczne = true;

            return gratyfikant;
        }
    }
}
