namespace API.Models.Enums;

public enum EmployeeStatus
{
    Pending = 1,             // Created by HR
    DocumentsSubmitted = 2,  // Employee uploaded required documents
    Active = 3,              // HR approved and employee is active
    Inactive = 4,            // Left or temporarily inactive
    Offboarded = 5           // Employment ended
}
