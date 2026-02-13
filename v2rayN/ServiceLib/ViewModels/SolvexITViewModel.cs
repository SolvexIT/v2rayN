using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ServiceLib.Handler;
using ServiceLib.Models;

namespace ServiceLib.ViewModels;

public class SolvexITViewModel : MyReactiveObject
{
    public IObservableCollection<SolvexITServerItem> ServerItems { get; } = new ObservableCollectionExtended<SolvexITServerItem>();

    [Reactive] public SolvexITServerItem SelectedItem { get; set; }
    [Reactive] public string EditRemarks { get; set; } = string.Empty;
    [Reactive] public string EditDomainPort { get; set; } = string.Empty;
    [Reactive] public string EditVrtexID { get; set; } = string.Empty;
    [Reactive] public string AddBtnText { get; set; } = string.Empty;

    public ReactiveCommand<Unit, Unit> AddCmd { get; }
    public ReactiveCommand<Unit, Unit> DeleteCmd { get; }

    public SolvexITViewModel()
    {
        AddBtnText = ResUI.menuSubAdd;

        var canAdd = this.WhenAnyValue(
            x => x.EditDomainPort,
            x => x.EditVrtexID,
            (dp, vid) => !string.IsNullOrWhiteSpace(dp) && dp.Contains(':') && !string.IsNullOrWhiteSpace(vid) && vid.Length == 12);

        AddCmd = ReactiveCommand.CreateFromTask(async () =>
        {
            var parts = EditDomainPort.Split(':');
            if (parts.Length != 2 || !int.TryParse(parts[1], out int port)) return;

            if (SelectedItem != null)
            {
                // Update existing
                SelectedItem.Remarks = EditRemarks;
                SelectedItem.Domain = parts[0];
                SelectedItem.Port = port;
                SelectedItem.VrtexID = EditVrtexID;
                
                // Trigger refresh in UI
                var index = ServerItems.IndexOf(SelectedItem);
                ServerItems.RemoveAt(index);
                ServerItems.Insert(index, SelectedItem);
            }
            else
            {
                // Add new
                var newItem = new SolvexITServerItem
                {
                    Id = Utils.GetGuid(false),
                    Remarks = EditRemarks,
                    Domain = parts[0],
                    Port = port,
                    VrtexID = EditVrtexID,
                    Sort = DateTime.Now.Ticks
                };
                ServerItems.Add(newItem);
            }
            
            await SaveAll();
            
            if (SelectedItem == null)
            {
                EditRemarks = string.Empty;
                EditDomainPort = string.Empty;
                EditVrtexID = string.Empty;
            }
        }, canAdd);

        DeleteCmd = ReactiveCommand.CreateFromTask(async () =>
        {
            if (SelectedItem != null)
            {
                ServerItems.Remove(SelectedItem);
                SelectedItem = null;
                await SaveAll();
            }
        }, this.WhenAnyValue(x => x.SelectedItem).Select(x => x != null));

        // Selection changed logic
        this.WhenAnyValue(x => x.SelectedItem)
            .Subscribe(item =>
            {
                if (item != null)
                {
                    EditRemarks = item.Remarks;
                    EditDomainPort = $"{item.Domain}:{item.Port}";
                    EditVrtexID = item.VrtexID;
                    AddBtnText = ResUI.TbConfirm; // Or a "Save" resource if available
                }
                else
                {
                    AddBtnText = ResUI.menuSubAdd;
                }
            });

        // Auto-save draft logic
        this.WhenAnyValue(x => x.EditRemarks, x => x.EditDomainPort, x => x.EditVrtexID)
            .Throttle(TimeSpan.FromSeconds(1))
            .Subscribe(async _ => { await SaveAll(); });

        LoadData();
    }

    private async void LoadData()
    {
        var config = await SolvexITHandler.LoadConfig();
        ServerItems.Clear();
        ServerItems.AddRange(config.Servers.OrderBy(x => x.Sort));
        
        EditRemarks = config.DraftRemarks;
        EditDomainPort = config.DraftDomainPort;
        EditVrtexID = config.DraftVrtexID;
    }

    private async Task SaveAll()
    {
        var config = new SolvexITConfig
        {
            Servers = ServerItems.ToList(),
            DraftRemarks = EditRemarks,
            DraftDomainPort = EditDomainPort,
            DraftVrtexID = EditVrtexID
        };
        await SolvexITHandler.SaveConfig(config);
    }
}
