using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace MiddleWare.Models.DataSet
{
    public class VariableState
    {
        public VariableState()
        {
        }
        [Required(ErrorMessage = "Username is required")]
        public JToken Value { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public bool IsValid => Value != null;
    }
}
