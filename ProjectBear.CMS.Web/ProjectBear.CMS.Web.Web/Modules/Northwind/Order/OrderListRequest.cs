using Serenity.Services;

namespace ProjectBear.CMS.Web.Northwind
{
    public class OrderListRequest : ListRequest
    {
        public int? ProductID { get; set; }
    }
}