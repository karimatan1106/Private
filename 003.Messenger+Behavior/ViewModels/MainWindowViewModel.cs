using Prism.Interactivity.InteractionRequest;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using SourceModuleCollect.Models;
using System;

namespace SourceModuleCollect.ViewModels
{
    internal class MainWindowViewModel
    {
        #region フィールド

        private readonly SaveFolderPathToJson _saveFolderPathToJson = new SaveFolderPathToJson();

        #endregion

        #region プロパティ
        public ReactiveProperty<int> Height { get; set; }
        public ReactiveProperty<int> Width { get; set; }

        public ReactiveProperty<string> UpdLblFolderPath { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> BldLblFolderPath { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> SaveFolderPath { get; set; } = new ReactiveProperty<string>();
        public InteractionRequest<OpenFolderDialogConfirmation> OpenFolderDialogRequest { get; } =
            new InteractionRequest<OpenFolderDialogConfirmation>();
        private string[] FolderPathHistories { get; set; }

        #endregion

        #region コマンド

        public ReactiveCommand InitializeMainWindowViewModelCommand { get; } = new ReactiveCommand();
        public ReactiveCommand TextChangedCommand { get; } = new ReactiveCommand();
        public ReactiveCommand OpenFolderDialogUpdLabelCommand { get; } = new ReactiveCommand();
        public ReactiveCommand OpenFolderDialogBldLabelCommand { get; } = new ReactiveCommand();
        public ReactiveCommand OpenFolderDialogSaveCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ExecuteCommand { get; } = new ReactiveCommand();

        #endregion

        #region コンストラクタ

        public MainWindowViewModel()
        {
            //フォルダ選択ダイアログをView側で表示
            void SetFolderPath(IReactiveProperty<string> folderPath)
            {
                OpenFolderDialogRequest.Raise(
                    new OpenFolderDialogConfirmation(),
                    x =>
                    {
                        if (x.Confirmed)
                        {
                            folderPath.Value = x.FolderName;
                        }
                    });
            }

            //双方向バインディングさせる
            FolderPathHistories = _saveFolderPathToJson
                                  .ToReactivePropertyAsSynchronized(x => x.FolderPathHistories)
                                  .Value;

            //画面ロード時
            InitializeMainWindowViewModelCommand.Subscribe(_ =>
            {
                if (FolderPathHistories != null)
                {
                    //読み込んだデータをUIへ反映
                    UpdLblFolderPath.Value = FolderPathHistories[0] ?? string.Empty;
                    BldLblFolderPath.Value = FolderPathHistories[1] ?? string.Empty;
                    SaveFolderPath.Value = FolderPathHistories[2] ?? string.Empty;
                    FolderPathHistories = FolderPathHistories;
                }
            });

           //テキスト変更時
            TextChangedCommand.Subscribe(_ =>
            {
                FolderPathHistories[0] = UpdLblFolderPath.Value;
                FolderPathHistories[1] = BldLblFolderPath.Value;
                FolderPathHistories[2] = SaveFolderPath.Value;
                _saveFolderPathToJson.Serialize();
            });

            //Updラベルフォルダ選択時
            OpenFolderDialogUpdLabelCommand.Subscribe(_ =>
            {
                //フォルダ選択ダイアログをView側で表示
                SetFolderPath(UpdLblFolderPath);
            });

            ////Bldラベルフォルダ選択時
            OpenFolderDialogBldLabelCommand.Subscribe(_ =>
            {
                //フォルダ選択ダイアログをView側で表示
                SetFolderPath(BldLblFolderPath);
            });

            ////差分を保存するフォルダ選択時
            OpenFolderDialogSaveCommand.Subscribe(_ =>
            {
                //フォルダ選択ダイアログをView側で表示
                SetFolderPath(SaveFolderPath);
            });
        }

        #endregion
    }
}