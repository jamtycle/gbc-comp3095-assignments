using System.Data;

namespace assignment1.Models.Interfaces
{
    public interface IDataRow
    {
        public DataRow ToDataRow(DataTable _skeleton);
        public bool FromDataRow(DataRow _row);
    }
}