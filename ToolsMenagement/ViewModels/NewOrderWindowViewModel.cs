using Avalonia.Data;
using ReactiveUI;

namespace ToolsMenagement.ViewModels;

public class NewOrderWindowViewModel:ViewModelBase
{
    private bool _technologyvalid;

    public bool TechnologyValid
    {
        get => _technologyvalid;
        set
        {
            this.RaiseAndSetIfChanged(ref _technologyvalid, value);
        }
    }
    
    private string _technologyNumber;

    public string TechnologyNumber
    {
        get => _technologyNumber;
        set
        {
            int number;
            if(string.IsNullOrWhiteSpace(value))
            {
                TechnologyValid = false;
                throw new DataValidationException("");
            }
            else
            {
                if (value.Contains('.'))
                {
                    TechnologyValid = false;
                    throw new DataValidationException("");
                }
                else
                {
                    if (!int.TryParse(value, out number) || value.Contains('-') || value.Equals("0"))
                    {
                        TechnologyValid = false;
                        throw new DataValidationException("");
                    }
                    else
                    {
                        TechnologyValid = true;
                        this.RaiseAndSetIfChanged(ref _technologyNumber, value);
                    }
                }
            }
        }
    }
    public NewOrderWindowViewModel()
    {
        MyReferences.nowvm = this;
    }
}