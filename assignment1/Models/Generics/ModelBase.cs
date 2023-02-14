using System.Data;
using System.Reflection;
using assignment1.Models.Interfaces;

namespace assignment1.Models.Generics
{
    public abstract class ModelBase : IDataRow
    {
        public ModelBase()
        {
            
        }

        public ModelBase(DataRow _row)
        {
            FromDataRow(_row);
        }

        public void FromDataRow(DataRow _row)
        {
            FieldInfo[] fields = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
                if (_row.Table.Columns.Contains(field.Name) && field.FieldType == _row[field.Name].GetType())
                    field.SetValue(this, _row[field.Name] is DBNull ? null : _row[field.Name]);
        }

        public DataRow ToDataRow(DataTable _skeleton)
        {
            DataRow row = _skeleton.NewRow();
            FieldInfo[] fields = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
                if (_skeleton.Columns.Contains(field.Name))
                    row[field.Name] = field.GetValue(this) ?? DBNull.Value;
            return row;
        }
    }
}