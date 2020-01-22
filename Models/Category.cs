using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Prod_Cate.Models
{
    public class Category
    {

        [Key]
        public int CategoryId {get;set;}


        [Required]
        public string Name {get;set;}


        public DateTime Updated_At {get;set;} = DateTime.Now;


        public DateTime Created_At {get;set;} = DateTime.Now;


        //Relationships and Navigations
        public List<Connection> Connections {get;set;} = new List<Connection>();




















    }
}