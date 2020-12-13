using System;
using System.Threading.Tasks;

namespace Hallmanac.CryptoHelpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class FunctionalCommandExtensions
    {
        #region Then

        /// <summary>
        ///     Chains a method onto a <see cref="CommandResult{T}" />.
        /// </summary>
        /// <param name="callback">Method to be used as long as <see cref="CommandResult.IsSuccessful" /> is true</param>
        public static CommandResult<TOutput> Then<TOutput>(this CommandResult @this, Func<CommandResult<TOutput>> callback)
        {
            if (@this.IsFailure) return CommandResultFactory.Fail<TOutput>(@this.Message, null);
            var funq = callback();
            return funq;
        }


        /// <summary>
        ///     Chains a method onto a <see cref="CommandResult{T}" />.
        /// </summary>
        /// <param name="callback">Method to be used as long as <see cref="CommandResult.IsSuccessful" /> is true</param>
        public static CommandResult<TOutput> Then<TInput, TOutput>(this CommandResult<TInput> @this, Func<TInput, CommandResult<TOutput>> callback)
        {
            if (@this.IsFailure) return CommandResultFactory.Fail(@this.Message, default(TOutput));
            var result = callback(@this.Value);
            return result;
        }


        /// <summary>
        ///     Chains a method onto a <see cref="CommandResult{T}" />.
        /// </summary>
        /// <param name="callback">Method to be used as long as <see cref="CommandResult.IsSuccessful" /> is true</param>
        public static CommandResult Then(this CommandResult @this, Func<CommandResult> callback)
        {
            return @this.IsFailure ? CommandResultFactory.Fail(@this.Message) : callback();
        }


        /// <summary>
        ///     Chains a method onto a <see cref="CommandResult{T}" />.
        /// </summary>
        /// <param name="callback">Method to be used as long as <see cref="CommandResult.IsSuccessful" /> is true</param>
        public static CommandResult Then<TInput>(this CommandResult<TInput> @this, Func<TInput, CommandResult> callback)
        {
            return @this.IsFailure ? CommandResultFactory.Fail(@this.Message) : callback(@this.Value);
        }

        #endregion

        #region Then Async

        /// <summary>
        ///     Chains a method onto a <see cref="CommandResult{T}" /> in an async manner to allow for Async usage.
        /// </summary>
        /// <param name="callback">Method to be used as long as <see cref="CommandResult.IsSuccessful" /> is true</param>
        public static async Task<CommandResult<TOutput>> ThenAsync<TOutput>(this Task<CommandResult> thisTask, Func<Task<CommandResult<TOutput>>> callback)
        {
            var @this = await thisTask;
            if (@this.IsFailure) return await Task.FromResult(CommandResultFactory.Fail<TOutput>(@this.Message, null)).ConfigureAwait(false);
            var funq = await callback().ConfigureAwait(false);
            return funq;
        }


        /// <summary>
        ///     Chains a method onto a <see cref="CommandResult{T}" /> in an async manner to allow for Async usage.
        /// </summary>
        /// <param name="callback">Method to be used as long as <see cref="CommandResult.IsSuccessful" /> is true</param>
        public static async Task<CommandResult<TOutput>> ThenAsync<TOutput>(this Task<CommandResult<TOutput>> thisTask, Func<TOutput, Task<CommandResult<TOutput>>> callback)
        {
            var @this = await thisTask;
            if (@this.IsFailure) return await Task.FromResult(CommandResultFactory.Fail(@this.Message, default(TOutput)));
            var funq = await callback(@this.Value).ConfigureAwait(false);
            return funq;
        }


        /// <summary>
        ///     Chains a method onto a <see cref="CommandResult{T}" /> in an async manner to allow for Async usage.
        /// </summary>
        /// <param name="callback">Method to be used as long as <see cref="CommandResult.IsSuccessful" /> is true</param>
        public static async Task<CommandResult<TOutput>> ThenAsync<TInput, TOutput>(this Task<CommandResult<TInput>> thisTask, Func<TInput, Task<CommandResult<TOutput>>> callback)
        {
            var @this = await thisTask.ConfigureAwait(false);
            if (@this.IsFailure) return await Task.FromResult(CommandResultFactory.Fail(@this.Message, default(TOutput))).ConfigureAwait(false);
            var funq = await callback(@this.Value).ConfigureAwait(false);
            return funq;
        }


        /// <summary>
        ///     Chains a method onto a <see cref="CommandResult{T}" /> in an async manner to allow for Async usage.
        /// </summary>
        /// <param name="callback">Method to be used as long as <see cref="CommandResult.IsSuccessful" /> is true</param>
        public static async Task<CommandResult> ThenAsync(this Task<CommandResult> thisTask, Func<Task<CommandResult>> callback)
        {
            var @this = await thisTask;
            if (@this.IsFailure) return await Task.FromResult(CommandResultFactory.Fail(@this.Message)).ConfigureAwait(false);
            var funq = await callback().ConfigureAwait(false);
            return funq;
        }


        /// <summary>
        ///     Chains a method onto a <see cref="CommandResult{T}" /> in an async manner to allow for Async usage.
        /// </summary>
        /// <param name="callback">Method to be used as long as <see cref="CommandResult.IsSuccessful" /> is true</param>
        public static async Task<CommandResult> ThenAsync<TInput>(this Task<CommandResult<TInput>> thisTask, Func<TInput, Task<CommandResult>> callback)
        {
            var @this = await thisTask;
            if (@this.IsFailure) return await Task.FromResult(CommandResultFactory.Fail(@this.Message)).ConfigureAwait(false);
            var funq = await callback(@this.Value).ConfigureAwait(false);
            return funq;
        }

        #endregion


        #region Catch

        /// <summary>
        ///     This is a way to chain a error handling callback method onto the end of the method chain. The "Catch" will always
        ///     call the callback
        ///     method parameter. The callback itself is responsible for whether or not to pass along a success or fail.
        ///     <para>
        ///         For example: When there is an error and there is a way for the error to be corrected or an attempt to correct
        ///         the error can be made. The Catch
        ///         chained method can correct the error and then allow the method chain to continue in a successful manner.
        ///     </para>
        ///     <para>
        ///         Another example is where the Catch method can log an error.
        ///     </para>
        /// </summary>
        /// <param name="callback">Method to be called (no matter what).</param>
        public static CommandResult<TOutput> Catch<TInput, TOutput>(this CommandResult<TInput> @this, Func<CommandResult<TInput>, CommandResult<TOutput>> callback)
        {
            // Catch should always call its callback. The Implementation of the catch callback should be responsible for returning an OK or a Fail
            var errorFunq = callback(@this);
            return errorFunq;
        }


        /// <summary>
        ///     This is a way to chain a error handling callback method onto the end of the method chain. The "Catch" will always
        ///     call the callback
        ///     method parameter. The callback itself is responsible for whether or not to pass along a success or fail.
        ///     <para>
        ///         For example: When there is an error and there is a way for the error to be corrected or an attempt to correct
        ///         the error can be made. The Catch
        ///         chained method can correct the error and then allow the method chain to continue in a successful manner.
        ///     </para>
        ///     <para>
        ///         Another example is where the Catch method can log an error.
        ///     </para>
        /// </summary>
        /// <param name="callback">Method to be called (no matter what).</param>
        public static CommandResult Catch<TInput>(this CommandResult<TInput> @this, Func<CommandResult<TInput>, CommandResult> callback)
        {
            if (@this.IsSuccessful) return CommandResultFactory.Ok(@this.Message);
            var funq = callback(@this);
            return funq;
        }


        /// <summary>
        ///     This is a way to chain a error handling callback method onto the end of the method chain. The "Catch" will always
        ///     call the callback
        ///     method parameter. The callback itself is responsible for whether or not to pass along a success or fail.
        ///     <para>
        ///         For example: When there is an error and there is a way for the error to be corrected or an attempt to correct
        ///         the error can be made. The Catch
        ///         chained method can correct the error and then allow the method chain to continue in a successful manner.
        ///     </para>
        ///     <para>
        ///         Another example is where the Catch method can log an error.
        ///     </para>
        /// </summary>
        /// <param name="callback">Method to be called (no matter what).</param>
        public static CommandResult Catch(this CommandResult @this, Func<CommandResult, CommandResult> callback)
        {
            if (@this.IsSuccessful) return CommandResultFactory.Ok(@this.Message);
            var errorFunq = callback(@this);
            return errorFunq;
        }

        #endregion

        #region Catch Async

        /// <summary>
        ///     This is an async way to chain a error handling callback method onto the end of the method chain. The "Catch" will
        ///     always call the callback
        ///     method parameter. The callback itself is responsible for whether or not to pass along a success or fail.
        ///     <para>
        ///         For example: When there is an error and there is a way for the error to be corrected or an attempt to correct
        ///         the error can be made. The Catch
        ///         chained method can correct the error and then allow the method chain to continue in a successful manner.
        ///     </para>
        ///     <para>
        ///         Another example is where the Catch method can log an error.
        ///     </para>
        /// </summary>
        /// <param name="callback">Method to be called (no matter what).</param>
        public static async Task<CommandResult<TOutput>> CatchAsync<TOutput>(this Task<CommandResult<TOutput>> thisTask, Func<Task<CommandResult<TOutput>>> callback)
        {
            var @this = await thisTask.ConfigureAwait(false);
            if (@this.IsSuccessful) return await Task.FromResult(CommandResultFactory.Ok(@this.Value, @this.Message)).ConfigureAwait(false);
            var errorFunq = await callback().ConfigureAwait(false);
            return errorFunq;
        }


        /// <summary>
        ///     This is an async way to chain a error handling callback method onto the end of the method chain. The "Catch" will
        ///     always call the callback
        ///     method parameter. The callback itself is responsible for whether or not to pass along a success or fail.
        ///     <para>
        ///         For example: When there is an error and there is a way for the error to be corrected or an attempt to correct
        ///         the error can be made. The Catch
        ///         chained method can correct the error and then allow the method chain to continue in a successful manner.
        ///     </para>
        ///     <para>
        ///         Another example is where the Catch method can log an error.
        ///     </para>
        /// </summary>
        /// <param name="callback">Method to be called (no matter what).</param>
        public static async Task<CommandResult<TOutput>> CatchAsync<TInput, TOutput>(this Task<CommandResult<TInput>> thisTask, Func<CommandResult<TInput>, Task<CommandResult<TOutput>>> callback)
        {
            var @this = await thisTask.ConfigureAwait(false);

            var errorFunq = await callback(@this).ConfigureAwait(false);
            return errorFunq;
        }


        /// <summary>
        ///     This is an async way to chain a error handling callback method onto the end of the method chain. The "Catch" will
        ///     always call the callback
        ///     method parameter. The callback itself is responsible for whether or not to pass along a success or fail.
        ///     <para>
        ///         For example: When there is an error and there is a way for the error to be corrected or an attempt to correct
        ///         the error can be made. The Catch
        ///         chained method can correct the error and then allow the method chain to continue in a successful manner.
        ///     </para>
        ///     <para>
        ///         Another example is where the Catch method can log an error.
        ///     </para>
        /// </summary>
        /// <param name="callback">Method to be called (no matter what).</param>
        public static async Task<CommandResult> CatchAsync<TInput>(this Task<CommandResult<TInput>> thisTask, Func<Task<CommandResult>> callback)
        {
            var @this = await thisTask.ConfigureAwait(false);
            if (@this.IsSuccessful) return await Task.FromResult(CommandResultFactory.Ok(@this.Message)).ConfigureAwait(false);
            var funq = await callback().ConfigureAwait(false);
            return funq;
        }


        /// <summary>
        ///     This is an async way to chain a error handling callback method onto the end of the method chain. The "Catch" will
        ///     always call the callback
        ///     method parameter. The callback itself is responsible for whether or not to pass along a success or fail.
        ///     <para>
        ///         For example: When there is an error and there is a way for the error to be corrected or an attempt to correct
        ///         the error can be made. The Catch
        ///         chained method can correct the error and then allow the method chain to continue in a successful manner.
        ///     </para>
        ///     <para>
        ///         Another example is where the Catch method can log an error.
        ///     </para>
        /// </summary>
        /// <param name="callback">Method to be called (no matter what).</param>
        public static async Task<CommandResult> CatchAsync(this Task<CommandResult> thisTask, Func<Task<CommandResult>> callback)
        {
            var @this = await thisTask.ConfigureAwait(false);
            if (@this.IsSuccessful) return await Task.FromResult(CommandResultFactory.Ok(@this.Message)).ConfigureAwait(false);
            var errorFunq = await callback().ConfigureAwait(false);
            return errorFunq;
        }

        #endregion


        #region Finally

        /// <summary>
        ///     This is a way to chain a "Finally" handling callback method onto the end of the method chain. You can think of this
        ///     as being
        ///     similar to a "finally" call in .Net. The "Finally" will always call the callback method parameter. The callback
        ///     itself can
        ///     be responsible for whether or not to pass along a success or fail. There are also overloads that will simply call
        ///     the callback
        ///     and return the current success/fail status.
        ///     <para>
        ///         For example: When there is an error and there is a way for the error to be corrected or an attempt to correct
        ///         the error can be made. The Catch
        ///         chained method can correct the error and then allow the method chain to continue in a successful manner.
        ///     </para>
        ///     <para>
        ///         Another example is where the Catch method can log an error.
        ///     </para>
        /// </summary>
        /// <param name="callback">Method to be called (no matter what).</param>
        public static CommandResult<TOutput> Finally<TInput, TOutput>(this CommandResult<TInput> @this, Func<CommandResult<TInput>, CommandResult<TOutput>> callback)
        {
            var result = @this.IsSuccessful ? callback(CommandResultFactory.Ok(@this, @this.Message)) : callback(CommandResultFactory.Fail(@this.Message, @this));
            return result;
        }


        /// <summary>
        ///     This is a way to chain a "Finally" handling callback method onto the end of the method chain. You can think of this
        ///     as being
        ///     similar to a "finally" call in .Net. The "Finally" will always call the callback method parameter. The callback
        ///     itself can
        ///     be responsible for whether or not to pass along a success or fail. There are also overloads that will simply call
        ///     the callback
        ///     and return the current success/fail status.
        ///     <para>
        ///         For example: When there is an error and there is a way for the error to be corrected or an attempt to correct
        ///         the error can be made. The Catch
        ///         chained method can correct the error and then allow the method chain to continue in a successful manner.
        ///     </para>
        ///     <para>
        ///         Another example is where the Catch method can log an error.
        ///     </para>
        /// </summary>
        /// <param name="callback">Method to be called (no matter what).</param>
        public static CommandResult Finally<TInput>(this CommandResult<TInput> @this, Func<CommandResult<TInput>, CommandResult> callback)
        {
            var result = @this.IsSuccessful
                ? callback(CommandResultFactory.Ok(@this, @this.Message))
                : callback(CommandResultFactory.Fail(@this.Message, @this));
            return result;
        }


        /// <summary>
        ///     This is a way to chain a "Finally" handling callback method onto the end of the method chain. You can think of this
        ///     as being
        ///     similar to a "finally" call in .Net. The "Finally" will always call the callback method parameter. The callback
        ///     itself can
        ///     be responsible for whether or not to pass along a success or fail. There are also overloads that will simply call
        ///     the callback
        ///     and return the current success/fail status.
        ///     <para>
        ///         For example: When there is an error and there is a way for the error to be corrected or an attempt to correct
        ///         the error can be made. The Catch
        ///         chained method can correct the error and then allow the method chain to continue in a successful manner.
        ///     </para>
        ///     <para>
        ///         Another example is where the Catch method can log an error.
        ///     </para>
        /// </summary>
        /// <param name="callback">Method to be called (no matter what).</param>
        public static CommandResult<TOutput> Finally<TOutput>(this CommandResult @this, Func<CommandResult, CommandResult<TOutput>> callback)
        {
            var result = @this.IsSuccessful
                ? callback(CommandResultFactory.Ok(@this.Message))
                : callback(CommandResultFactory.Fail(@this.Message));
            return result;
        }


        /// <summary>
        ///     This is a way to chain a "Finally" handling callback method onto the end of the method chain. You can think of this
        ///     as being
        ///     similar to a "finally" call in .Net. The "Finally" will always call the callback method parameter. The callback
        ///     itself can
        ///     be responsible for whether or not to pass along a success or fail. There are also overloads that will simply call
        ///     the callback
        ///     and return the current success/fail status.
        ///     <para>
        ///         For example: When there is an error and there is a way for the error to be corrected or an attempt to correct
        ///         the error can be made. The Catch
        ///         chained method can correct the error and then allow the method chain to continue in a successful manner.
        ///     </para>
        ///     <para>
        ///         Another example is where the Catch method can log an error.
        ///     </para>
        /// </summary>
        /// <param name="callback">Method to be called (no matter what).</param>
        public static CommandResult Finally(this CommandResult @this, Func<CommandResult, CommandResult> callback)
        {
            var result = @this.IsSuccessful ? callback(CommandResultFactory.Ok(@this)) : callback(CommandResultFactory.Fail(@this.Message, @this));
            return result;
        }


        /// <summary>
        ///     This is a way to chain a "Finally" handling callback method onto the end of the method chain. You can think of this
        ///     as being
        ///     similar to a "finally" call in .Net. The "Finally" will always call the callback method parameter. The callback
        ///     itself can
        ///     be responsible for whether or not to pass along a success or fail. There are also overloads that will simply call
        ///     the callback
        ///     and return the current success/fail status.
        ///     <para>
        ///         For example: When there is an error and there is a way for the error to be corrected or an attempt to correct
        ///         the error can be made. The Catch
        ///         chained method can correct the error and then allow the method chain to continue in a successful manner.
        ///     </para>
        ///     <para>
        ///         Another example is where the Catch method can log an error.
        ///     </para>
        /// </summary>
        /// <param name="callback">Method to be called (no matter what).</param>
        public static CommandResult<TInput> Finally<TInput>(this CommandResult<TInput> @this, Action<CommandResult<TInput>> callback)
        {
            if (@this.IsSuccessful)
            {
                var okResult = CommandResultFactory.Ok(@this, @this.Message);
                callback(okResult);
                return okResult;
            }

            var failResult = CommandResultFactory.Fail(@this.Message, @this);
            callback(failResult);
            return failResult;
        }


        /// <summary>
        ///     This is a way to chain a "Finally" handling callback method onto the end of the method chain. You can think of this
        ///     as being
        ///     similar to a "finally" call in .Net. The "Finally" will always call the callback method parameter. The callback
        ///     itself can
        ///     be responsible for whether or not to pass along a success or fail. There are also overloads that will simply call
        ///     the callback
        ///     and return the current success/fail status.
        ///     <para>
        ///         For example: When there is an error and there is a way for the error to be corrected or an attempt to correct
        ///         the error can be made. The Catch
        ///         chained method can correct the error and then allow the method chain to continue in a successful manner.
        ///     </para>
        ///     <para>
        ///         Another example is where the Catch method can log an error.
        ///     </para>
        /// </summary>
        /// <param name="callback">Method to be called (no matter what).</param>
        public static CommandResult Finally(this CommandResult @this, Action<CommandResult> callback)
        {
            if (@this.IsSuccessful)
            {
                var okResult = CommandResultFactory.Ok(@this, @this.Message);
                callback(okResult);
                return okResult;
            }

            var failResult = CommandResultFactory.Fail(@this.Message);
            callback(failResult);
            return failResult;
        }

        #endregion

        #region Finally Async

        /// <summary>
        ///     This is an async way to chain a "Finally" handling callback method onto the end of the method chain. You can think
        ///     of this as being
        ///     similar to a "finally" call in .Net. The "Finally" will always call the callback method parameter. The callback
        ///     itself can
        ///     be responsible for whether or not to pass along a success or fail. There are also overloads that will simply call
        ///     the callback
        ///     and return the current success/fail status.
        ///     <para>
        ///         For example: When there is an error and there is a way for the error to be corrected or an attempt to correct
        ///         the error can be made. The Catch
        ///         chained method can correct the error and then allow the method chain to continue in a successful manner.
        ///     </para>
        ///     <para>
        ///         Another example is where the Catch method can log an error.
        ///     </para>
        /// </summary>
        /// <param name="callback">Method to be called (no matter what).</param>
        public static async Task<CommandResult<TOutput>> FinallyAsync<TInput, TOutput>(this Task<CommandResult<TInput>> thisAsync, Func<CommandResult<TInput>, Task<CommandResult<TOutput>>> callback)
        {
            var @this = await thisAsync.ConfigureAwait(false);
            var result = @this.IsSuccessful
                ? await callback(CommandResultFactory.Ok(@this, @this.Message)).ConfigureAwait(false)
                : await callback(CommandResultFactory.Fail(@this.Message, @this)).ConfigureAwait(false);
            return result;
        }


        /// <summary>
        ///     This is an async way to chain a "Finally" handling callback method onto the end of the method chain. You can think
        ///     of this as being
        ///     similar to a "finally" call in .Net. The "Finally" will always call the callback method parameter. The callback
        ///     itself can
        ///     be responsible for whether or not to pass along a success or fail. There are also overloads that will simply call
        ///     the callback
        ///     and return the current success/fail status.
        ///     <para>
        ///         For example: When there is an error and there is a way for the error to be corrected or an attempt to correct
        ///         the error can be made. The Catch
        ///         chained method can correct the error and then allow the method chain to continue in a successful manner.
        ///     </para>
        ///     <para>
        ///         Another example is where the Catch method can log an error.
        ///     </para>
        /// </summary>
        /// <param name="callback">Method to be called (no matter what).</param>
        public static async Task<CommandResult<TOutput>> FinallyAsync<TOutput>(this Task<CommandResult> thisAsync, Func<CommandResult, Task<CommandResult<TOutput>>> callback)
        {
            var @this = await thisAsync.ConfigureAwait(false);
            var result = @this.IsSuccessful
                ? await callback(CommandResultFactory.Ok(@this, @this.Message)).ConfigureAwait(false)
                : await callback(CommandResultFactory.Fail(@this.Message, @this)).ConfigureAwait(false);
            return result;
        }


        /// <summary>
        ///     This is an async way to chain a "Finally" handling callback method onto the end of the method chain. You can think
        ///     of this as being
        ///     similar to a "finally" call in .Net. The "Finally" will always call the callback method parameter. The callback
        ///     itself can
        ///     be responsible for whether or not to pass along a success or fail. There are also overloads that will simply call
        ///     the callback
        ///     and return the current success/fail status.
        ///     <para>
        ///         For example: When there is an error and there is a way for the error to be corrected or an attempt to correct
        ///         the error can be made. The Catch
        ///         chained method can correct the error and then allow the method chain to continue in a successful manner.
        ///     </para>
        ///     <para>
        ///         Another example is where the Catch method can log an error.
        ///     </para>
        /// </summary>
        /// <param name="callback">Method to be called (no matter what).</param>
        public static async Task<CommandResult> FinallyAsync<TInput>(this Task<CommandResult<TInput>> thisAsync, Func<CommandResult<TInput>, Task<CommandResult>> callback)
        {
            var @this = await thisAsync.ConfigureAwait(false);
            var result = @this.IsSuccessful
                ? await callback(CommandResultFactory.Ok(@this, @this.Message)).ConfigureAwait(false)
                : await callback(CommandResultFactory.Fail(@this.Message, @this)).ConfigureAwait(false);
            return result;
        }


        /// <summary>
        ///     This is an async way to chain a "Finally" handling callback method onto the end of the method chain. You can think
        ///     of this as being
        ///     similar to a "finally" call in .Net. The "Finally" will always call the callback method parameter. The callback
        ///     itself can
        ///     be responsible for whether or not to pass along a success or fail. There are also overloads that will simply call
        ///     the callback
        ///     and return the current success/fail status.
        ///     <para>
        ///         For example: When there is an error and there is a way for the error to be corrected or an attempt to correct
        ///         the error can be made. The Catch
        ///         chained method can correct the error and then allow the method chain to continue in a successful manner.
        ///     </para>
        ///     <para>
        ///         Another example is where the Catch method can log an error.
        ///     </para>
        /// </summary>
        /// <param name="callback">Method to be called (no matter what).</param>
        public static async Task<CommandResult> FinallyAsync(this Task<CommandResult> thisAsync, Func<CommandResult, Task<CommandResult>> callback)
        {
            var @this = await thisAsync.ConfigureAwait(false);
            var result = @this.IsSuccessful
                ? await callback(CommandResultFactory.Ok(@this)).ConfigureAwait(false)
                : await callback(CommandResultFactory.Fail(@this.Message, @this)).ConfigureAwait(false);
            return result;
        }


        /// <summary>
        ///     This is an async way to chain a "Finally" handling callback method onto the end of the method chain. You can think
        ///     of this as being
        ///     similar to a "finally" call in .Net. The "Finally" will always call the callback method parameter. The callback
        ///     itself can
        ///     be responsible for whether or not to pass along a success or fail. There are also overloads that will simply call
        ///     the callback
        ///     and return the current success/fail status.
        ///     <para>
        ///         For example: When there is an error and there is a way for the error to be corrected or an attempt to correct
        ///         the error can be made. The Catch
        ///         chained method can correct the error and then allow the method chain to continue in a successful manner.
        ///     </para>
        ///     <para>
        ///         Another example is where the Catch method can log an error.
        ///     </para>
        /// </summary>
        /// <param name="callback">Method to be called (no matter what).</param>
        public static async Task<CommandResult<TInput>> Finally<TInput>(this Task<CommandResult<TInput>> thisAsync, Func<CommandResult<TInput>, Task> callback)
        {
            var @this = await thisAsync.ConfigureAwait(false);
            if (@this.IsSuccessful)
            {
                var okResult = CommandResultFactory.Ok(@this, @this.Message);
                await callback(okResult).ConfigureAwait(false);
                return okResult;
            }

            var failResult = CommandResultFactory.Fail(@this.Message, @this);
            await callback(failResult).ConfigureAwait(false);
            return failResult;
        }


        /// <summary>
        ///     This is an async way to chain a "Finally" handling callback method onto the end of the method chain. You can think
        ///     of this as being
        ///     similar to a "finally" call in .Net. The "Finally" will always call the callback method parameter. The callback
        ///     itself can
        ///     be responsible for whether or not to pass along a success or fail. There are also overloads that will simply call
        ///     the callback
        ///     and return the current success/fail status.
        ///     <para>
        ///         For example: When there is an error and there is a way for the error to be corrected or an attempt to correct
        ///         the error can be made. The Catch
        ///         chained method can correct the error and then allow the method chain to continue in a successful manner.
        ///     </para>
        ///     <para>
        ///         Another example is where the Catch method can log an error.
        ///     </para>
        /// </summary>
        /// <param name="callback">Method to be called (no matter what).</param>
        public static async Task<CommandResult> Finally(this Task<CommandResult> thisAsync, Func<CommandResult, Task> callback)
        {
            var @this = await thisAsync.ConfigureAwait(false);
            if (@this.IsSuccessful)
            {
                var okResult = CommandResultFactory.Ok(@this, @this.Message);
                await callback(okResult).ConfigureAwait(false);
                return okResult;
            }

            var failResult = CommandResultFactory.Fail(@this.Message);
            await callback(failResult).ConfigureAwait(false);
            return failResult;
        }

        #endregion
    }
}