namespace Authentication.Utils.Response
{
    public enum ErrorType
    {
        None = 1000,
        InvalidToken = 1001,
        InvalidCredentials,
        UnauthorizedAccess,
        GenericError = 1003,
        ReleaseMessage,
        RejectMessage,
        ModelNotValid = 1004,
        ServerError = 500,
        OperationNotAllowed = 1009,
        InvalidOpertaion

    }
}
