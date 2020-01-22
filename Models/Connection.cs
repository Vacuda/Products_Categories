using System.ComponentModel.DataAnnotations;

namespace Prod_Cate.Models
{
    public class Connection
    {
        
        [Key]
        public int ConnectionId {get;set;}


        public int ProductId {get;set;}


        public int CategoryId {get;set;}


        //Relationships and Navigations

        public Product Product {get;set;}


        public Category Category {get;set;}





















    }
}