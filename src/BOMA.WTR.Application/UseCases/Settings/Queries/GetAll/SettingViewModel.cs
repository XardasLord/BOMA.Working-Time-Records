namespace BOMA.WTR.Application.UseCases.Settings.Queries.GetAll;

public class SettingViewModel
{
    public int Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public DateTimeOffset LastModified { get; set; }
}