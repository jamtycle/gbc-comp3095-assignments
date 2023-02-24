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
            var fields = GetDeepFields;
            foreach (FieldInfo field in fields)
                if (_row.Table.Columns.Contains(field.Name) && field.FieldType == _row[field.Name].GetType())
                    field.SetValue(this, _row[field.Name] is DBNull ? null : _row[field.Name]);
        }

        public DataRow ToDataRow(DataTable _skeleton, bool _deep_copy = false)
        {
            DataRow row = _skeleton.NewRow();

            IEnumerable<FieldInfo> fields;
            if (_deep_copy) fields = GetDeepFields;
            else fields = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
                if (_skeleton.Columns.Contains(field.Name))
                    row[field.Name] = field.GetValue(this) ?? DBNull.Value;
                    
            return row;
        }

        private IEnumerable<FieldInfo> GetDeepFields
        { 
            get
            {
                Type current = this.GetType();
                while(current != null)
                {
                    foreach (var field in current.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                        yield return field;
                    current = current.BaseType;
                }
            }
        }
    }
}