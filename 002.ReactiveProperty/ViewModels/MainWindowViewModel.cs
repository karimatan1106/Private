using System;
using RComponents.JSON;
using RComponents.Dialog;
using Reactive.Bindings;

namespace SourceModuleCollect.ViewModels
{
    internal class MainWindowViewModel
    {
        #region プロパティ

        public ReactiveProperty<string> UpdLblFolderPath { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> BldLblFolderPath { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> SaveFolderPath { get; set; } = new ReactiveProperty<string>();

        /// <summary>
        /// 入力したフォルダパスを格納
        /// </summary>
        private string[] FolderPathHistories { get; set; } = new string[3];

        #endregion

        #region コマンド

        public ReactiveCommand InitializeMainWindowViewModelCommand { get; } = new ReactiveCommand();
        public ReactiveCommand OpenFolderDialogUpdLabelCommand { get; } = new ReactiveCommand();
        public ReactiveCommand OpenFolderDialogBldLabelCommand { get; } = new ReactiveCommand();
        public ReactiveCommand OpenFolderDialogSaveCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ExecuteCommand { get; } = new ReactiveCommand();

        #endregion

        #region コンストラクタ

        public MainWindowViewModel()
        {
            //画面ロード時
            this.InitializeMainWindowViewModelCommand.Subscribe(_ =>
            {
                var folderPathHistories = (string[])OperateJson.ParseFromJsonFolder(nameof(FolderPathHistories));

                if (folderPathHistories != null)
                {
                    //読み込んだデータをUIへ反映
                    this.UpdLblFolderPath.Value = folderPathHistories[0] ?? string.Empty;
                    this.BldLblFolderPath.Value = folderPathHistories[1] ?? string.Empty;
                    this.SaveFolderPath.Value = folderPathHistories[2] ?? string.Empty;
                    FolderPathHistories = folderPathHistories;
                }
            });

            //Updラベルフォルダ選択時
            OpenFolderDialogUpdLabelCommand.Subscribe(_ =>
            {
                this.UpdLblFolderPath.Value = OpenDialog.FolderSelect();
                FolderPathHistories[0] = UpdLblFolderPath.Value?.ToString() ?? string.Empty;
                OperateJson.SerializeWithSaveToJsonFolder(nameof(FolderPathHistories), FolderPathHistories);
            });

            ////Bldラベルフォルダ選択時
            OpenFolderDialogBldLabelCommand.Subscribe(_ =>
            {
                this.BldLblFolderPath.Value = OpenDialog.FolderSelect();
                FolderPathHistories[1] = BldLblFolderPath.Value?.ToString() ?? string.Empty;
                OperateJson.SerializeWithSaveToJsonFolder(nameof(FolderPathHistories), FolderPathHistories);
            });

            ////差分を保存するフォルダ選択時
            OpenFolderDialogSaveCommand.Subscribe(_ =>
            {
                this.SaveFolderPath.Value = OpenDialog.FolderSelect();
                FolderPathHistories[2] = SaveFolderPath.Value?.ToString() ?? string.Empty;
                OperateJson.SerializeWithSaveToJsonFolder(nameof(FolderPathHistories), FolderPathHistories);
            });

            //実行ボタン押下時
            
        }

        #endregion
    }
}
