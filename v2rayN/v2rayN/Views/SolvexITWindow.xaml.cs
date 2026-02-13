using ServiceLib.ViewModels;
using v2rayN.Base;

namespace v2rayN.Views;

public partial class SolvexITWindow : WindowBase<SolvexITViewModel>
{
    public SolvexITWindow()
    {
        InitializeComponent();
        ViewModel = new SolvexITViewModel();
    }
}
