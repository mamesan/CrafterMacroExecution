namespace CrafterMacroExecution.Bean
{
    /// <summary>
    /// キャラクター情報を格納するクラス(インターフェイス)
    /// </summary>
    public interface ICharacterBean
    {
        /// <summary>
        /// CPを格納する
        /// </summary>
        string CP { get; set; }

        /// <summary>
        /// 作業精度を格納する
        /// </summary>
        string CraftSmanship { get; set; }

        /// <summary>
        /// 加工精度を格納する
        /// </summary>
        string CraftControl { get; set; }

        /// <summary>
        /// 木工師
        /// </summary>
        string Carpenter { get; set; }

        /// <summary>
        /// 鍛冶師
        /// </summary>
        string Blacksmith { get; set; }

        /// <summary>
        /// 甲冑師
        /// </summary>
        string Armorer { get; set; }

        /// <summary>
        /// 彫金師
        /// </summary>
        string Goldsmith { get; set; }

        /// <summary>
        /// 革細工師
        /// </summary>
        string Leatherworker { get; set; }

        /// <summary>
        /// 裁縫師
        /// </summary>
        string Weaver { get; set; }

        /// <summary>
        /// 錬金術師
        /// </summary>
        string Alchemist { get; set; }

        /// <summary>
        /// 調理師
        /// </summary>
        string Culinarian { get; set; }

        /// <summary>
        /// チョコボ師
        /// </summary>
        string ChocoboMeister { get; set; }

        /// <summary>
        /// 例のあの人
        /// </summary>
        string AwayukiKusushi { get; set; }

        /// <summary>
        /// マイスター1
        /// </summary>
        string Meister1 { get; set; }

        /// <summary>
        /// マイスター2
        /// </summary>
        string Meister2 { get; set; }

        /// <summary>
        /// マイスター3
        /// </summary>
        string Meister3 { get; set; }
    }

    /// <summary>
    /// キャラクター情報を格納するクラス
    /// </summary>
    public class CharacterBean : ICharacterBean
    {
        private string cP;
        private string craftSmanship;
        private string craftControl;
        private string carpenter;
        private string blacksmith;
        private string armorer;
        private string goldsmith;
        private string leatherworker;
        private string weaver;
        private string alchemist;
        private string culinarian;
        private string chocoboMeister;
        private string awayukiKusushi;
        private string meister1;
        private string meister2;
        private string meister3;


        /// <summary>
        /// CP定義
        /// </summary>
        public string CP
        {
            get { return this.cP; }
            set { this.cP = value; }
        }

        /// <summary>
        /// 作業精度定義
        /// </summary>
        public string CraftSmanship
        {
            get { return this.craftSmanship; }
            set { this.craftSmanship = value; }
        }

        /// <summary>
        /// 加工精度定義
        /// </summary>
        public string CraftControl
        {
            get { return this.craftControl; }
            set { this.craftControl = value; }
        }

        /// <summary>
        /// 木工師定義
        /// </summary>
        public string Carpenter
        {
            get { return this.carpenter; }
            set { this.carpenter = value; }
        }

        /// <summary>
        /// 鍛冶師定義
        /// </summary>
        public string Blacksmith
        {
            get { return this.blacksmith; }
            set { this.blacksmith = value; }
        }

        /// <summary>
        /// 甲冑師定義
        /// </summary>
        public string Armorer
        {
            get { return this.armorer; }
            set { this.armorer = value; }
        }

        /// <summary>
        /// 彫金師定義
        /// </summary>
        public string Goldsmith
        {
            get { return this.goldsmith; }
            set { this.goldsmith = value; }
        }

        /// <summary>
        /// 革細工師定義
        /// </summary>
        public string Leatherworker
        {
            get { return this.leatherworker; }
            set { this.leatherworker = value; }
        }

        /// <summary>
        /// 裁縫師定義
        /// </summary>
        public string Weaver
        {
            get { return this.weaver; }
            set { this.weaver = value; }
        }

        /// <summary>
        /// 錬金術師定義
        /// </summary>
        public string Alchemist
        {
            get { return this.alchemist; }
            set { this.alchemist = value; }
        }

        /// <summary>
        /// 調理師定義
        /// </summary>
        public string Culinarian
        {
            get { return this.culinarian; }
            set { this.culinarian = value; }
        }

        /// <summary>
        /// チョコボ師定義
        /// </summary>
        public string ChocoboMeister
        {
            get { return this.chocoboMeister; }
            set { this.chocoboMeister = value; }
        }

        /// <summary>
        /// 例のあの人定義
        /// </summary>
        public string AwayukiKusushi
        {
            get { return this.awayukiKusushi; }
            set { this.awayukiKusushi = value; }
        }

        /// <summary>
        /// マイスター1
        /// </summary>
        public string Meister1
        {
            get { return this.meister1; }
            set { this.meister1 = value; }
        }

        /// <summary>
        /// マイスター2
        /// </summary>
        public string Meister2
        {
            get { return this.meister2; }
            set { this.meister2 = value; }
        }

        /// <summary>
        /// マイスター3
        /// </summary>
        public string Meister3
        {
            get { return this.meister3; }
            set { this.meister3 = value; }
        }
    }
}

