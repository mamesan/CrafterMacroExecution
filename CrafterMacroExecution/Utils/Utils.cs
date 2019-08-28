using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using static CrafterMacroExecution.Utils.Const;

namespace CrafterMacroExecution.Utils
{
    using Advanced_Combat_Tracker;
    using global::CrafterMacroExecution.Data;
    using System.Drawing;

    /// <summary>
    /// 共通処理クラス
    /// </summary>
    public static class Utils
    {

        /// <summary>
        /// マップを生成し、返却するメソッド
        /// </summary>
        /// <param name="args">パラメーター</param>
        /// <param name="flg">1  : キャラクタ情報<br>
        ///                   2  : マクロ情報<br>
        ///                   3  : マクロ詳細情報</param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> CreateDictionary(List<string[]> list, int flg)
        {
            // 返却用リストを生成する
            List<Dictionary<string, string>> dcList = new List<Dictionary<string, string>>();

            // マップのインスタンスを生成する
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            // マップ作製情報を取得する
            string[] info = GetInfo(flg);

            // カウンターを定義する
            int i = 0;

            // リスト分回す
            foreach (string[] args in list)
            {
                i = 0;
                dictionary = new Dictionary<string, string>();
                // 取得した情報を元に、マップを作製する
                foreach (string str in info)
                {
                    dictionary.Add(str, args[i]);
                    i++;
                }
                dcList.Add(dictionary);
            }

            return dcList;
        }


        /// <summary>
        /// イメージを作成する
        /// </summary>
        /// <param name="image"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public static Image createThumbnail(Image image, int w, int h)
        {
            Bitmap canvas = new Bitmap(w, h);

            Graphics g = Graphics.FromImage(canvas);
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, w, h);

            float fw = (float)w / (float)image.Width;
            float fh = (float)h / (float)image.Height;

            float scale = Math.Min(fw, fh);
            fw = image.Width * scale;
            fh = image.Height * scale;

            g.DrawImage(image, (w - fw) / 2, (h - fh) / 2, fw, fh);
            g.Dispose();

            return canvas;
        }

        /// <summary>
        /// 作成情報リストを生成するクラス
        /// </summary>
        /// <param name="flg"></param>
        /// <returns></returns>
        private static string[] GetInfo(int flg)
        {
            if (flg == 1)
            {
                return CHARA_INFO;
            }
            else if (flg == 2)
            {
                return MACRO_INFO;
            }
            else if (flg == 3)
            {
                return MACRO_DETAIL_INFO;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// FF14プロセスをアクティベート化する
        /// </summary>
        /// <returns></returns>
        public static Boolean ActivateFF14()
        {
            // 実行中のすべてのプロセスを取得する
            System.Diagnostics.Process[] hProcesses = System.Diagnostics.Process.GetProcesses();

            string stPrompt = string.Empty;

            // 取得できたプロセスからプロセス名を取得する
            foreach (System.Diagnostics.Process hProcess in hProcesses)
            {
                //"FF14"がメインウィンドウのタイトルに含まれているか調べる
                if (hProcess.MainWindowTitle == "FINAL FANTASY XIV")
                {
                    // アクティブ化
                    SetForegroundWindow(hProcess.MainWindowHandle);
                    // Microsoft.VisualBasic.Interaction.AppActivate(ps[0].Id);
                    return true;
                }


            }
            return false;
        }

        /// <summary>
        /// 引数で渡されたパスから、フォルダの内容を取得する
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> FindFile(string path, string Extension)
        {
            // パスを設定
            var x = new DirectoryInfo(Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, path));

            var p = x.GetFiles(Extension).Select(fileinfo => fileinfo.Name);

            // 返却用リストを生成する
            List<string> list = new List<string>();

            // 取得した名称を格納する
            list.AddRange(p.ToArray<string>());

            return list;
        }

        /// <summary>
        /// マウスイベントを発生させる
        /// </summary>
        /// <param name="MouseEvent"></param>
        /// <param name="point"></param>
        public static void mouse_Click(string X, string Y)
        {
            SetCursorPos(int.Parse(X), int.Parse(Y));
            Thread.Sleep(50);
            mouse_event(KeyCodeList.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);  // マウスの左ボタンダウンイベントを発生させる
            Thread.Sleep(50);
            mouse_event(KeyCodeList.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);    // マウスの左ボタンアップイベントを発生させる
        }

        /// <summary>
        /// キー操作をシュミレートする
        /// </summary>
        /// <param name="key"></param>
        public static void KeySim(byte key)
        {
            // キーの押し下げをシミュレートする。
            keybd_event(key, 0, 0, (UIntPtr)0);
            Thread.Sleep(10);
            keybd_event(key, 0, 2, (UIntPtr)0);

        }

        /// <summary>
        /// 数値チェック
        /// </summary>
        /// <param name="targetStr"></param>
        /// <returns></returns>
        public static Boolean checkNumber(string targetStr)
        {
            if (String.IsNullOrEmpty(targetStr)) return true;
            try { int.Parse(targetStr); }
            catch { return false; }
            return true;
        }

        /// <summary>
        /// キー操作用拡張
        /// </summary>
        /// <param name="bVk"></param>
        /// <param name="bScan"></param>
        /// <param name="dwFlags"></param>
        /// <param name="dwExtraInfo"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern uint keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        /// <summary>
        /// メッセージボックス用
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// マウスポイントを移動させるイベント
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void SetCursorPos(int X, int Y);

        /// <summary>
        /// クリックを発生させるイベント
        /// </summary>
        /// <param name="dwFlags"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="cButtons"></param>
        /// <param name="dwExtraInfo"></param>
        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
    }
}