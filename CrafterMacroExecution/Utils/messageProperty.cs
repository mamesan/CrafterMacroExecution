namespace CrafterMacroExecution.Utils
{
    class MessageProperty
    {
        public static readonly string CHARACTER_EDIT_REQUIRED_ERROR = "キャラクター情報で、未入力の項目があります。";
        public static readonly string CHARACTER_EDIT_DUPLICATE_ERROR = "登録を行う予定のキャラクター名が、既に登録されています。";
        public static readonly string CHARACTER_EDIT_DUPLICATE_SUCCESS = "キャラクター情報の作成が完了しました。";
        public static readonly string CHARACTER_EDIT_UPDATE_DUPLICATE_SUCCESS = "キャラクター情報の更新が完了しました。";
        public static readonly string CHARACTER_EDIT_DELETE_DUPLICATE_ERROR = "削除対象のキャラクターが選択されていません。";
        public static readonly string CHARACTER_EDIT_DELETE_DUPLICATE_QA = "以下キャラ名を削除します。よろしいですか？";


        public static readonly string READ_MACRO_INFO1 = "指定されたマクロ情報が存在しませんでした。";
        public static readonly string READ_MACRO_INFO2 = "情報のみ新しく作成しますか？";
        public static readonly string READ_MACRO_NEW_INFO = "新しくマクロ情報を作成しました。";
        public static readonly string READ_MACRO_DELETE_INFO = "不要なリストを削除しました。";

        public static readonly string DELETE_MACRO_INFO = "以下マクロを削除します。よろしいですか？";
        public static readonly string CREATE_MACRO_INFO_ERROR = "必須項目を全て入力してください。";
        public static readonly string CREATE_MACRO_INFO_NAME_ERROR = "マクロ名が重複しております。";
        public static readonly string CREATE_MACRO_INFO_SUCCESS = "マクロの作成が完了しました。";
        public static readonly string EDIT_MACRO_INFO_SUCCESS = "更新が完了しました。";
    }
}
