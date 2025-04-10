﻿namespace DATN_Models.Models
{
    public class ErrorLog
    {
        public int ErrorLogID { get; set; }
        public DateTime ErrorTime { get; set; }
        public string? UserName { get; set; }
        public string? HostName { get; set; }
        public int ErrorNumber { get; set; }
        public int? ErrorCode { get; set; }
        public int? ErrorSeverity { get; set; }
        public int? ErrorState { get; set; }
        public string? ErrorProcedure { get; set; }
        public int? ErrorLine { get; set; }
        public string ErrorMessage { get; set; }
    }
}
