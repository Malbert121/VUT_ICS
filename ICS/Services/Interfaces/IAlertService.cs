namespace ICS.Services;

public interface IAlertService
{
    Task DisplayAsync(string title, string message);
}
