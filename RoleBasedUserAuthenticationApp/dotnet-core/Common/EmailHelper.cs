using System.Threading.Tasks;

namespace startup_kit_api.Common
{
    public class EmailHelper
    {
        public static async Task<bool> SendWelcomeEmail(string fullname, string email)
        {
            using EmailService emailService = new EmailService();
            emailService._body.Append("<html>");
            emailService._body.Append($"Hello {fullname}! <br /><br />");
            emailService._body.Append("Thank you for signing up!  <br /><br />");
            emailService._body.Append("Regards <br /><br />");
            emailService._body.Append("Company brand");
            emailService._body.Append("</html>");
            return await emailService.SendEmailAsync(fullname, email, $"Welcome {fullname}");
        }

        public static async Task<bool> SendRecoveryLinkEmail(string link, string fullname, string userEmal)
        {
            using EmailService emailService = new EmailService();
            emailService._body.Append("<html>");
            emailService._body.Append($"Hello {fullname}! <br /><br />");
            emailService._body.Append("Please click on the link below o change your password<br /><br />");
            emailService._body.Append("<a href='" + link + "' target='_blank'>Click here</a> <br /><br />");
            emailService._body.Append("This link is available for 24h <br /><br />");
            emailService._body.Append("Regards <br /><br />");
            emailService._body.Append("Company brand");
            emailService._body.Append("</html>");
            return await emailService.SendEmailAsync(fullname, userEmal, $"Password recovery...");
        }
    }
}
