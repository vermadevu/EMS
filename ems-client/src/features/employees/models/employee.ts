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
  dateOfBirth: string | null;
  gender: string | null;
  bloodGroup: string | null;
  address: string | null;
  city: string | null;
  state: string | null;
  country: string | null;
  emergencyContactName: string | null;
  emergencyContactPhone: string | null;
  emergencyContactRelationship: string | null;
  departmentId: number;
  departmentName: string;
  designationId: number;
  designationName: string;
  managerId: number | null;
  managerName: string | null;
}