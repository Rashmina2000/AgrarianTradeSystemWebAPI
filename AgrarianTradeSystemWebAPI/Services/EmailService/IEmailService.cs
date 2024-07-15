using AgrarianTradeSystemWebAPI.Models.EmailModels;
using AgrarianTradeSystemWebAPI.Models.UserModels;

namespace AgrarianTradeSystemWebAPI.Services.EmailService
{
    public interface IEmailService
    {
        void SendUserRegisterEmail(string to, string fname, string lname, string token);
        void SendRegisterEmail(string to, string fname, string lname, string token);
        void passwordResetEmail(string to, string token);
        void verifyEmail(string to, string token);
        void rejectUserMail(string to, string fname, string lname, string reasons);
        void approveUserMail(string to, string fname, string lname);
    }
}
