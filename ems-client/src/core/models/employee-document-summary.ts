export interface EmployeeDocumentSummary {
    employeeId: number;
    employeeCode: string;
    fullName: string;
    profileImage?: string;
    department: string;
    totalDocuments: number;
}