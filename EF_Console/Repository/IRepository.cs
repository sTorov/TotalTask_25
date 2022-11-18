namespace EF_Console.Repository
{
    /// <summary>
    /// Основные действия над моделями
    /// </summary>
    internal interface IRepository<T>
    {
        /// <summary>
        /// Получение объекта <typeparamref name="T"/> по его Id
        /// </summary>
        T? FindById(int id);
        /// <summary>
        /// Получение списка всех объектов типа <typeparamref name="T"/>
        /// </summary>
        List<T> FindAll();
        /// <summary>
        /// Добавление объекта <typeparamref name="T"/> в БД
        /// </summary>
        /// <returns>Возвращает Id добавленной записи</returns>
        int Add(T model);
        /// <summary>
        /// Удаление объект типа <typeparamref name="T"/> из БД
        /// </summary>
        /// <returns>Возвращает количество затронутых строк</returns>
        int Delete(T model);
    }
}
