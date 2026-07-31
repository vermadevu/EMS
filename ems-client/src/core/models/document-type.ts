export enum DocumentType {
  Aadhaar = 0,
  PAN = 1,
  Resume = 2,
  OfferLetter = 3,
  Other = 4
}

export const DOCUMENT_TYPES = [
  { value: DocumentType.Aadhaar, label: 'Aadhaar Card' },
  { value: DocumentType.PAN, label: 'PAN Card' },
  { value: DocumentType.Resume, label: 'Resume' },
  { value: DocumentType.OfferLetter, label: 'Offer Letter' },
  { value: DocumentType.Other, label: 'Other' }
];