using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Falcon.UI.Html.Admin
{
    public enum ButtonActionType
    {
        Submit,
        Add,
        Back,
        Save,
        Edit,
        Reset,
        Delete,
        SaveAndContinueEdit,
        Copy,
        Move,
        Review,
        UnReview,
        Pending,
        Publish,
        UnPublish,
        Archive,
        UnArchive,
        Headline,
        UnHeadline,
        Send,
        Help,
        Search,
        Lock,
        Unlock,
        Verify,
        Other
    }

    public class ButtonActionName
    {
        public const string Submit = "Submit";
        public const string Add = "Add";
        public const string Back = "Back";
        public const string Save = "Save";
        public const string Edit = "Edit";
        public const string Reset = "Reset";
        public const string Delete = "Delete";
        public const string SaveAndContinueEdit = "SaveAndContinueEdit";
        public const string Copy = "Copy";
        public const string Move = "Move";
        public const string Review = "Review";
        public const string UnReview = "UnReview";
        public const string Pending = "Pending";
        public const string Publish = "Publish";
        public const string UnPublish = "UnPublish";
        public const string Archive = "Archive";
        public const string UnArchive = "UnArchive";
        public const string Headline = "Headline";
        public const string UnHeadline = "UnHeadline";
        public const string Send = "Send";
        public const string Help = "Help";
        public const string Search = "Search";
        public const string Lock = "Lock";
        public const string Unlock = "Unlock";
        public const string Verify = "Verify";
        public const string Other = "Other";
    }
}
