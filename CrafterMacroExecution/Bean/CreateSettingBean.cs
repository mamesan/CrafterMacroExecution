namespace CrafterMacroExecution.Bean
{
    public interface ICreateSettingBean
    {
        // 基本項目
        string Iキャラ名 { get; set; }
        bool I飯チェック { get; set; }
        bool I薬チェック { get; set; }
        bool I修理チェック { get; set; }
        bool Iアディチェック { get; set; }

        // 実行マクロ構成
        bool Iマクロ一個だけチェック { get; set; }
        bool Iマクロ複数チェック { get; set; }
    }

    public class CreateSettingBean : ICreateSettingBean
    {
        private string iキャラ名;
        private bool i飯チェック;
        private bool i薬チェック;
        private bool i修理チェック;
        private bool iアディチェック;
        private bool iマクロ一個だけチェック;
        private bool iマクロ複数チェック;

        public string Iキャラ名
        {
            get { return iキャラ名; }
            set { iキャラ名 = value; }
        }
        public bool Iマクロ複数チェック
        {
            get { return iマクロ複数チェック; }
            set { iマクロ複数チェック = value; }
        }
        public bool Iマクロ一個だけチェック
        {
            get { return iマクロ一個だけチェック; }
            set { iマクロ一個だけチェック = value; }
        }
        public bool Iアディチェック
        {
            get { return iアディチェック; }
            set { iアディチェック = value; }
        }
        public bool I修理チェック
        {
            get { return i修理チェック; }
            set { i修理チェック = value; }
        }
        public bool I薬チェック
        {
            get { return i薬チェック; }
            set { i薬チェック = value; }
        }
        public bool I飯チェック
        {
            get { return i飯チェック; }
            set { i飯チェック = value; }
        }
    }
}

