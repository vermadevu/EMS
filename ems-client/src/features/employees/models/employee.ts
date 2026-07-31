
export interface Employee {
  id: number;
  employeeCode: string;
  fullName: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  joiningDate: string;
  status: string;
  profileImage: string | null;
  departmentId: number;
  departmentName: string;
  designationId: number;
  designationName: string;
  managerId: number | null;
  managerName: string | null;
}