using System.Windows;
using System.Windows.Interactivity;
using Prism.Interactivity.InteractionRequest;
using RComponents.Dialog;
using SourceModuleCollect.Models;

namespace SourceModuleCollect.Views
{
    internal class ShowFolderDialogAction : TriggerAction<DependencyObject>
    {
        protected override void Invoke(object parameter)
        {
            var e = parameter as InteractionRequestedEventArgs;

            var n = e.Context as OpenFolderDialogConfirmation;
            n.FolderName = OpenDialog.FolderSelect();
            n.Confirmed = !string.IsNullOrEmpty(n.FolderName);
            e.Callback();
        }
    }
}