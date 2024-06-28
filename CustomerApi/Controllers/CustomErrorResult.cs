namespace CustomerApi.Controllers
{
    /// <summary>
    /// Represents a custom error result.
    /// </summary>
    public class CustomErrorResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation succeeded.
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Gets or sets the collection of error messages.
        /// </summary>
        public IEnumerable<string>? Errors { get; set; }
    }
}