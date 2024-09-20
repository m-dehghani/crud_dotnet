using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mc2.CrudTest.Presentation.Server.Migrations.ReadModelDb
{
    /// <inheritdoc />
    public partial class createView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE OR REPLACE VIEW public.Events_View\r\n AS\r\n SELECT \"Data\"::json ->> 'FirstName' as \"FirstName\",\r\n\t\"Data\"::json ->> 'LastName' as \"LastName\" ,\r\n\t\"Data\"::json #>> '{Email,Value}' as \"Email\" ,\r\n\t\"Data\"::json #>> '{BankAccount, Value}' as \"BankAccount\" , Date(\"Data\"::json #>> '{DateOfBirth, Value}') as \"DateOfBirth\",\r\n\t\r\n\t\"Data\"::json #>> '{PhoneNumber, Value}' as \"PhoneNumber\",\r\n\t\"Data\"::json ->> 'IsDeleted' as \"IsDeleted\" ,\r\n\t\r\n\t\"OccurredOn\", \"event_type\",\"AggregateId\"\r\n   FROM \"Events\";\r\n\r\nALTER TABLE public.Events_View\r\n    OWNER TO postgres;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
