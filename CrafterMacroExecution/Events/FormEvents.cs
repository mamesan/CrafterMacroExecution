using CrafterMacroExecution.Bean;
using CrafterMacroExecution.Impl;

namespace CrafterMacroExecution.Events
{
    class FormEvents : CrafterMacroExecution
    {
        public void init_formEvents()
        {
            キャラクター追加ボタン.Click += キャラクター追加ボタン_Click;
            キャラクター情報更新ボタン.Click += キャラクター情報更新ボタン_Click;
        }

        private void キャラクター情報更新ボタン_Click(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void キャラクター追加ボタン_Click(object sender, System.EventArgs e)
        {
            EditCharacter.createCharacterInfo(this);
        }
    }
}
