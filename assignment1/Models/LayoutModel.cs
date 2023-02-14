using System.Data;
using assignment1.Data;
using assignment1.Models.Generics;
using assignment1.Models.Interfaces;

namespace assignment1.Models
{
    public class LayoutModel
    {
        private IEnumerable<MenuModel> menus;

        public LayoutModel()
        {

        }

        public IEnumerable<MenuModel> Menus { get => menus; set => menus = value; }
    }

    public class LayoutModel<T> : LayoutModel
    {
        private T data;

        public LayoutModel() : base()
        {

        }

        public LayoutModel(DataRow _row)
        {
            // If a row is passed to this model it build dynamically the inner row
            data = (T)Activator.CreateInstance(typeof(T), _row);
        }

        public T Data { get => data; set => data = value; }
    }
}