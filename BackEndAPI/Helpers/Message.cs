namespace BackEndAPI.Helpers
{
    public class Message
    {
        public const string LoginFailed = "Username or password is incorrect. Please try again!";
        public const string ChangePasswordSucceed = "Your password has been changed successfully!";
        public const string OldPasswordIncorrect = "Your current password is incorrect!";
        public const string UnauthorizedUser = "User is not authorized";
        public const string UserNotFound = "User not found!";        
        public const string AssignmentNotFound = "Assignment not found";
        public const string InternalError = "Internal server error!";
        public const string InvalidId = "Invalid Id!";
        public const string NullFirstName = "First Name can not be null!";
        public const string EmptyOrSpacesFirstName = "First Name can not be empty or contains only spaces!";
        public const string NullLastName = "Last Name can not be null!";
        public const string EmptyOrSpacesLastName = "Last Name can not be empty or contains only spaces!";
        public const string NullOrEmptyUsername = "Username can not be null or empty!";
        public const string NullUser = "User can not be null!";
        public const string NullInputModel = "Input model is null";
        public const string NullSearchQuery = "Search query is null";
        public const string RestrictedAge = "User is under 18. Please select a different date";
        public const string WeekendJoinedDate = "Joined date is Saturday or Sunday. Please select a different date";
        public const string JoinedBeforeBirth ="Joined date is not later than date of birth. Please select a different date";
        public const string TriedToCreateReturnRequestForSomeoneElseAssignment = "You are trying to create a return request on an asset assigned to someone else. Please do that on assets assigned to YOU.";
        public const string AssignedAssetNotAccepted = "The assigned asset has not been accepted. Please wait until the asset is accepted";
        public const string NullOrEmptyCategoryName = "Category name can not be null or empty!";
        public const string NullOrEmptyCategoryCode = "Category code can not be null or empty!";
        public const string NullAssetCategory = "Asset Category can not be null!";
        public const string CategoryNameExisted = "Category is already existed. Please enter a different category";
        public const string CategoryCodeExisted = "Prefix is already existed. Please enter a different prefix";
        public const string NullAsset = "Asset can not be null!";
        public const string AssetNumberError = "Can not get number of assets!";
        public const string NullOrEmptyPrefix = "Prefix can not be empty or contains only spaces!";
        public const string ReturnRequestNotFound = "Return request not found";
        public const string AssetHadHistoricalAssignment = "Cannot delete the asset because it belongs to one or more historical assignments. If the asset is not able to be used anymore, please update its state in Edit Asset page!";
        public const string AssignmentAccepted = "Assignment accepted!";
        public const string AssignmentDeclined = "Assignment declined!";
        public const string AssignmentUndidRespone = "Assignment undid respone!";
        public const string AmbiguousSortConfig = "Ambiguous sort configuration";
    }
}