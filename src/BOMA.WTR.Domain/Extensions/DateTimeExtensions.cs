namespace BOMA.WTR.Domain.Extensions;

public static class DateTimeExtensions
{
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
    
    public static double WorkingHoursInMonth(this DateTime dateTime)
    {
        // TODO: Need to count only working week days without holidays (e.g. 06.2022 should have 168 hours instead of 176 hours)
        var weekDaysInMonth = dateTime.WeekDaysInMonth();

        return weekDaysInMonth * 8;
    }
}