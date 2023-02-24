using System.Data;

namespace assignment1.Models.Interfaces
{
    public interface IDataRow
    {
        public DataRow ToDataRow(DataTable _skeleton, bool _deep_copy);
        public void FromDataRow(DataRow _row);
    }
}