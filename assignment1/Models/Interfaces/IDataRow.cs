using System.Data;

namespace assignment1.Models.Interfaces
{
    public interface IDataRow
    {
        public DataRow ToDataRow(DataTable _skeleton);
        public void FromDataRow(DataRow _row);
    }
}