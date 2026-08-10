using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Common
{
    public sealed record Result(bool Success, string? Error = null, ResultKind Kind = ResultKind.Ok)
    {
        public static Result OK() => new(true);
        public static Result Failed(string error, ResultKind kind = ResultKind.Conflict) => new(false, error, kind);

        public static Result NotFound(string error = "Not Found") => new(false, error, ResultKind.NotFount);

        public static Result Validation(string error, ResultKind kind = ResultKind.ValidationFailed) => new(false, error, kind);


    }

    public sealed record Result<T>(bool Success, T? value, string? Error = null, ResultKind Kind = ResultKind.Ok)
    {
        public static Result<T> OK(T value) => new(true, value);
        public static Result<T> Failed( string error, ResultKind kind =ResultKind.Conflict ) => new(false,default,error,kind);

        public static Result<T> NotFound(string error = "Not Found") => new(false,default,error);
      
    
    }
   
}
