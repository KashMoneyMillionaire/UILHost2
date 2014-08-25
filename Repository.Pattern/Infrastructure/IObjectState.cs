
using System.ComponentModel.DataAnnotations.Schema;
using UILHost.Repository.Pattern.Infrastructure;

namespace Repository.Pattern.Infrastructure
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}