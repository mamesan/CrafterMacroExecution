using Advanced_Combat_Tracker;
using CrafterMacroExecution.Bean;
using CrafterMacroExecution.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CrafterMacroExecution.Utils.Const;
using static CrafterMacroExecution.Utils.MessageProperty;

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
            try
            {
                // 対象マクロ情報を取得する
                IMacroInfoBean macroInfoBean = FileController.ReadMacroInfo(ListBoxText);

                // マクロ名
                formInfo.マクロ名_textBox.Text = ListBoxText;
                // 作るもの
                formInfo.作るもの_comboBox.Text = macroInfoBean.WhatMakes;

                // リストを一度削除する
                formInfo.選択中マクロ_listBox.Items.Clear();

                // マクロ情報を取得する
                List<IPlayMacroInfoBean> list = FileController.GetTempMacroInfo(FILE_PATH_TEMPMACRO + ListBoxText + ".xml");

                // 指定されたマクロリストの内容を、チェックリストに格納する
                foreach (IPlayMacroInfoBean playMacroInfoBean in list)
                {
                    string スキル名 = playMacroInfoBean.Text.Substring(4);
                    // スキル名のみトリムした名前を、リストに格納する
                    formInfo.選択中マクロ_listBox.Items.Add(スキル名);
                    formInfo.マクロ編集_listBox.AppendText(スキル名 + "\r\n");
                }
            }
            catch
            {
                // メッセージボックスを生成する
                DialogResult result = MessageBox.Show(READ_MACRO_INFO1 + "\r\n" + READ_MACRO_INFO2, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                //何が選択されたか調べる
                if (result == DialogResult.Yes)
                {
                    // マクロ名
                    formInfo.マクロ名_textBox.Text = ListBoxText;

                    // リストを一時作成する
                    List<string[]> dclist = new List<string[]>();
                    dclist.Add(new string[] { ListBoxText, "HQ" });

                    // マクロ情報を保存する
                    FileController.SaveInfo(new List<string> { ListBoxText }, Utils.Utils.CreateDictionary(dclist, SAVE_MACRO_INFO), FILE_PATH_MACROINFO);
                    MessageBox.Show(READ_MACRO_NEW_INFO + "\r\n「" + ListBoxText + "」");

                    // リストを一度削除する
                    formInfo.選択中マクロ_listBox.Items.Clear();

                    // マクロ情報を取得する
                    List<IPlayMacroInfoBean> list = FileController.GetTempMacroInfo(FILE_PATH_TEMPMACRO + ListBoxText + ".xml");

                    // 指定されたマクロリストの内容を、チェックリストに格納する
                    foreach (IPlayMacroInfoBean playMacroInfoBean in list)
                    {
                        string スキル名 = playMacroInfoBean.Text.Substring(4);
                        // スキル名のみトリムした名前を、リストに格納する
                        formInfo.選択中マクロ_listBox.Items.Add(スキル名);
                        formInfo.マクロ編集_listBox.AppendText(スキル名 + "\r\n");
                    }
                }
                else
                {
                    for (int i = 0; i < formInfo.Macro一覧.Items.Count; i++)
                    {
                        if (formInfo.Macro一覧.Items[i].ToString() == ListBoxText) formInfo.Macro一覧.Items.Remove(ListBoxText);
                    }
                    MessageBox.Show(READ_MACRO_DELETE_INFO, "ふれってぃー");
                }
            }
        }

        /// <summary>
        /// マクロを作成する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void マクロ追加ボタン_click(CrafterMacroExecution formInfo)
        {
            string マクロ名 = formInfo.マクロ名_textBox.Text;

            // 空だった場合、返却
            if (String.IsNullOrWhiteSpace(マクロ名)
                || String.IsNullOrWhiteSpace(formInfo.マクロ編集_listBox.Text)
                || String.IsNullOrWhiteSpace(formInfo.作るもの_comboBox.Text))
            {
                MessageBox.Show(CREATE_MACRO_INFO_ERROR);
                return;
            }

            // マクロ名重複チェック(簡易)
            for (int ii = 0; ii < formInfo.Macro一覧.Items.Count; ii++)
            {
                if (formInfo.Macro一覧.Items[ii].ToString().Equals(マクロ名))
                {
                    MessageBox.Show(CREATE_MACRO_INFO_NAME_ERROR);
                    return;
                }
            }

            // リストを一時作成する
            List<string[]> dclist = new List<string[]>();
            dclist.Add(new string[] { マクロ名, formInfo.作るもの_comboBox.Text });

            // マクロ情報を保存する
            FileController.SaveInfo(new List<string> { マクロ名 }, Utils.Utils.CreateDictionary(dclist, SAVE_MACRO_INFO), FILE_PATH_MACROINFO);

            // リストボックスに、情報を書き込む
            formInfo.Macro一覧.Items.Add(マクロ名);

            // スキル一覧を取得する
            List<ISkilInfoBean> skilInfoBean = FileController.GetSkilInfo(FILE_PATH_SKILLINFO);

            // リストを一時作成する
            dclist = new List<string[]>();
            List<string> keylist = new List<string>();
            int i = 1;
            string wait = null;
            string[] del = { "\r\n" };
            string[] MacroList = formInfo.マクロ編集_listBox.Text.Split(del, StringSplitOptions.None);

            // リストの中身分回す
            foreach (string str in formInfo.Macro一覧.Items)
            {
                keylist.Add("/ac " + str);
                // スキル一覧より、判定を行う
                foreach (ISkilInfoBean tmpBean in skilInfoBean)
                {

                    // スキル名が一致した場合、待機時間を判定する
                    if (tmpBean.SkillName.Equals(str))
                    {
                        if (tmpBean.SkillTypeV.Equals("A"))
                        {
                            wait = "3";
                            break;
                        }
                        else if (tmpBean.SkillTypeV.Equals("B"))
                        {
                            wait = "2";
                            break;
                        }
                        else if (tmpBean.SkillTypeV.Equals("C"))
                        {
                            wait = "3";
                            break;
                        }
                    }
                    // 初手仕込の場合、待機時間を3秒に指定する
                    if (tmpBean.SkillName.Equals("初手仕込") || tmpBean.SkillName.Equals("経過観察"))
                    {
                        wait = "3";
                        break;
                    }
                }
                dclist.Add(new string[] { i.ToString(), wait });
                i++;
            }

            // ファイルを保存する
            FileController.SaveInfo(keylist, Utils.Utils.CreateDictionary(dclist, SAVE_MACRO_DETAIL_INFO), FILE_PATH_TEMPMACRO + マクロ名 + ".xml");

            // リストに新規作成したマクロ名を挿入する
            formInfo.実行マクロ名_comboBox.Items.Add(マクロ名 + ".xml");

            MessageBox.Show(CREATE_MACRO_INFO_SUCCESS, "例のあの人");
        }

        /// <summary>
        /// マクロ編集を行う
        /// </summary>
        /// <param name="formInfo"></param>
        public static void マクロ編集ボタン_click(CrafterMacroExecution formInfo)
        {
            string マクロ名 = formInfo.Macro一覧.Text;

            // 空だった場合、返却
            if (string.IsNullOrWhiteSpace(マクロ名)
                || string.IsNullOrWhiteSpace(formInfo.作るもの_comboBox.Text))
            {
                MessageBox.Show(CREATE_MACRO_INFO_ERROR);
                return;
            }


            // 対象マクロ情報を修正する
            FileController.EditMacroInfo(マクロ名, formInfo.作るもの_comboBox.Text);

            // スキル一覧を取得する
            List<ISkilInfoBean> skilInfoBean = FileController.GetSkilInfo(FILE_PATH_SKILLINFO);

            // リストを一時作成する
            List<string[]> dclist = new List<string[]>();
            List<string> keylist = new List<string>();
            int i = 1;
            string wait = null;

            // リストの中身分回す
            foreach (string str in formInfo.選択中マクロ_listBox.Items)
            {
                keylist.Add("/ac " + str);
                // スキル一覧より、判定を行う
                foreach (ISkilInfoBean tmpBean in skilInfoBean)
                {

                    // スキル名が一致した場合、待機時間を判定する
                    if (tmpBean.SkillName.Equals(str))
                    {
                        if (tmpBean.SkillTypeV.Equals("A"))
                        {
                            wait = "3";
                            break;
                        }
                        else if (tmpBean.SkillTypeV.Equals("B"))
                        {
                            wait = "2";
                            break;
                        }
                        else if (tmpBean.SkillTypeV.Equals("C"))
                        {
                            wait = "3";
                            break;
                        }
                    }
                    // 初手仕込、経過観察の場合、待機時間を3秒に指定する
                    if (tmpBean.SkillName.Equals("初手仕込") || tmpBean.SkillName.Equals("経過観察"))
                    {
                        wait = "3";
                        break;
                    }
                }
                dclist.Add(new string[] { i.ToString(), wait });
                i++;
            }

            // ファイルを削除する
            File.Delete(@ActGlobals.oFormActMain.AppDataFolder.FullName + "\\" + FILE_PATH_TEMPMACRO + "\\" + マクロ名 + ".xml");
            // ファイルを保存する
            FileController.SaveInfo(keylist, Utils.Utils.CreateDictionary(dclist, SAVE_MACRO_DETAIL_INFO), FILE_PATH_TEMPMACRO + マクロ名 + ".xml");

            MessageBox.Show(EDIT_MACRO_INFO_SUCCESS, "更新完了");


        }

        /// <summary>
        /// マクロを削除する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void マクロ削除ボタン_click(CrafterMacroExecution formInfo, string ListBoxText)
        {
            // メッセージボックスを生成する
            DialogResult result = MessageBox.Show(DELETE_MACRO_INFO + "\r\n「" + ListBoxText + "」", "質問", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            //何が選択されたか調べる
            if (result == DialogResult.Yes)
            {
                // 対象マクロ情報を削除する
                FileController.RemoveXMLInfo(ListBoxText, FILE_PATH_MACROINFO);
                // ファイルを削除する
                File.Delete(ActGlobals.oFormActMain.AppDataFolder.FullName + "\\" + FILE_PATH_TEMPMACRO + ListBoxText + ".xml");

                // 削除対象の子を一覧から消す
                formInfo.実行マクロ名_comboBox.Items.Remove(ListBoxText + ".xml");

                formInfo.マクロ名_textBox.Text = "";
                formInfo.作るもの_comboBox.SelectedIndex = -1;
                formInfo.マクロ編集_listBox.Text = "";
                formInfo.選択中マクロ_listBox.Items.Clear();
            }
        }
    }
}
