namespace PepperBudget.Web.Client.Services;

public class PageInfoService
{
    public string PageTitle { get; set => SetValue(ref field, value); } = "";

    public event Action? OnChanged;
    private void SetValue<TValue>(ref TValue field, TValue value)
    {
        if (!EqualityComparer<TValue>.Default.Equals(field, value))
        {
            field = value;
            NotifyChanged();
        }
    }
    private void NotifyChanged() => OnChanged?.Invoke();
}
