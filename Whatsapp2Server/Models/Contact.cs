using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Whatsapp2Server.Models
{
    public class Contact
    {

        public Contact()
        {
            this.server = "";
            this.id = "";
            this.last = "";
            this.lastdate = "";
            this.profilePicSrc = "";
        }

        public string id { get; set; }
        /*        [Required] 
        */
       // [DataType(DataType.Password)]

        public string name { get; set; }
        public string server { get; set; }

        public string last { get; set; }

        public string lastdate { get; set; }

        public string profilePicSrc { get; set; }


    }
}

        
    
    
