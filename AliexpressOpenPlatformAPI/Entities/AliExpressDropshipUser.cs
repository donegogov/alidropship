using System.ComponentModel.DataAnnotations;

namespace AliexpressOpenPlatformAPI.Entities
{
    public class AliExpressDropshipUser
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string StoreURL { get; set; }
    }
}
