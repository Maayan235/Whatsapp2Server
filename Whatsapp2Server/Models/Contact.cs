using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Whatsapp2Server.Models
{
    public class Contact
    {


        public string id { get; set; }
        /*        [Required] 
        */
        [DataType(DataType.Password)]

        public string name { get; set; }
        public string server { get; set; }

        public string last { get; set; }

        public string lastdate { get; set; }


    }
}

        
    
    
