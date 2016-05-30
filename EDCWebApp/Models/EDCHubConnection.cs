using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EDCWebApp.Models
{
    public class EDCHubConnection
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public string HubConnectionID { get; set; }

        public bool Connected { get; set; }
        public string LoginDate { get; set; }
        public string LoginTime { get; set; }

    }
}