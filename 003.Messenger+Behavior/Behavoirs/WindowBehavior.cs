using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace SourceModuleCollect.Behavoirs
{
    /// <summary> 
    /// ウィンドウ(Window)用ビヘイビアクラス
    /// </summary>
    public static class WindowBehavior
    {
        /// <summary>
        /// ウィンドウ状態(位置、サイズ)を記憶するかどうかを取得します。
        /// </summary>
        /// <param name="obj">対象オブジェクト(Window)</param>
        /// <returns>true:記憶する。</returns>
        public static bool GetSaveWindowState(DependencyObject obj)
        {
            return (bool)obj.GetValue(SaveWindowStateProperty);
        }

        /// <summary>
        /// ウィンドウ状態(位置、サイズ)を記憶するかどうかを設定します。
        /// </summary>
        /// <param name="obj">対象オブジェクト(Window)</param>
        /// <param name="value">true:記憶する。</param>
        public static void SetSaveWindowState(DependencyObject obj, bool value)
        {
            obj.SetValue(SaveWindowStateProperty, value);
        }

        /// <summary>ウィンドウ状態(位置、サイズ)を記憶するかどうか</summary>
        public static readonly DependencyProperty SaveWindowStateProperty =
            DependencyProperty.RegisterAttached("SaveWindowState", typeof(bool),
                                typeof(WindowBehavior),
                                new UIPropertyMetadata(false, OnSaveWindowStatePropertyChanged));

        /// <summary>
        /// SaveWindowStateプロパティ値変更イベントハンドラ
        /// </summary>
        /// <param name="dpObj">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private static void OnSaveWindowStatePropertyChanged(DependencyObject dpObj, DependencyPropertyChangedEventArgs e)
        {
            var window = dpObj as Window;

            if (window == null)
            {
                return;
            }

            if ((bool)e.NewValue)
            {
                if (!window.IsLoaded)
                {
                    window.Loaded += SaveStateWindow_Loaded;
                }
                else
                {
                    //Load済
                    SaveStateWindow_Loaded(window, new RoutedEventArgs());
                }
            }
        }

        /// <summary>
        /// Loadedイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private static void SaveStateWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //ロード時にファイルを検索・設定読み込み
            //終了時にファイルへ設定を書き込み
            var window = sender as Window;

            //イベントハンドラを削除します。
            window.Loaded -= SaveStateWindow_Loaded;

            //終了時イベントにハンドラを登録します。
            window.Closed += SaveStateWindow_Closed;

            var settingPath = window.ResolveSetringPath();

            if (File.Exists(settingPath))
            {
                //ロードします。
                var setting = File.ReadAllText(settingPath);

                if (string.IsNullOrWhiteSpace(setting)) return;

                //カンマ区切りで分割
                var settingList = setting.Split(',');

                try
                {
                    window.Left = double.Parse(settingList[0]);
                    window.Top = double.Parse(settingList[1]);
                    window.Width = double.Parse(settingList[2]);
                    window.Height = double.Parse(settingList[3]);

                    var state = (WindowState)Enum.Parse(typeof(WindowState), settingList[4]);

                    if (state == WindowState.Maximized)
                    {
                        //最大化の時のみ反映させよう。
                        window.WindowState = WindowState.Maximized;
                    }

                }
                catch
                {
                    //反映失敗(でも黙殺)
                }
            }
        }

        /// <summary>
        /// ウィンドウ終了イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private static void SaveStateWindow_Closed(object sender, EventArgs e)
        {
            var window = sender as Window;

            //イベントハンドラを削除します。
            window.Closed -= SaveStateWindow_Closed;

            //保存先
            var settingPath = window.ResolveSetringPath();
            const string FORMAT = "#0";
            var minWidth = 30D;//デフォルト値
            var minHeight = 30D;//デフォルト値

            if (!double.IsNaN(window.MinWidth))
            {
                minWidth = window.MinWidth;
            }

            if (!double.IsNaN(window.MinHeight))
            {
                minHeight = window.MinHeight;
            }

            var settingList = new[]{
                Math.Max(0D, window.Left).ToString(FORMAT),//0:左座標
                Math.Max(0D, window.Top).ToString(FORMAT),//1:上座標
                Math.Max(minWidth, window.Width).ToString(FORMAT),//2:幅
                Math.Max(minHeight, window.Height).ToString(FORMAT),//3:高さ
                window.WindowState.ToString()//4:状態
            };

            //保存します。(カンマ区切りで)
            var fileInfo = new FileInfo(settingPath);

            //ディレクトリなければ作る。
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }

            File.WriteAllText(settingPath, settingList.Aggregate((x, next) => x + "," + next));
        }

        /// <summary>
        /// 指定のウィンドウに関する情報を保存するための設定ファイルパスを取得します。
        /// thisをつけて、拡張メソッド風に使えるようにしています。
        /// </summary>
        /// <param name="window">ウィンドウ</param>
        /// <returns>設定ファイルパス</returns>
        private static string ResolveSetringPath(this Window window)
        {
            //アプリケーション用フォルダ
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            //エントリポイントを持つアセンブリ(大抵*.exe)の名前
            var appName = System.Reflection.Assembly.GetEntryAssembly().GetName();

            //ファイル区切り文字
            var separator = Path.DirectorySeparatorChar.ToString();

            return string.Concat(
                folder,
                separator,
                appName.Name,
                separator,
                window.GetType().Name,
                ".config"
                );
        }
    }
}