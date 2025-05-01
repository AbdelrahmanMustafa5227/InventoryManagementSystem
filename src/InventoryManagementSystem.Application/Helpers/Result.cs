using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InventoryManagementSystem.Application.Helpers
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public bool IsFailure => !IsSuccess;
        public Error? Error { get; set; }

        public Result(bool isSuccess, Error? errors)
        {
            IsSuccess = isSuccess;
            Error = errors;
        }

        public static Result Success() => new Result(true, null);
        public static Result Failure(Error error) => new Result(false, error);

        public static Result<T> Success<T>(T value) => new Result<T>(value, true, null);
        public static Result<T> Failure<T>(Error error) => new Result<T>(default, false, error);

        public static Result<T> Create<T>(T value) => value is not null ? Success<T>(value) : Failure<T>(Error.Null);
    }

    public class Result<T> : Result
    {
        private readonly T? _value;

        public Result(T? value, bool isSuccess, Error? error) : base(isSuccess, error)
        {
            _value = value;
        }

        public T Value => IsSuccess ? _value! : throw new InvalidOperationException()!;

        public static implicit operator Result<T>(T value) => Create(value);
    }

}

