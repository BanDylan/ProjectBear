using Serenity.Services;

namespace ProjectBear.CMS.Northwind
{
    public class OrderListRequest : ListRequest
    {
        public int? ProductID { get; set; }
    }
}