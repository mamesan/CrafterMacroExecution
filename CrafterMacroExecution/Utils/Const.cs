namespace CrafterMacroExecution.Utils
{
    /// <summary>
    /// 定数クラス
    /// </summary>
    class Const
    {
        /// <summary>
        /// FF14プラグイン名
        /// </summary>
        public static readonly string ACTPLUGIN_NAME = "FFXIV_ACT_Plugin";

        /// <summary>
        /// ACTタブ用の文言
        /// </summary>
        public static readonly string TABNAME = "new_クラフタシュミレーター";

        /// <summary>
        /// プラグイン起動時門後
        /// </summary>
        public static readonly string GOSTATUS = "Started";


        /// <summary>
        /// ACT設定読込時、KEY項目
        /// </summary>
        public static readonly string INITACTSETTING = "_init";

        /// <summary>
        /// CP
        /// </summary>
        public static readonly string CP = "CP";

        /// <summary>
        /// 作業精度
        /// </summary>
        public static readonly string CRAFT_SMANSHIP = "CraftSmanship";

        /// <summary>
        /// 木工師
        /// </summary>
        public static readonly string CARPENTER = "Carpenter";

        /// <summary>
        /// 鍛冶師
        /// </summary>
        public static readonly string BLACKSMITH = "Blacksmith";

        /// <summary>
        /// 甲冑師
        /// </summary>
        public static readonly string ARMORER = "Armorer";

        /// <summary>
        /// 彫金師
        /// </summary>
        public static readonly string GOLDSMITH = "Goldsmith";

        /// <summary>
        /// 革細工師
        /// </summary>
        public static readonly string LEATHERWORKER = "Leatherworker";

        /// <summary>
        /// 裁縫師
        /// </summary>
        public static readonly string WEAVER = "Weaver";

        /// <summary>
        /// 錬金術師
        /// </summary>
        public static readonly string ALCHEMIST = "Alchemist";

        /// <summary>
        /// 調理師
        /// </summary>
        public static readonly string CULINARIAN = "Culinarian";

        /// <summary>
        /// チョコボ
        /// </summary>
        public static readonly string CHOCOBO = "Chocobo";

        /// <summary>
        /// 例のあの人
        /// </summary>
        public static readonly string AWAYUKIKUSUSHI = "AwayukiKusushi";

        /// <summary>
        /// マイスター1
        /// </summary>
        public static readonly string MEISTER1 = "Meister1";

        /// <summary>
        /// マイスター2
        /// </summary>
        public static readonly string MEISTER2 = "Meister2";

        /// <summary>
        /// マイスター3
        /// </summary>
        public static readonly string MEISTER3 = "Meister3";

        /// <summary>
        /// 加工精度
        /// </summary>
        public static readonly string CRAFT_CONTROL = "CraftControl";

        /// <summary>
        /// マクロ名
        /// </summary>
        public static readonly string MACRO_NAME = "MacroName";

        /// <summary>
        /// 必要工数
        /// </summary>
        public static readonly string NECESSARY_MAN_HOURS = "NecessaryManHours";

        /// <summary>
        /// 星数
        /// </summary>
        public static readonly string STAR_COUNT = "StarCount";

        /// <summary>
        /// 必要品質
        /// </summary>
        public static readonly string CRAFTCONTROL_COUNT = "CraftControlCount";

        /// <summary>
        /// 作るもの
        /// </summary>
        public static readonly string WHAT_MAKES = "WhatMakes";

        /// <summary>
        /// No
        /// </summary>
        public static readonly string NO = "No";

        /// <summary>
        /// 待機時間
        /// </summary>
        public static readonly string WAIT = "Wait";

        /// <summary>
        /// スキル名
        /// </summary>
        public static readonly string SSKILLNAME = "SkillName";

        /// <summary>
        /// スキルタイプ
        /// </summary>
        public static readonly string SKILLTYPE_V = "SkillTypeV";

        /// <summary>
        /// 必要CP量
        /// </summary>
        public static readonly string CP_V = "CPV";

        /// <summary>
        /// 作業工数
        /// </summary>
        public static readonly string CRAFT_SMANSHIP_V = "CraftSmanshipV";

        /// <summary>
        /// 品質工数
        /// </summary>
        public static readonly string CRAFT_CONTROL_V = "CraftControlV";

        /// <summary>
        /// 確率
        /// </summary>
        public static readonly string SUCCESS_RATE_V = "SuccessRateV";

        /// <summary>
        /// タブType
        /// </summary>
        public static readonly string TAB_TYPE_V = "TabType";

        /// <summary>
        /// キャラクター情報保存用フラグ
        /// </summary>
        public static readonly int SAVE_CHARACTER_INFO = 1;

        /// <summary>
        /// マクロ情報保存用フラグ
        /// </summary>
        public static readonly int SAVE_MACRO_INFO = 2;

        /// <summary>
        /// マクロ詳細情報保存用フラグ
        /// </summary>
        public static readonly int SAVE_MACRO_DETAIL_INFO = 3;

        /// <summary>
        /// キャラ情報作成情報
        /// </summary>
        public static readonly string[] CHARA_INFO = { CP, CRAFT_SMANSHIP, CRAFT_CONTROL, CARPENTER, BLACKSMITH, ARMORER, GOLDSMITH, LEATHERWORKER, WEAVER, ALCHEMIST, CULINARIAN, CHOCOBO, AWAYUKIKUSUSHI, MEISTER1, MEISTER2, MEISTER3 };

        /// <summary>
        /// マクロ情報作成情報
        /// </summary>
        public static readonly string[] MACRO_INFO = { NECESSARY_MAN_HOURS, STAR_COUNT, CRAFTCONTROL_COUNT, WHAT_MAKES };

        /// <summary>
        /// マクロ詳細情報作成情報
        /// </summary>
        public static readonly string[] MACRO_DETAIL_INFO = { NO, WAIT };

        /// <summary>
        /// スキル情報作成情報
        /// </summary>
        public static readonly string[] SKILL_INFO = { SSKILLNAME, SKILLTYPE_V, CP_V, CRAFT_SMANSHIP_V, CRAFT_CONTROL_V, SUCCESS_RATE_V, TAB_TYPE_V };

        /// <summary>
        /// XML形式
        /// </summary>
        public static readonly string FILEEXTENSION_XML = "*.xml";

        /// <summary>
        /// png形式
        /// </summary>
        public static readonly string FILEEXTENSION_PNG = "*.png";

        /// <summary>
        /// キャラクター情報格納ファイルパス
        /// </summary>
        public static readonly string FILE_PATH_CHARAINFO = "ACT_CrafterSimulator\\Config\\ACT_CrafterSimulator.CharacterInfo.xml";

        /// <summary>
        /// マクロ情報格納ファイルパス
        /// </summary>
        public static readonly string FILE_PATH_MACROINFO = "ACT_CrafterSimulator\\Config\\ACT_CrafterSimulator.MacroInfo.xml";

        /// <summary>
        /// スキル情報格納ファイルパス
        /// </summary>
        public static readonly string FILE_PATH_SKILLINFO = "ACT_CrafterSimulator\\Config\\ACT_CrafterSimulator.SkillList.xml";

        /// <summary>
        /// マクロファイルのパス
        /// </summary>
        public static readonly string FILE_PATH_TEMPMACRO = "ACT_CrafterSimulator\\Template\\";

        /// <summary>
        /// アディッショナル設定ファイルのパス
        /// </summary>
        public static readonly string FILE_PATH_ADDITIONAL = "ACT_CrafterSimulator\\Config\\Additional_config.xml";

        /// <summary>
        /// お気に入りリスト設定ファイルのパス
        /// </summary>
        public static readonly string FILE_PATH_FAVORITE = "ACT_CrafterSimulator\\Config\\ACT_CrafterSimulator.FavoriteInfo.xml";

        /// <summary>
        /// マクロアイコン設定フォルダパス
        /// </summary>
        public static readonly string FILE_PATH_ICON = "ACT_CrafterSimulator\\icon\\";

        /// <summary>
        /// 背景表示用のパス
        /// </summary>
        public static readonly string FILE_PATH_IMAGE = "ACT_CrafterSimulator\\Config\\chocobo\\設定.png";
    }
}
