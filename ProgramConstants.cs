namespace InformationSecurity
{
    class ProgramConstants
    {
        public static readonly string _2FA_ERROR_HEADER               = "Ошибка аутентификации";
        public static readonly string LOCAL_USER_ERROR_DESCTIPTION    = "Системе не удалось подтвердить подлинность " +
                                                                        "вашей локальной учётной записи Windows.";
        public static readonly string CODE_TO_LOGIN_ERROR_DESCTIPTION = "Указанный Вами одноразовый код не подходит пользователю.";
        public static readonly string WRONG_LOGIN_OR_PASSWORD_ERROR_DESCTIPTION = "Логин или пароль содержит ошибку.";

        public static readonly string USERS_JSON = "Resources\\Users.json";
    }
}
