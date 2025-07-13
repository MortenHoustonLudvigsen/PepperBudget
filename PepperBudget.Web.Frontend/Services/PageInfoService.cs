namespace PepperBudget.Web.Frontend.Services;

public class PageInfoService
{
    public string PageTitle { get; set => SetValue(ref field, value); } = "Slambam";

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
