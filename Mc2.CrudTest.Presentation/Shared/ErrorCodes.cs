namespace Mc2.CrudTest.Presentation.Shared
{


    public class ErrorCodes
    {
        public ErrorCodes(string ErrorDesctription, int ErrorCode)
        {
            this.ErrorDesctription = ErrorDesctription;
            this.ErrorCode = ErrorCode;
        }
        public string ErrorDesctription { get; set; }
        public int ErrorCode { get; set; }

    }
}
