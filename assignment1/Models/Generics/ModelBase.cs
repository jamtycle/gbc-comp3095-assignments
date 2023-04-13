using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
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
                if (_row.Table.Columns.Contains(field.Name) && CheckTypeCompatability(field.FieldType, _row[field.Name].GetType()))
                    field.SetValue(this, _row[field.Name] is DBNull ? null : _row[field.Name]);
        }

        private static bool CheckTypeCompatability(Type _type1, Type _type2)
        {
            var pattern = new Regex("\\[(.*?)\\]");
            if (_type1.ToString().Contains("Nullable") && _type2.ToString().Contains("Nullable"))
                return pattern.Match(_type1.ToString()).Groups.Values.LastOrDefault().Value.Equals(pattern.Match(_type2.ToString()).Groups.Values.LastOrDefault().Value);

            if (_type1.ToString().Contains("Nullable"))
                return pattern.Match(_type1.ToString()).Groups.Values.LastOrDefault().Value.Equals(_type2.ToString());

            if (_type2.ToString().Contains("Nullable"))
                return pattern.Match(_type2.ToString()).Groups.Values.LastOrDefault().Value.Equals(_type1.ToString());

            return _type1.Equals(_type2);
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