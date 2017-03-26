using Prism.Commands;
using Prism.Mvvm;
using RComponents.JSON;
using RComponents.Dialog;

namespace SourceModuleCollect.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        #region フィールド

        private string _updLblFolderPath;
        private string _bldLblFolderPath;
        private string _saveFolderPath;

        #endregion

        #region プロパティ

        /// <summary>
        /// UIへの通知を行う
        /// </summary>
        public string UpdLblFolderPath
        {
            get { return this._updLblFolderPath; }
            set { this.SetProperty(ref this._updLblFolderPath, value); }
        }

        /// <summary>
        /// UIへの通知を行う
        /// </summary>
        public string BldLblFolderPath
        {
            get { return this._bldLblFolderPath; }
            set { this.SetProperty(ref this._bldLblFolderPath, value); }
        }

        /// <summary>
        /// UIへの通知を行う
        /// </summary>
        public string SaveFolderPath
        {
            get { return this._saveFolderPath; }
            set { this.SetProperty(ref this._saveFolderPath, value); }
        }

        /// <summary>
        /// 入力したフォルダパスを格納
        /// </summary>
        private string[] FolderPathHistories { get; set; } = new string[3];

        #endregion

        #region コマンド

        public DelegateCommand InitializeMainWindowViewModelCommand { get; }
        public DelegateCommand OpenFolderDialogUpdLabelCommand { get; }
        public DelegateCommand OpenFolderDialogBldLabelCommand { get; }
        public DelegateCommand OpenFolderDialogSaveCommand { get; }
        public DelegateCommand ExecuteCommand { get; }

        #endregion

        #region コンストラクタ

        public MainWindowViewModel()
        {
            //画面ロード時
            this.InitializeMainWindowViewModelCommand = new DelegateCommand(() =>
            {
                var folderPathHistories = (string[])OperateJson.ParseFromJsonFolder(nameof(FolderPathHistories));

                if (folderPathHistories != null)
                {
                    //読み込んだデータをUIへ反映
                    this.UpdLblFolderPath = folderPathHistories[0];
                    this.BldLblFolderPath = folderPathHistories[1];
                    this.SaveFolderPath = folderPathHistories[2];

                    FolderPathHistories = folderPathHistories;
                }
            });

            //Updラベルフォルダ選択時
            this.OpenFolderDialogUpdLabelCommand = new DelegateCommand(() =>
            {
                this.UpdLblFolderPath = OpenDialog.FolderSelect();
                FolderPathHistories[0] = this.UpdLblFolderPath ?? string.Empty;
                OperateJson.SerializeWithSaveToJsonFolder(nameof(FolderPathHistories), FolderPathHistories);
            });

            //Bldラベルフォルダ選択時
            this.OpenFolderDialogBldLabelCommand = new DelegateCommand(() =>
            {
                this.BldLblFolderPath = OpenDialog.FolderSelect();
                FolderPathHistories[1] = this.BldLblFolderPath ?? string.Empty;
                OperateJson.SerializeWithSaveToJsonFolder(nameof(FolderPathHistories), FolderPathHistories);
            });

            //差分を保存するフォルダ選択時
            this.OpenFolderDialogSaveCommand = new DelegateCommand(() =>
            {
                this.SaveFolderPath = OpenDialog.FolderSelect();
                FolderPathHistories[2] = this.SaveFolderPath ?? string.Empty;
                OperateJson.SerializeWithSaveToJsonFolder(nameof(FolderPathHistories), FolderPathHistories);
            });

            //差分を保存するフォルダ選択時
            this.ExecuteCommand = new DelegateCommand(() =>
            {

            });
        }

        #endregion
    }
}
