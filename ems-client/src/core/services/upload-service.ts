import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { Observable } from 'rxjs';
import { ImageUploadResponse } from '../models/image-upload-response';
import { environment } from '../../environments/environment';

@Service()
export class UploadService {
    private readonly http = inject(HttpClient);

  uploadImage(file: File): Observable<ImageUploadResponse> {

    const formData = new FormData();

    formData.append('file', file);

    return this.http.post<ImageUploadResponse>(
      `${environment.apiUrl}/upload/image`,
      formData
    );
  }
}
