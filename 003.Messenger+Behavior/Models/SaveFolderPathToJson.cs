using Prism.Mvvm;
using RComponents.JSON;

namespace SourceModuleCollect.Models
{
    internal class SaveFolderPathToJson : BindableBase
    {
        #region プロパティ

        internal string[] FolderPathHistories { get; set; } = new string[3];

        #endregion

        #region コンストラクタ

        public SaveFolderPathToJson()
        {
            Parse();
        }

        #endregion

        #region メソッド

        /// <summary>
        /// フォルダパスをJson形式で保存
        /// </summary>
        internal void Serialize()
        {
            OperateJson.SerializeWithSaveToJsonFolder(nameof(FolderPathHistories), FolderPathHistories);
        }

        /// <summary>
        /// 保存しているJsonファイルのパースを行う
        /// </summary>
        private void Parse()
        {
            var parseData = OperateJson.ParseFromJsonFolder(nameof(FolderPathHistories));
            if (parseData == null) return;
            FolderPathHistories = (string[])parseData;
        }

        #endregion
    }
}