namespace Domain.AppSettings.Interfaces;

public interface IFilterSetting<TFilter>
{
    IReadOnlyList<TFilter> Filters { get; }
}