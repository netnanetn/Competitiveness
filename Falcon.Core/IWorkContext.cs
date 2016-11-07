//using Falcon.Domain.Customers;
//using Falcon.Domain.Directory;
//using Falcon.Domain.Localization;
//using Falcon.Domain.Tax;
using Falcon.Themes;
using Falcon.Data.Domain;

namespace Falcon
{
    /// <summary>
    /// Work context
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// Gets or sets the current User
        /// </summary>
        User CurrentUser { get; }
        int CurrentUserId { get; }

        //Account CurrentAccount { get; }
        //int CurrentAccountId { get; }

        /// <summary>
        /// Get or set value indicating whether we're in admin area
        /// </summary>
        bool IsAdmin { get; set; }
    }
}
