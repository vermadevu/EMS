import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';

import { EmployeeDocumentSummary } from '../models/employee-document-summary';
import { Document } from '../models/document';
import { DocumentListState } from '../models/document-list-state';
import { PagedResult } from '../../features/employees/models/paged-result';

@Service()
export class DocumentService {

    private readonly http = inject(HttpClient);

    private readonly baseUrl = `${environment.apiUrl}/document`;

    getEmployeeSummary(
        queryParams: DocumentListState
    ): Observable<PagedResult<EmployeeDocumentSummary>> {

        let params = new HttpParams()
            .set('pageNumber', queryParams.pageNumber)
            .set('pageSize', queryParams.pageSize)
            .set('sortBy', queryParams.sortBy)
            .set('sortDirection', queryParams.sortDirection);

        if (queryParams.search) {
            params = params.set('search', queryParams.search);
        }

        return this.http.get<PagedResult<EmployeeDocumentSummary>>(
            `${this.baseUrl}/employee`,
            { params }
        );
    }

    getByEmployee(employeeId: number): Observable<Document[]> {
        return this.http.get<Document[]>(
            `${this.baseUrl}/employee/${employeeId}`
        );
    }

    upload(formData: FormData): Observable<Document> {
        return this.http.post<Document>(
            this.baseUrl,
            formData
        );
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(
            `${this.baseUrl}/${id}`
        );
    }

    getMyDocuments(): Observable<Document[]> {
        return this.http.get<Document[]>(
            `${this.baseUrl}/me`
        );
    }

    uploadMyDocument(formData: FormData): Observable<Document> {
        return this.http.post<Document>(
            `${this.baseUrl}/me`,
            formData
        );
    }

    deleteMyDocument(id: number): Observable<void> {
        return this.http.delete<void>(
            `${this.baseUrl}/me/${id}`
        );
    }

}