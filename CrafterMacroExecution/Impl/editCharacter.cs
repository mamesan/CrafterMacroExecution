using CrafterMacroExecution.Bean;
using CrafterMacroExecution.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CrafterMacroExecution.Utils.MessageProperty;
using static CrafterMacroExecution.Utils.Const;

/// <summary>
/// キャラクター情報を生成するクラス
/// </summary>
namespace CrafterMacroExecution.Impl
{
    public static class EditCharacter
    {
        /// <summary>
        /// キャラクター情報を作成する
        /// </summary>
        /// <param name="forminfo"></param>
        public static void createCharacterInfo(CrafterMacroExecution forminfo)
        {
            string キャラ名 = forminfo.キャラクターのお名前.Text;

            // 必須項目が空だった場合、返却
            if (string.IsNullOrWhiteSpace(キャラ名)
                // 必須かどうかはいったん考える
                // || String.IsNullOrWhiteSpace(this.textBox4.Text)
                // || String.IsNullOrWhiteSpace(this.textBox5.Text)
                // || String.IsNullOrWhiteSpace(this.textBox3.Text)
                // || String.IsNullOrWhiteSpace(this.木工師_textBox.Text)
                // || String.IsNullOrWhiteSpace(this.革細工師_textBox.Text)
                // || String.IsNullOrWhiteSpace(this.鍛冶師_textBox.Text)
                // || String.IsNullOrWhiteSpace(this.裁縫師_textBox.Text)
                // || String.IsNullOrWhiteSpace(this.甲冑師_textBox.Text)
                // || String.IsNullOrWhiteSpace(this.錬金術師_textBox.Text)
                // || String.IsNullOrWhiteSpace(this.彫金師_textBox.Text)
                // || String.IsNullOrWhiteSpace(this.調理師_textBox.Text)
                // || String.IsNullOrWhiteSpace(this.チョコボ師_textBox.Text)
                // || String.IsNullOrWhiteSpace(this.例のあの人_textBox.Text)
                // || String.IsNullOrWhiteSpace(this.マイスター1_comboBox.Text)
                // || String.IsNullOrWhiteSpace(this.マイスター2_comboBox.Text)
                // || String.IsNullOrWhiteSpace(this.マイスター3_comboBox.Text)
                )
            {
                MessageBox.Show(CHARACTER_EDIT_REQUIRED_ERROR);
                return;
            }
            // キャラ名重複チェック(簡易)
            for (int ii = 0; ii < forminfo.キャラ一覧.Items.Count; ii++)
            {
                if (forminfo.キャラ一覧.Items[ii].ToString().Equals(forminfo.キャラクターのお名前.Text))
                {
                    MessageBox.Show(CHARACTER_EDIT_DUPLICATE_ERROR);
                    return;
                }
            }

            // リストを一時作成する
            List<string[]> dclist = new List<string[]>();
            dclist.Add(new string[] {
                キャラ名,
                forminfo.木工師_textBox.Text,
                forminfo.鍛冶師_textBox.Text,
                forminfo.甲冑師_textBox.Text,
                forminfo.彫金師_textBox.Text,
                forminfo.革細工師_textBox.Text,
                forminfo.裁縫師_textBox.Text,
                forminfo.錬金術師_textBox.Text,
                forminfo.調理師_textBox.Text,
                forminfo.予備1_textBox.Text,
                forminfo.予備2_textBox.Text});

            // キャラクター情報を保存する
            FileController.SaveInfo(new List<string> { キャラ名 }, Utils.Utils.CreateDictionary(dclist, SAVE_CHARACTER_INFO), FILE_PATH_CHARAINFO);

            forminfo.キャラリストボックス.Items.Add(キャラ名);
            forminfo.キャラ一覧.Items.Add(キャラ名);

            MessageBox.Show("キャラクター情報の作成が完了しました。", "くぇ～");
        }
    }
}
