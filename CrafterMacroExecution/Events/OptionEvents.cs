using System.Windows.Forms;
using CrafterMacroExecution.Model;

namespace CrafterMacroExecution.Events
{
    static class OptionEvents
    {

        /// <summary>
        /// オプションイベントの初期処理
        /// </summary>
        /// <param name="tex1"></param>
        /// <param name="but1"></param>
        /// <param name="tex2"></param>
        /// <param name="tex3"></param>
        /// <param name="flg"></param>
        public static void setOptionActivity(TextBox tex1, Button but1, TextBox tex2, TextBox tex3, bool flg)
        {
            if (!flg)
            {
                tex1.Enabled = false;
                but1.Enabled = true;
                tex2.Enabled = true;
                tex3.Enabled = true;
            }
            else
            {
                tex1.Enabled = true;
                but1.Enabled = false;
                tex2.Enabled = false;
                tex3.Enabled = false;
            }
        }

        /// <summary>
        /// オプションイベントの初期処理（修理専用）
        /// </summary>
        /// <param name="tex1"></param>
        /// <param name="but1"></param>
        /// <param name="tex2"></param>
        /// <param name="tex3"></param>
        /// <param name="flg"></param>
        public static void setOptionActivity(TextBox tex1,
            Button but1, TextBox tex2, TextBox tex3,
            Button but2, TextBox tex4, TextBox tex5,
            Button but3, TextBox tex6, TextBox tex7, bool flg)
        {
            if (!flg)
            {
                tex1.Enabled = false;
                but1.Enabled = true;
                but2.Enabled = true;
                but3.Enabled = true;
                tex2.Enabled = true;
                tex3.Enabled = true;
                tex4.Enabled = true;
                tex5.Enabled = true;
                tex6.Enabled = true;
                tex7.Enabled = true;
            }
            else
            {
                tex1.Enabled = true;
                but1.Enabled = false;
                but2.Enabled = false;
                but3.Enabled = false;
                tex2.Enabled = false;
                tex3.Enabled = false;
                tex4.Enabled = false;
                tex5.Enabled = false;
                tex6.Enabled = false;
                tex7.Enabled = false;
            }
        }

        /// <summary>
        /// 座標ボタンをクリックした際に発生するイベント
        /// </summary>
        /// <param name="tex1"></param>
        /// <param name="tex2"></param>
        public static void OptionEventCoordinateClick(TextBox tex1, TextBox tex2)
        {
            Coordinate f = new Coordinate();
            f.ShowDialog();
            // 座標を埋め込む
            tex1.Text = f.mousePoint.X.ToString();
            tex2.Text = f.mousePoint.Y.ToString();

            f.Close();
        }

    }
}
