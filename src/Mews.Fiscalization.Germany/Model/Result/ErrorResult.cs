﻿using Fiskaly.Errors;
using System;

namespace Mews.Fiscalization.Germany.Model
{
    public sealed class ErrorResult
    {
        private ErrorResult(string message, string requestId, FiskalyError error)
        {
            Message = message;
            RequestId = requestId;
            Error = error;
        }

        public string Message { get; }

        public string RequestId { get; }

        public FiskalyError Error { get; }

        internal static ErrorResult Map(FiskalyHttpError error)
        {
            return new ErrorResult(
                message: error.Message,
                requestId: error.RequestId,
                error: MapError(error)
            );
        }

        internal static FiskalyError MapError(FiskalyHttpError error)
        {
            // For some reason, when the credentials are invalid, Fiskaly returns null code but with a message.
            if (error.Code == null && error.Message.Equals("Invalid credentials"))
            {
                return FiskalyError.InvalidCredentials;
            }
            switch (error.Code)
            {
                case "E_TX_UPSERT":
                    return FiskalyError.InvalidTransactionOperation;
                case "E_TSS_DISABLED":
                case "E_TSS_NOT_INITIALIZED":
                    return FiskalyError.InvalidTssOperation;
                case "E_TX_ILLEGAL_TYPE_CHANGE":
                case "E_TX_NO_TYPE_DEFINED":
                case "E_API_VERSION":
                    throw new InvalidOperationException($"Invalid request from the library with error code: {error.Code}");
                case "E_CLIENT_NOT_FOUND":
                    return FiskalyError.InvalidClientId;
                case "E_TSS_NOT_FOUND":
                    return FiskalyError.InvalidTssId;
                default:
                    throw new NotImplementedException($"Error code: {error.Code} is not implemented.");
            }
        }
    }
}