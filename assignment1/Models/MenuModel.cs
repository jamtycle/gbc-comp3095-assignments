using System.Data;
using assignment1.Data;
using assignment1.Models.Generics;
using assignment1.Models.Interfaces;

namespace assignment1.Models
{
    public class MenuModel : ModelBase
    {
        private int menu_id;
        private string label;
        private string action;
        private string controller;
        private string area;
        private string classes;

        public MenuModel(DataRow _row) : base(_row) { }

        public int MenuId { get => menu_id; set => menu_id = value; }
        public string Label { get => label; set => label = value; }
        public string Action { get => action; set => action = value; }
        public string Controller { get => controller; set => controller = value; }
        public string Area { get => area; set => area = value; }
        public string Classes { get => classes; set => classes = value; }
    }
}