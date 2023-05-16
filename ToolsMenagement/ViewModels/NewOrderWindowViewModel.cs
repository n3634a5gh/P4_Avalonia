using Avalonia.Data;
using ReactiveUI;

namespace ToolsMenagement.ViewModels;

public class NewOrderWindowViewModel:ViewModelBase
{
    private string _technologyNumber;

    public string TechnologyNumber
    {
        get => _technologyNumber;
        set
        {
            int number;
            if(string.IsNullOrWhiteSpace(value))
            {
                throw new DataValidationException("Pole nie może być puste");
            }
            else
            {
                if (value.Contains('.'))
                {
                    throw new DataValidationException("Wartość musi być dodatnia typu int");
                }
                else
                {
                    if (!int.TryParse(value, out number) || value.Contains('-') || value.Equals("0"))
                    {
                        throw new DataValidationException("Wartość musi być dodatnia typu int");
                    }
                    else
                    {
                        this.RaiseAndSetIfChanged(ref _technologyNumber, value);
                    }
                }
            }
        }
    }
}