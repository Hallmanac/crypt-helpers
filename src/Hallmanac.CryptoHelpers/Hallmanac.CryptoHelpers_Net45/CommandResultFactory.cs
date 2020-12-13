namespace Hallmanac.CryptoHelpers

{
    /// <summary>
    /// </summary>
    public class CommandResultFactory
    {
        /// <summary>
        ///     Creates a new <see cref="CommandResult" /> with the IsSuccessful property set to true.
        /// </summary>
        /// <param name="message">
        ///     Optional message that is saved to the Message property on the <see cref="CommandResult" />
        /// </param>
        public static CommandResult Ok(string message = null)
        {
            return new CommandResult(true, message);
        }


        /// <summary>
        ///     Creates a new <see cref="CommandResult{T}" /> with the IsSuccessful property set to true.
        /// </summary>
        /// <param name="result">The underlying value of the new <see cref="CommandResult{T}" /></param>
        /// <param name="message">
        ///     Optional message that is saved to the Message property on the <see cref="CommandResult{T}" />
        /// </param>
        public static CommandResult<T> Ok<T>(T result, string message = null)
        {
            return new CommandResult<T>(result, true, message);
        }

        /// <summary>
        ///     Creates a new <see cref="CommandResult{T}" /> with the IsSuccessful property set to true.
        /// </summary>
        /// <typeparam name="T">The type of the underlying Value property</typeparam>
        /// <param name="commandResultResult">An existing CommandResult to pass along</param>
        /// <param name="message">
        ///     Optional message that is saved to the Message property on the <see cref="CommandResult{T}" />
        /// </param>
        public static CommandResult<T> Ok<T>(CommandResult<T> commandResultResult, string message = null)
        {
            return new CommandResult<T>(commandResultResult.Value, true, message);
        }

        /// <summary>
        ///     Creates a new <see cref="CommandResult" /> with the IsSuccessful property set to false.
        /// </summary>
        /// <param name="message">
        ///     Optional message that is saved to the Message property on the <see cref="CommandResult" />
        /// </param>
        public static CommandResult Fail(string message)
        {
            return new CommandResult(false, message);
        }

        /// <summary>
        ///     Creates a new <see cref="CommandResult{T}" /> with the IsSuccessful property set to false.
        /// </summary>
        /// <typeparam name="T">The type of the underlying Value value</typeparam>
        /// <param name="message">
        ///     Optional message that is saved to the Message property on the <see cref="CommandResult{T}" />
        /// </param>
        /// <param name="result">The underlying value of the new <see cref="CommandResult{T}" /></param>
        public static CommandResult<T> Fail<T>(string message, T result)
        {
            return new CommandResult<T>(result, false, message);
        }

        /// <summary>
        ///     Creates a new <see cref="CommandResult{T}" /> with the IsSuccessful property set to false.
        /// </summary>
        /// <typeparam name="T">The type of the underlying Value value</typeparam>
        /// <param name="message">
        ///     Optional message that is saved to the Message property on the <see cref="CommandResult{T}" />
        /// </param>
        /// <param name="commandResultResult">An existing CommandResult to pass along</param>
        /// <returns></returns>
        public static CommandResult<T> Fail<T>(string message, CommandResult<T> commandResultResult)
        {
            return commandResultResult == null ? new CommandResult<T>(default, false, message) : new CommandResult<T>(commandResultResult.Value, false, message);
        }
    }
}