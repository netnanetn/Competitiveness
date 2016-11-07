using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Falcon.Data.Domain;
using System.Web.Security;

namespace Falcon.Security
{
    public interface IAuthenticationService
    {
        void SignIn(User user, bool createPersistentCookie);        
        void SetAuthenticatedUserForRequest(User user);
        User GetAuthenticatedUser(FormsIdentity identity);
        User GetAuthenticatedUser();
        int GetAuthenticatedUserId();

        //void AccountSignIn(Account account, bool createPersistentCookie);
        //void FakeAccountSignIn(Account account);
        //void SetAuthenticatedAccountForRequest(Account account);
        //Account GetAuthenticatedAccount(FormsIdentity identity);
        //Account GetAuthenticatedAccount();
        //int GetAuthenticatedAccountId();

        void SignOut();
    }
}
