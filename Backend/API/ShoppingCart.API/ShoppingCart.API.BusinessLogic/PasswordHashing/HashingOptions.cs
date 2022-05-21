namespace ShoppingCart.API.BusinessLogic
{
    /// <summary>
    /// Summary:
    //     Represents the configuration options for the `PasswordHashManager`.
    /// </summary>
    public sealed class HashingOptions
    {
        public int Iterations { get; set; } = 10000;
    }
}
