using ServiceLib.ViewModels;
using v2rayN.Desktop.Base;

namespace v2rayN.Desktop.Views;

public partial class SolvexITWindow : WindowBase<SolvexITViewModel>
{
    public SolvexITWindow()
    {
        InitializeComponent();
        ViewModel = new SolvexITViewModel();
    }
}
