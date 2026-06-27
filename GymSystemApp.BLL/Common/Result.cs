using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.BLL.Common
{
    public record Result(bool IsSuccess, string? ErorrMessage = null,ResultKind Kind = ResultKind.Ok)
    {
        public static Result Ok() => new Result(true);
        public static Result Fail(string errorMassage,ResultKind kind= ResultKind.Conflict) => new Result(false, errorMassage,kind);
        public static Result NotFound(string errorMassage = "Not Found") => new Result(false,errorMassage,ResultKind.NotFound);
        public static Result Validation(string errorMassage = "Validation Failed") => new Result(false, errorMassage, ResultKind.ValidationFailed);
        public static Result Forbidden(string errorMassage = "Forbidden") => new Result(false, errorMassage, ResultKind.Forbidden);

    }
    public record Result<T> (T? Value,bool IsSuccess, string? ErorrMessage = null, ResultKind Kind = ResultKind.Ok)
    {
        public static Result<T> Ok(T value) => new (value,true);
        public static Result<T> Fail(string errorMassage, ResultKind kind = ResultKind.Conflict) => new (default,false, errorMassage, kind);
        public static Result<T> NotFound(string errorMassage = "Not Found") => new (default,false, errorMassage, ResultKind.NotFound);

    }

    public enum ResultKind
    {
        Ok,
        NotFound,
        Conflict,
        ValidationFailed,
        Forbidden
    }
}
