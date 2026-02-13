using System.Reactive.Disposables;
using ReactiveUI;
using ServiceLib.ViewModels;
using v2rayN.Base;

namespace v2rayN.Views;

public partial class SolvexITWindow : WindowBase<SolvexITViewModel>
{
    public SolvexITWindow()
    {
        InitializeComponent();
        ViewModel = new SolvexITViewModel();

        this.WhenActivated(disposables =>
        {
            this.OneWayBind(ViewModel, vm => vm.ServerItems, v => v.lstServers.ItemsSource).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.SelectedItem, v => v.lstServers.SelectedItem).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.EditRemarks, v => v.txtRemarks.Text).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.EditDomainPort, v => v.txtDomainPort.Text).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.EditVrtexID, v => v.txtVrtexID.Text).DisposeWith(disposables);
            this.OneWayBind(ViewModel, vm => vm.AddBtnText, v => v.btnAdd.Content).DisposeWith(disposables);

            this.BindCommand(ViewModel, vm => vm.AddCmd, v => v.btnAdd).DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.DeleteCmd, v => v.btnDelete).DisposeWith(disposables);
        });
    }
}
