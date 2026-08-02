export enum DocumentType {
  Resume = 1,
  Aadhaar = 2,
  PAN = 3,
  Passport = 4,
  Degree = 5,
  OfferLetter = 6,
  ExperienceLetter = 7,
  Photo = 8,
  Other = 9
}

export const DOCUMENT_TYPES = [
  { value: DocumentType.Resume, label: 'Resume' },
  { value: DocumentType.Aadhaar, label: 'Aadhaar Card' },
  { value: DocumentType.PAN, label: 'PAN Card' },
  { value: DocumentType.Degree, label: 'Degree' },
  { value: DocumentType.OfferLetter, label: 'Offer Letter' },
  { value: DocumentType.ExperienceLetter, label: 'Experience Letter' },
  { value: DocumentType.Photo, label: 'Photo' },
  { value: DocumentType.Other, label: 'Other' }
];