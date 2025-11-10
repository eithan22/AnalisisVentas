namespace AnalisisVentas.Domian.Repository
{
    public interface IFileReaderRepository<TClass> where TClass : class
    {
        Task<IEnumerable<TClass>> ReadFileAsync(string filePath);

    }
}
