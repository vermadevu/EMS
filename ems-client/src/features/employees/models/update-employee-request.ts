import { CreateEmployeeRequest } from "./create-employee-request";

export type UpdateEmployeeRequest = {
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
    joiningDate: string;
    departmentId: number;
    designationId: number;
    managerId: number | null;
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

}