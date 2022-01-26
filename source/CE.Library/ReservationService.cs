using System.Data;
using CE.Library.Extraneous;
using Serilog;
using System.Data.SqlClient;

namespace CE.Library
{
    public class ReservationService
    {
        public static ILogger Logger = Log.Logger;

        public SmtpClient SmtpClient { get; set; }

        public ReservationService()
        {
            this.SmtpClient = new SmtpClient();
        }

        public void AddReservation(string name, DateTime dateTime)
        {
            bool success = true;

            Reservation reservation = new Reservation()
            {
                Id = Guid.NewGuid(),
                Name = name,
                DateTime = dateTime,
                IsConfirmed = false,
            };

            Logger.Information("Object ready for insertion.");

            try
            {
                using (var connection = new SqlConnection("thisIsMyConnectionString"))
                {
                    connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        var command = connection.CreateCommand();
                        command.Transaction = transaction;

                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "dbo.insert_reservation";

                        command.Parameters.AddWithValue("@Id", reservation.Id);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@DateTime", dateTime);
                        command.Parameters.AddWithValue("@IsConfirmed", reservation.IsConfirmed);

                        command.ExecuteNonQueryAsync();
                        transaction.Commit();
                    }

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Logger.Information("Something bad happened.");
                success = false;
            }

            if (success)
            {
                this.SmtpClient.SendEmail($"Your reservation has been confirmed at {reservation.DateTime}");
            }
        }
    }
}