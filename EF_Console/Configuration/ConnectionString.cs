﻿namespace EF_Console.Configuration
{
    /// <summary>
    /// Класс со строками подключения к БД
    /// </summary>
    static public class ConnectionString
    {
        /// <summary>
        /// Основная строка подключения
        /// </summary>
        public const string MAIN = @"Server=ASUS\SQLEXPRESS;Database=LibraryDB;" +
                "Trusted_Connection=True;TrustServerCertificate=True;";

        /// <summary>
        /// Строка подключения для тестов
        /// </summary>
        public const string TESTING = @"Server=ASUS\SQLEXPRESS;Database=LibraryDBForTesting;" +
                "Trusted_Connection=True;TrustServerCertificate=True;";
    }
}
