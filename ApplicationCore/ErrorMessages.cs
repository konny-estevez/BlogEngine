namespace ApplicationCore
{
    public static class ErrorMessages
    {
        public static readonly string DuplicatedRecordPattern = "Cannot insert duplicate";

        //  Users error messages
        public static readonly string DuplicatedUser = "Cannot insert duplicate user record.";
        public static readonly string ErrorCreatingUser = "Error creating a new user";
        public static readonly string UserNotFound = "User not found";
        public static readonly string AccountLockedOut = "Account is locked out.";
        public static readonly string AccountNotConfirmed = "Account is not confirmed.";
        public static readonly string AccountRequires2FA = "Login requires 2 factor authentication.";
        public static readonly string FailedLogin = "Some error occurs in the login attempt.";
        public static readonly string ErrorConfirmAccount = "Can not confirm email account.";
        public static readonly string ConfirmTokenInvalid = "Confirmation token is invalid.";
        public static readonly string AccountTokenError = "Confirmation token is not valid or account not exists.";

        //  Posts error messages 
        public static readonly string ErrorCreatingPost = "Error creating a new post";
        public static readonly string DuplicatedPost = "Cannot insert duplicate post record.";
        public static readonly string PostNotFound = "Post not found";
        public static readonly string ErrorGettingPosts = "Error retrieving the user's posts.";
        public static readonly string UserIdNotExists = "User with the current Id does not exist.";
        public static readonly string CannotEditPost = "The post can not be edited.";
        public static readonly string UserNotAllowed = "User is not allowed to create a post.";
        public static readonly string UserNotAllowedEdit = "User not allowed to edit post.";
        public static readonly string CannotVerifyIdentity = "Can not verify identity to update the post.";
        public static readonly string PostIsApproved = "Post is approved, can not be edited or deleted.";

        //  Comments error messages
        public static readonly string ErrorGettingComments = "Error retrieving the post's comments.";
        public static readonly string PostIdNotExists = "Post with the current If does not exist.";
        public static readonly string ErrorCreatingComment = "Error creating a new comment.";
        public static readonly string DuplicatedComment = "Cannot insert duplicate comment record.";
        public static readonly string CommentNotFound = "Comment not found.";
    }
}
