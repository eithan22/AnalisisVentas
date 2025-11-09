using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisVentas.Application.Interfaces
{
    // La interfaz genérica que tu 'CsvExtractor' implementará
    public interface IExtractor<T> where T : class
    {
        // Acepta un parámetro 'string' que será la ruta del archivo
        Task<IEnumerable<T>> ExtractAsync(string? parameter);
    }
}