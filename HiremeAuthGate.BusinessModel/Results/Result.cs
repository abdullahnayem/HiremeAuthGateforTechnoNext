namespace HiremeAuthGate.BusinessModel.Results
{
    /// <summary>
    /// Generic result wrapper for operations that return a value.
    /// Provides a consistent way to handle success and failure scenarios.
    /// </summary>
    /// <typeparam name="T">The type of the value returned on success.</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// Gets whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; }
        
        /// <summary>
        /// Gets the value returned on successful operation.
        /// Null if the operation failed.
        /// </summary>
        public T? Value { get; }
        
        /// <summary>
        /// Gets the error message if the operation failed.
        /// Null if the operation was successful.
        /// </summary>
        public string? ErrorMessage { get; }
        
        /// <summary>
        /// Gets the exception that occurred if the operation failed.
        /// Null if the operation was successful or no exception occurred.
        /// </summary>
        public Exception? Exception { get; }

        /// <summary>
        /// Private constructor to enforce factory method usage.
        /// </summary>
        /// <param name="isSuccess">Whether the operation was successful.</param>
        /// <param name="value">The value returned on success.</param>
        /// <param name="errorMessage">The error message on failure.</param>
        /// <param name="exception">The exception that occurred on failure.</param>
        private Result(bool isSuccess, T? value, string? errorMessage = null, Exception? exception = null)
        {
            IsSuccess = isSuccess;
            Value = value;
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        /// <summary>
        /// Creates a successful result with the specified value.
        /// </summary>
        /// <param name="value">The value to return.</param>
        /// <returns>A successful result containing the value.</returns>
        public static Result<T> Success(T value) => new(true, value);
        
        /// <summary>
        /// Creates a failed result with the specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message describing the failure.</param>
        /// <returns>A failed result with the error message.</returns>
        public static Result<T> Failure(string errorMessage) => new(false, default, errorMessage);
        
        /// <summary>
        /// Creates a failed result with the specified error message and exception.
        /// </summary>
        /// <param name="errorMessage">The error message describing the failure.</param>
        /// <param name="exception">The exception that occurred.</param>
        /// <returns>A failed result with the error message and exception.</returns>
        public static Result<T> Failure(string errorMessage, Exception exception) => new(false, default, errorMessage, exception);

        /// <summary>
        /// Maps the result to a new type using the specified mapper function.
        /// If the original result was successful, applies the mapper to the value.
        /// If the original result failed, returns a failed result with the same error.
        /// </summary>
        /// <typeparam name="TNew">The new type to map to.</typeparam>
        /// <param name="mapper">The function to transform the value.</param>
        /// <returns>A new result with the mapped value or the original error.</returns>
        public Result<TNew> Map<TNew>(Func<T, TNew> mapper)
        {
            return IsSuccess && Value != null 
                ? Result<TNew>.Success(mapper(Value)) 
                : Result<TNew>.Failure(ErrorMessage ?? "Unknown error", Exception);
        }
    }

    /// <summary>
    /// Non-generic result wrapper for operations that don't return a value.
    /// Provides a consistent way to handle success and failure scenarios.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Gets whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; }
        
        /// <summary>
        /// Gets the error message if the operation failed.
        /// Null if the operation was successful.
        /// </summary>
        public string? ErrorMessage { get; }
        
        /// <summary>
        /// Gets the exception that occurred if the operation failed.
        /// Null if the operation was successful or no exception occurred.
        /// </summary>
        public Exception? Exception { get; }

        /// <summary>
        /// Private constructor to enforce factory method usage.
        /// </summary>
        /// <param name="isSuccess">Whether the operation was successful.</param>
        /// <param name="errorMessage">The error message on failure.</param>
        /// <param name="exception">The exception that occurred on failure.</param>
        private Result(bool isSuccess, string? errorMessage = null, Exception? exception = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        /// <summary>
        /// Creates a successful result.
        /// </summary>
        /// <returns>A successful result.</returns>
        public static Result Success() => new(true);
        
        /// <summary>
        /// Creates a failed result with the specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message describing the failure.</param>
        /// <returns>A failed result with the error message.</returns>
        public static Result Failure(string errorMessage) => new(false, errorMessage);
        
        /// <summary>
        /// Creates a failed result with the specified error message and exception.
        /// </summary>
        /// <param name="errorMessage">The error message describing the failure.</param>
        /// <param name="exception">The exception that occurred.</param>
        /// <returns>A failed result with the error message and exception.</returns>
        public static Result Failure(string errorMessage, Exception exception) => new(false, errorMessage, exception);
    }
}
