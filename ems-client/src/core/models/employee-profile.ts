export interface EmployeeProfile {
  // Read Only
  employeeCode: string;

  firstName: string;
  lastName: string;
  fullName: string;

  email: string;
  phone: string;

  joiningDate: string;

  departmentName: string;
  designationName: string;
  managerName: string | null;

  // Editable
  address: string | null;
  city: string | null;
  state: string | null;
  country: string | null;
  profileImage: string | null;

  dateOfBirth: string | null;
  gender: string | null;
  bloodGroup: string | null;

  emergencyContactName: string | null;
  emergencyContactRelationship: string | null;
  emergencyContactPhone: string | null;
}