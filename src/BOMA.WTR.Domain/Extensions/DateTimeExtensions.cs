namespace BOMA.WTR.Domain.Extensions;

public static class DateTimeExtensions
{
    private static readonly Dictionary<DateTime, bool> BankHolidays = new()
    {
        { new DateTime(2022, 6, 5), false },
        { new DateTime(2022, 6, 16), false },
        { new DateTime(2022, 8, 15), false },
        { new DateTime(2022, 11, 1), false },
        { new DateTime(2022, 11, 11), false },
        { new DateTime(2022, 12, 25), false },
        { new DateTime(2022, 12, 26), false },
        
        { new DateTime(2023, 1, 1), false },
        { new DateTime(2023, 1, 6), false },
        { new DateTime(2023, 4, 9), false },
        { new DateTime(2023, 4, 10), false },
        { new DateTime(2023, 5, 1), false },
        { new DateTime(2023, 5, 3), false },
        { new DateTime(2023, 5, 28), false },
        { new DateTime(2023, 6, 8), false },
        { new DateTime(2023, 8, 15), false },
        { new DateTime(2023, 11, 1), false },
        { new DateTime(2023, 11, 11), true },
        { new DateTime(2023, 12, 25), false },
        { new DateTime(2023, 12, 26), false },
        
        { new DateTime(2024, 1, 1), false },
        { new DateTime(2024, 1, 6), true },
        { new DateTime(2024, 3, 31), false },
        { new DateTime(2024, 4, 1), false },
        { new DateTime(2024, 5, 1), false },
        { new DateTime(2024, 5, 3), false },
        { new DateTime(2024, 5, 19), false },
        { new DateTime(2024, 5, 30), false },
        { new DateTime(2024, 8, 15), false },
        { new DateTime(2024, 11, 1), false },
        { new DateTime(2024, 11, 11), false },
        { new DateTime(2024, 12, 25), false },
        { new DateTime(2024, 12, 26), false },
    };
    
    public static int WeekDaysInMonth(this DateTime dateTime)
    {
        var days = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
        var dates = new List<DateTime>();
        
        for (var i = 1; i <= days; i++)
        {
            dates.Add(new DateTime(dateTime.Year, dateTime.Month, i));
        }

        var weekDays = dates.Count(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday);
        
        return weekDays;
    }
    
    public static int WorkingDaysInMonthExcludingBankHolidays(this DateTime dateTime)
    {
        var firstDay = new DateTime(dateTime.Year, dateTime.Month, 1);
        var lastDay = new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1);

        var numberOfWorkingDaysWithoutBankHolidays = Enumerable.Range(0, (lastDay - firstDay).Days)
            .Select(a => firstDay.AddDays(a))
            .Where(a => a.DayOfWeek != DayOfWeek.Sunday)
            .Where(a => a.DayOfWeek != DayOfWeek.Saturday)
            .Count(a => BankHolidays.All(x => x.Key != a));
        
        var numberOfBankHolidaysThatAreSaturdays = Enumerable.Range(0, (lastDay - firstDay).Days)
            .Select(a => firstDay.AddDays(a))
            .Count(a => a.DayOfWeek == DayOfWeek.Saturday && BankHolidays.Any(x => x.Key == a));
        
        return numberOfWorkingDaysWithoutBankHolidays - numberOfBankHolidaysThatAreSaturdays;
    }
    
    public static double WorkingHoursInMonthExcludingBankHolidays(this DateTime dateTime)
    {
        var weekDaysInMonth = dateTime.WorkingDaysInMonthExcludingBankHolidays();

        return weekDaysInMonth * 8;
    }
}