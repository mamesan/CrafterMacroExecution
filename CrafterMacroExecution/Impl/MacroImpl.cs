using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrafterMacroExecution.Events
{
    class MacroImpl
    {

        /// <summary>
        /// マクロ情報読み込み
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Macro一覧_click(CrafterMacroExecution formInfo, string ListBoxText)
        {
            // キャストを行う
            ListBox listBox = (ListBox)sender;

            // 空だった場合、返却
            if (String.IsNullOrWhiteSpace(listBox.Text))
            {
                return;
            }

            try
            {
                // 対象マクロ情報を取得する
                IMacroInfoBean macroInfoBean = FileController.ReadMacroInfo(listBox.Text);

                // マクロ名
                this.textBox8.Text = listBox.Text;
                // 必要工数
                this.textBox1.Text = macroInfoBean.NecessaryManHours;
                // 星数
                this.comboBox1.Text = macroInfoBean.StarCount;
                // 必要品質
                this.textBox2.Text = macroInfoBean.CraftControlCount;
                // 作るもの
                this.comboBox3.Text = macroInfoBean.WhatMakes;

                // リストを一度削除する
                this.listBox1.Items.Clear();

                // マクロ情報を取得する
                List<IPlayMacroInfoBean> list = FileController.GetTempMacroInfo(FILE_PATH_TEMPMACRO + listBox.Text + ".xml");

                // 指定されたマクロリストの内容を、チェックリストに格納する
                foreach (IPlayMacroInfoBean playMacroInfoBean in list)
                {
                    // スキル名のみトリムした名前を、リストに格納する
                    string Skill = playMacroInfoBean.Text.Substring(4);
                    this.listBox1.Items.Add(Skill);
                }
            }
            catch
            {
                // メッセージボックスを生成する
                DialogResult result = MessageBox.Show("指定されたマクロ情報が存在しませんでした。\r\n情報のみ新しく作成しますか？", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                //何が選択されたか調べる
                if (result == DialogResult.Yes)
                {

                    // マクロ名
                    this.textBox8.Text = listBox.Text;
                    // 必要工数
                    this.textBox1.Text = "0";
                    // 星数
                    this.comboBox1.Text = "0";
                    // 必要品質
                    this.textBox2.Text = "0";
                    // 作るもの
                    this.comboBox3.Text = "HQ";

                    // リストを一時作成する
                    List<string[]> dclist = new List<string[]>();
                    dclist.Add(new string[] { this.textBox1.Text, this.comboBox1.Text, this.textBox2.Text, this.comboBox3.Text });

                    // マクロ情報を保存する
                    FileController.SaveInfo(new List<string> { this.textBox8.Text }, Utils.Utils.CreateDictionary(dclist, SAVE_MACRO_INFO), FILE_PATH_MACROINFO);
                    // 作成終了アラート
                    MessageBox.Show("新しくマクロ情報を作成しました。\r\n「" + this.textBox8.Text + "」");

                    // リストを一度削除する
                    this.listBox1.Items.Clear();

                    // マクロ情報を取得する
                    List<IPlayMacroInfoBean> list = FileController.GetTempMacroInfo(FILE_PATH_TEMPMACRO + listBox.Text + ".xml");

                    // 指定されたマクロリストの内容を、チェックリストに格納する
                    foreach (IPlayMacroInfoBean playMacroInfoBean in list)
                    {
                        // スキル名のみトリムした名前を、リストに格納する
                        string Skill = playMacroInfoBean.Text.Substring(4);
                        this.listBox1.Items.Add(Skill);
                    }

                }
                else
                {
                    for (int i = 0; i < this.Macro一覧.Items.Count; i++)
                    {
                        if (this.Macro一覧.Items[i].ToString() == listBox.Text)
                        {
                            this.Macro一覧.Items.Remove(listBox.Text);
                        }
                    }

                    MessageBox.Show("不要なリストを削除しました。", "ふれってぃー");
                }
            }

        }

    }
}
