using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prod_Cate.Models
{
    public class Product
    {
        [Key]
        public int ProductId {get;set;}


        [Required]
        public string Name {get;set;}


        [Required]
        public string Description {get;set;}


        [Required]
        //price format
        public decimal Price {get;set;}


        public DateTime Updated_At {get;set;} = DateTime.Now;


        public DateTime Created_At {get;set;} = DateTime.Now;


        //Relationships and Navigations
        public List<Connection> Connections {get;set;}  = new List<Connection>();
    
    

    
        //Display Methods
        [NotMapped]
        public string Price_Display {
            get{
                decimal price = Math.Abs(decimal.Round(Price, 2, MidpointRounding.AwayFromZero));
                string string_price = price.ToString();
                
                return $"{string_price}";
            }
        }
    
    
    
    
    
    
    
    
    
    }
}