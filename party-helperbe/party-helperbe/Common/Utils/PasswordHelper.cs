﻿namespace party_helperbe.Common.Utils
{
    public static class PasswordHelper
    {
        public static string EncryptPassword(string? password)
        {


            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string? password, string? hashedPassword)
        {

            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
