export interface Document {
  id: number;
  originalFileName: string;
  documentType: DocumentType;
  fileSize: number;
  uploadedOn: string;
  employeeId: number;
  employeeName: string;
  url: string;
  publicId: string;
}