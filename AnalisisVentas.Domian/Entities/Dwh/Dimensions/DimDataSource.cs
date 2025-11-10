using System;

namespace AnalisisVentas.Domain.Entities.Dwh
{
    public class DimDataSource
    {
        public int DataSourceKey { get; set; }
        public string? SourceType { get; set; }
        public DateTime? LoadDate { get; set; }
    }
}