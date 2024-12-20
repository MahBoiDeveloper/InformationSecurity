﻿namespace InformationSecurity
{
    class ProgramConstants
    {
        public static readonly string DS_ERROR_HEADER                 = "Ошибка работы ЭЦП";
        public static readonly string DS_NO_DATA_DESCRIPTION          = "Отсутствуют данные для сохранения в таблицу.";

        public static readonly string RSA_ERROR_HEADER                = "Ошибка работы RSA";
        public static readonly string RSA_WRONG_KEYS_DESCRIPTION      = "Открытый и закрытый ключи не подходят, т.к. сообщение нельзя однозначно шифровать и расшифровать.";
        public static readonly string RSA_EMPTY_FIELDS                = "Ни одно поле (Открытая экспонента, Закрытая экспонента, Произведение простых чисел) не должно быть пустым!";

        public static readonly string KUZNECHIK_ERROR_HEADER          = "Ошибка работы Кузнечика";
        public static readonly string KEY1_INCOMPLETE_DESCRIPTION     = "1-ый ключ не равен по длине 16 байт. Длина ключа: {0} байт.";
        public static readonly string KEY2_INCOMPLETE_DESCRIPTION     = "2-ой ключ не равен по длине 16 байт. Длина ключа: {0} байт.";
        public static readonly string NO_DATA_TO_ENCRYPT_DESCRIPTION  = "Отсутствуют данные в поле \"Текст\". Шифрация пустых полей невозможна.";
        public static readonly string NO_DATA_TO_DECRYPT_DESCRIPTION  = "Отсутствуют данные в поле \"Шифр\". Дешифрация пустых полей невозможна.";
        public static readonly string OUTPUT_DATA_IS_NOT_A_HEX_DESC   = "Данные поле \"Шифр\" не является шифром.\n\nТекст ошибки: ";
        public static readonly string NO_DATA_TO_SAVE_DESCRIPTION     = "Нельзя сохранить данные в таблицу, т.к. одно из полей пустое.";

        public static readonly string _2FA_ERROR_HEADER               = "Ошибка аутентификации";
        public static readonly string LOCAL_USER_ERROR_DESCTIPTION    = "Системе не удалось подтвердить подлинность " +
                                                                        "вашей локальной учётной записи Windows.";
        public static readonly string CODE_TO_LOGIN_ERROR_DESCTIPTION = "Указанный Вами одноразовый код не подходит пользователю.";
        public static readonly string WRONG_LOGIN_OR_PASSWORD_ERROR_DESCTIPTION = "Логин или пароль содержит ошибку.";
        public static readonly string EMPTY_LOGIN_ERROR_DESCTIPTION   = "Авторизация под пустым логином невозможна!";
        public static readonly string SPEC_SYMBOLS_ERROR_DESCTIPTION  = "Использование специальных символов запрещено!";

        public static readonly string SALTS_JSON                      = "Resources\\Salts.json";
        public static readonly string USERS_JSON                      = "Resources\\Users.json";
        public static readonly string RSA_JSON                        = "Resources\\RSA.json";
        public static readonly string KUZNECHIK_JSON                  = "Resources\\Kuznechik.json";
        public static readonly string DATABASE_JSON                   = "Resources\\Database.json";
        public static readonly string DIGITAL_SIGNATURE_JSON          = "Resources\\DigitalSignature.json";

        public static readonly string DIGITAL_SIGNATURE_PNG           = "Resources\\ЭЦП.png";
    }
}
