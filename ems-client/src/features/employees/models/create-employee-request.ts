export interface CreateEmployeeRequest {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  joiningDate: string;
  departmentId: number;
  designationId: number;
  managerId: number | null;
}